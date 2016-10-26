// idreader2.cpp : Defines the entry point for the application.
//

#include "stdafx.h"
#include "idreader2.h"
#include <limits.h>

#define READERTHREAD
#define UITHREAD

UITHREAD HINSTANCE hInst;
UITHREAD WCHAR szTitle[MAX_LOADSTRING];
UITHREAD WCHAR szWindowClass[MAX_LOADSTRING];

READERTHREAD CHAR cFormatBuffer[FORMAT_BUFFER_SIZE];
READERTHREAD WCHAR szFormatBuffer[FORMAT_BUFFER_SIZE];
READERTHREAD uint64_t *idCodeBuffer;
uint64_t idCodeBufferSize = 500;
uint64_t lastIdCodeIndex = 0;

bool shouldClose = false;

const wchar_t* statusString = L"Laeb...";

/// Command that instructs the smart card to open the root folder of its FS.
const BYTE t0CmdChooseRootFolder[] = { 0x00, 0xA4, 0x00, 0x0C };
/// Command that instructs the smart card to open the folder 0xEEEE of its FS.
const BYTE t0CmdChooseFolderEEEE[] = { 0x00, 0xA4, 0x01, 0x0C, 0x02, 0xEE, 0xEE };
/// Command that instructs the smart card to select the file 0x5044 in its FS.
const BYTE t0CmdChooseFile5044[]   = { 0x00, 0xA4, 0x02, 0x04, 0x02, 0x50, 0x44 };

/// Command that instructs the smart card to prepare a personal data file record for reading.
/// Add the record ID to the byte #3, following this:
/// https://eid.eesti.ee/index.php/Creating_new_eID_applications#Using_the_personal_data_file
const BYTE t0CmdRequestRecord[]    = { 0x00, 0xB2, /* record ID */ 0x00, 0x04 };

/// Command that requests a record to be sent, implying it was previously requested.
/// The smart card record request sends the byte count, which can be used here as byte 5.
/// https://eid.eesti.ee/index.php/Creating_new_eID_applications#Using_the_personal_data_file 
const BYTE t0CmdReadBytes[]        = { 0x00, 0xC0, 0x00, 0x00, /* byte count */ 0x00 };

/// The reader is a state machine - here are its possible states.
enum readerState { WAITING_FOR_READER, TRYING_FOR_CARD, READING_CARD, WAITING_REMOVE };

/// This is the data we get to collect off the reader.
struct person {
	wchar_t idNumber[EID_LEN_IDNUMBER + 1];
	wchar_t lastName[EID_LEN_LAST_NAME + 1];
	wchar_t firstName[EID_LEN_NAME_1 + 1];
};

int wmain(int argc, wchar_t **argv) {
	std::thread sCardThread (sCardReaderThread);
	sCardThread.join();
    return 0;
}


void sCardReaderThread() {
	enum readerState readerState = WAITING_FOR_READER;
	
	struct person person;
	
	ZeroMemory(&person, sizeof(struct person));
	
	idCodeBuffer = (uint64_t *) calloc(idCodeBufferSize, sizeof(uint64_t));

	/// Any smart card related function will return an error code.
	LONG sCardErrorCode;
	
	/// To interact with the smart card, a context is required.
	SCARDCONTEXT sCardContext;
	
	/// Size of the LPTSTR that receives all reader names.
	/// Setting it to SCARD_AUTOALLOCATE will let the SCard library allocate the LPTSTR,
	/// and when it does, override the value.
	DWORD sCardReaders = READER_NAME_BUFFER_SIZE;
	
	/// Receives all available readers as names. ["Reader 1", "Reader 2", ""]
	/// ['R', 'e', ..., '1', '\0', 'R', 'e', ..., '2', '\0', '\0']
	wchar_t sCardSzReaders[READER_NAME_BUFFER_SIZE];
	
	ZeroMemory(&sCardSzReaders, READER_NAME_BUFFER_SIZE);

	/// Handle to a Smart Card that we've connected to.
	SCARDHANDLE sCardHandle;

	/// Receives the protocol that we've used, will be one from the list that we accept.
	DWORD sCardActiveProtocol;
	
	/// Generic byte buffer for data that's received from the smart card.
	BYTE sCardRecvBuffer[SCARD_RECVSIZE];
	ZeroMemory(&sCardRecvBuffer, SCARD_RECVSIZE);

	/// Use for passing the size of the receive buffer, and receiving the amount of data
	/// received.
	DWORD sCardRecvBytes = 0;

	SCARD_READERSTATE sCardReaderState;
	
	/// Time handling structs
	time_t currentTimet;
	tm currentTime;

	/// File handle for output
	std::wofstream outputFile;

	/// Reader states
	std::wofstream outputLog;

	// 0. open files for output
	currentTimet = time(0);
	localtime_s(&currentTime, &currentTimet);
	
	wcsftime(szFormatBuffer, FORMAT_BUFFER_SIZE, L"andmed-%Y-%m-%d-%H-%M-%S.csv", &currentTime);
	outputFile.open(szFormatBuffer, std::ios_base::app);
	
	wcsftime(szFormatBuffer, FORMAT_BUFFER_SIZE, L"logi-%Y-%m-%d-%H-%M-%S.txt", &currentTime);
	outputLog.open(szFormatBuffer, std::ios_base::app);

	wcsftime(szFormatBuffer, FORMAT_BUFFER_SIZE, L"%Y-%m-%d %H:%M:%S %z", &currentTime);

	// enable throwing on the failbit
	outputFile.exceptions(std::ios::badbit | std::ios::failbit);
	outputLog.exceptions(std::ios::badbit | std::ios::failbit);

	if (!outputFile.is_open()) {
		std::wcerr << "3001 Ei suutnud avada ID-kaardi logimise faili." << std::endl;
		goto teardown;
	}

	if (!outputLog.is_open()) {
		std::wcerr << "3002 Ei suutnud avada vealogide faili." << std::endl;
		goto teardown;
	}

	std::wcout << "0 === Alustan logimist: " << szFormatBuffer << L" ===" << std::endl;

	// 1. establish a security context
	sCardErrorCode = SCardEstablishContext(SCARD_SCOPE_USER, NULL, NULL, &sCardContext);
	SCARD_FATAL_ERROR(hWnd, &outputLog, sCardErrorCode);

	// Set to Notification to instantly receive notification when a card reader is inserted
	ZeroMemory(&sCardReaderState, sizeof(sCardReaderState));
	sCardReaderState.szReader = L"\\\\?PnP?\\Notification";
	sCardReaderState.pvUserData = nullptr;
	sCardReaderState.dwEventState = SCARD_STATE_PRESENT;
	std::wcout << "0 Otsib kaardilugejaid..." << std::endl;
	// 2. start the process:
	//    
	//    - list readers
	//    - for each reader, check if it has a card to connect to
	//    - if so, connect to card, read data
	//    - otherwise keep checking for readers
	while (true) {
		if (readerState == WAITING_FOR_READER) {

			sCardErrorCode = SCardGetStatusChange(sCardContext, INFINITE, &sCardReaderState, 1);
			SCARD_FATAL_ERROR(hWnd, &outputLog, sCardErrorCode);

			if (sCardReaderState.dwEventState & SCARD_STATE_PRESENT) {
				readerState = TRYING_FOR_CARD;
			}
			else if (sCardReaderState.dwEventState & SCARD_STATE_EMPTY) {
				std::wcout << L"1 Otsib kaarte..." << std::endl;
			}
			else if (sCardReaderState.dwEventState & SCARD_STATE_UNAVAILABLE) {
				std::wcout << L"0 Kaardilugeja ei ole saadaval" << std::endl;
			}
			else if (sCardReaderState.dwEventState & (SCARD_STATE_INUSE | SCARD_STATE_EXCLUSIVE)) {
				std::wcout << L"0 Kaardilugeja on kasutuses teise rakenduse poolt" << std::endl;
			}

			sCardReaderState.dwCurrentState = sCardReaderState.dwEventState;

			StringCbPrintf(szFormatBuffer, FORMAT_BUFFER_SIZE, L"Card reader state changed: eventState=%d currentState=%d\n", sCardReaderState.dwEventState, sCardReaderState.dwCurrentState);
			std::wcout << "0 " << szFormatBuffer;

			// 65538 seems to mean "card reader attached! now go figure out its name"
			if (sCardReaderState.dwCurrentState == 65538) {
				sCardErrorCode = SCardListReaders(sCardContext, NULL, sCardSzReaders, &sCardReaders);
				SCARD_FATAL_ERROR(hWnd, &outputLog, sCardErrorCode);
				sCardReaderState.szReader = sCardSzReaders;
			}
		}
		else if (readerState == TRYING_FOR_CARD) {
			
			std::wcout << L"1 Ühendan kaardiga..." << std::endl;

			sCardErrorCode = SCardConnect(sCardContext, sCardReaderState.szReader, SCARD_SHARE_SHARED,
				SCARD_PROTOCOL_T0 | SCARD_PROTOCOL_T1, &sCardHandle, &sCardActiveProtocol);

			StringCbPrintf(szFormatBuffer, FORMAT_BUFFER_SIZE, L"SCardConnect: %x", (unsigned long)sCardErrorCode);
			std::wcout << L"0 " << szFormatBuffer << std::endl;

			if (sCardErrorCode == SCARD_S_SUCCESS) {
				readerState = READING_CARD;
			}

			SCARD_FATAL_ERROR(hWnd, &outputLog, sCardErrorCode);
			sCardHandleReaderErrors(sCardErrorCode, &readerState);
			sCardHandleCardPull(sCardErrorCode, &readerState);

		}
		else if (readerState == READING_CARD) {

			std::wcout << L"1 Loen kaarti..." << std::endl;
			sCardErrorCode = openSCardPersonalFile(sCardHandle, sCardRecvBuffer, &sCardRecvBytes, sCardActiveProtocol);

			StringCbPrintf(szFormatBuffer, FORMAT_BUFFER_SIZE, L"openSCardPersonalFile: %x", (unsigned long)sCardErrorCode);
			std::wcout << "0 " << szFormatBuffer << std::endl;

			SCARD_FATAL_ERROR(hWnd, &outputLog, sCardErrorCode);
			if (sCardHandleReaderErrors(sCardErrorCode, &readerState) ||
				sCardHandleCardPull(sCardErrorCode, &readerState)) {
				continue;
			}


#pragma region Read ID code
			// 
			// Read ID number
			// 
			sCardErrorCode = readSCardPersonalFile(sCardHandle, EID_IDNUMBER, sCardRecvBuffer, &sCardRecvBytes, sCardActiveProtocol);

			StringCbPrintf(szFormatBuffer, FORMAT_BUFFER_SIZE, L"readSCardPersonalFile: %x", (unsigned long)sCardErrorCode);
			std::wcout << "0 " << szFormatBuffer << std::endl;

			SCARD_FATAL_ERROR(hWnd, &outputLog, sCardErrorCode);
			if (sCardHandleReaderErrors(sCardErrorCode, &readerState) ||
				sCardHandleCardPull(sCardErrorCode, &readerState)) {
				continue;
			}

			MultiByteToWideChar(CP_UTF8, 0, (LPCCH)sCardRecvBuffer, sCardRecvBytes, szFormatBuffer, EID_LEN_IDNUMBER);
			// TODO: handle conversion errors

			if (wmemcpy_s(person.idNumber, EID_LEN_IDNUMBER, szFormatBuffer, EID_LEN_IDNUMBER) != 0) {
				OutputDebugString(L"Failed to copy received ID number over to person object.\n");
				std::wcout << "3003 " << L"Viga andmete mälus liigutamisel" << std::endl;
			}

			if (sCardErrorCode == SCARD_W_REMOVED_CARD || sCardErrorCode == SCARD_E_NO_SMARTCARD) {
				OutputDebugString(L"Card was removed - waiting for new card...\n");
				std::wcout << "0 Kaart eemaldati - ootan järgmist..." << std::endl;
				readerState = TRYING_FOR_CARD;
				continue;
			}
#pragma endregion
#pragma region Read last name 

			//
			// Read last name
			// 

			sCardErrorCode = readSCardPersonalFile(sCardHandle, EID_LAST_NAME, sCardRecvBuffer, &sCardRecvBytes, sCardActiveProtocol);

			StringCbPrintf(szFormatBuffer, FORMAT_BUFFER_SIZE, L"readSCardPersonalFile: %x\n", (unsigned long)sCardErrorCode);
			OutputDebugString(szFormatBuffer);

			SCARD_FATAL_ERROR(hWnd, &outputLog, sCardErrorCode);
			if (sCardHandleReaderErrors(sCardErrorCode, &readerState) ||
				sCardHandleCardPull(sCardErrorCode, &readerState)) {
				continue;
			}

			MultiByteToWideChar(CP_UTF8, 0, (LPCCH)sCardRecvBuffer, sCardRecvBytes, szFormatBuffer, EID_LEN_LAST_NAME);
			// TODO: handle conversion errors

			for (int i = 0; i < EID_LEN_LAST_NAME;i++) {
				if (szFormatBuffer[i] == 65533) {
					szFormatBuffer[i] = 0;
					break;
				}
			}

			if (wmemcpy_s(person.lastName, EID_LEN_LAST_NAME, szFormatBuffer, EID_LEN_LAST_NAME) != 0) {
				OutputDebugString(L"Failed to copy received ID number over to person object.\n");
				// TODO: handle - retry?
			}

			if (sCardErrorCode == SCARD_W_REMOVED_CARD || sCardErrorCode == SCARD_E_NO_SMARTCARD) {
				OutputDebugString(L"Card was removed - waiting for new card...\n");
				readerState = TRYING_FOR_CARD;
				continue;
			}
#pragma endregion
#pragma region Read first name
			//
			// Read first name
			//

			sCardErrorCode = readSCardPersonalFile(sCardHandle, EID_NAME_1, sCardRecvBuffer, &sCardRecvBytes, sCardActiveProtocol);

			StringCbPrintf(szFormatBuffer, FORMAT_BUFFER_SIZE, L"readSCardPersonalFile: %x\n", (unsigned long)sCardErrorCode);
			OutputDebugString(szFormatBuffer);

			SCARD_FATAL_ERROR(hWnd, &outputLog, sCardErrorCode);
			if (sCardHandleReaderErrors(sCardErrorCode, &readerState) ||
				sCardHandleCardPull(sCardErrorCode, &readerState)) {
				continue;
			}

			MultiByteToWideChar(CP_UTF8, 0, (LPCCH)sCardRecvBuffer, sCardRecvBytes, szFormatBuffer, EID_LEN_NAME_1);
			// TODO: handle conversion errors

			for (int i = 0; i < EID_LEN_LAST_NAME;i++) {
				if (szFormatBuffer[i] == 65533) {
					szFormatBuffer[i] = 0;
					break;
				}
			}

			if (wmemcpy_s(person.firstName, EID_LEN_NAME_1, szFormatBuffer, EID_LEN_NAME_1) != 0) {
				OutputDebugString(L"Failed to copy received ID number over to person object.\n");
				// TODO: handle - retry?
			}

			if (sCardErrorCode == SCARD_W_REMOVED_CARD || sCardErrorCode == SCARD_E_NO_SMARTCARD) {
				OutputDebugString(L"Card was removed - waiting for new card...\n");
				readerState = TRYING_FOR_CARD;
				continue;
			}
#pragma endregion
			// if successfully read data, can move to DONE_READING
			if (sCardErrorCode == SCARD_S_SUCCESS) {
				readerState = WAITING_REMOVE;
			}
			else if (sCardErrorCode == SCARD_W_REMOVED_CARD) {
				readerState = TRYING_FOR_CARD;
				// TODO: wait until card
				// sCardErrorCode = SCardGetStatusChange(sCardContext, INFINITE, )
			}
		}
		else if (readerState == WAITING_REMOVE) {
			OutputDebugString(L"Done reading this card.\n");
			SCardDisconnect(sCardHandle, SCARD_UNPOWER_CARD);
			std::wcout << "Kaart on loetud." << std::endl;

			// TODO: handle errors.

			uint64_t idNumber = wcstoll(person.idNumber, NULL, 10);

			if (!hasIdCodeBeenScanned(idNumber)) {			

				// Keep track that it has now been used
				idCodeBuffer[lastIdCodeIndex] = idNumber;
				lastIdCodeIndex++;

				// double the buffer if it gets maxed
				if (lastIdCodeIndex == idCodeBufferSize - 1) {
					if (idCodeBufferSize * 2 > SIZE_MAX) {
						OutputDebugString(L"idCodeBufferSize would be bigger than SIZE_MAX, cannot keep doubling");
					}
					else {
						idCodeBuffer = (uint64_t *)realloc(idCodeBuffer, idCodeBufferSize * 2);
					}
				}

				currentTimet = time(NULL);
				localtime_s(&currentTime, &currentTimet);
				wcsftime(szFormatBuffer, FORMAT_BUFFER_SIZE, L"%Y-%m-%d %H:%M:%S %z", &currentTime);

				// Let the GUI know we've read an ID card
				std::wcout << "2 Time="     << szFormatBuffer
							<< " IDcode="    << person.idNumber 
							<< " FirstName=" << person.firstName 
							<< " LastName="  << person.lastName << std::endl;

				// Output person structure to file
				try {
					// "2016-05-12 14:45:50 +0400", 39610042010
					outputFile << L'"' << szFormatBuffer << L"\"," << person.idNumber << L"," << person.firstName << L"," << person.lastName << std::endl;
					outputFile.flush();

					if (outputFile.fail()) {
						std::wcerr << "3004 ID-kaardi lugeja teatab failiga seonduvast veast." << std::endl;
					}

					MessageBeep(0xFFFFFFFF);
				}
				catch (const std::exception &e) {
					OutputDebugString(L"IO error thrown:\n\t");
					const char* errorMessage = e.what();
					strerror_s(cFormatBuffer, FORMAT_BUFFER_SIZE);
					DWORD lasterror = GetLastError();

					std::wcerr << "3005 IO veateade: " << errorMessage
						<< " errno: " << errno
						<< " strerror_s tulemus: " << cFormatBuffer
						<< " GetLastError(): " << lasterror
						<< std::endl;
					outputLog << "3005 IO veateade: " << errorMessage
						<< " errno: " << errno
						<< " strerror_s tulemus: " << cFormatBuffer
						<< " GetLastError(): " << lasterror
						<< std::endl;
					goto teardown;
				}
			}
			else {
				OutputDebugString(L"Did not log card because have seen it before");
				std::wcout << "3 Kaart on juba loetud!" << std::endl;
			}

			// Wait for empty state
			sCardErrorCode = SCardGetStatusChange(sCardContext, INFINITE, &sCardReaderState, 1);
			SCARD_FATAL_ERROR(hWnd, &outputLog, sCardErrorCode);

			readerState = WAITING_FOR_READER;
		}
		
#pragma endregion
		if (shouldClose) {
			break;
		}
		Sleep(SLEEP_TIME);
	}
teardown:
	free(idCodeBuffer);
	outputFile.close();
	outputLog.close();
	std::wcout << L"1 Töö lõpetatud.";
	return;
}
DWORD openSCardPersonalFile(SCARDHANDLE sCardHandle, LPBYTE receiveBuffer, LPDWORD receivedBytes, DWORD sCardActiveProtocol) {
	DWORD sCardErrorCode;

	*receivedBytes = SCARD_RECVSIZE;
	sCardErrorCode = SCardTransmit(sCardHandle, sCardActiveProtocol == SCARD_PROTOCOL_T0 ? SCARD_PCI_T0 : SCARD_PCI_T1, t0CmdChooseRootFolder,
		sizeof(t0CmdChooseRootFolder), NULL, receiveBuffer, receivedBytes);

	if (sCardErrorCode != SCARD_S_SUCCESS) {
		return sCardErrorCode;
	}
	

	*receivedBytes = SCARD_RECVSIZE;
	sCardErrorCode = SCardTransmit(sCardHandle, sCardActiveProtocol == SCARD_PROTOCOL_T0 ? SCARD_PCI_T0 : SCARD_PCI_T1, t0CmdChooseFolderEEEE,
		sizeof(t0CmdChooseFolderEEEE), NULL, receiveBuffer, receivedBytes);

	if (sCardErrorCode != SCARD_S_SUCCESS) {
		return sCardErrorCode;
	}

	*receivedBytes = SCARD_RECVSIZE;
	sCardErrorCode = SCardTransmit(sCardHandle, sCardActiveProtocol == SCARD_PROTOCOL_T0 ? SCARD_PCI_T0 : SCARD_PCI_T1, t0CmdChooseFile5044,
		sizeof(t0CmdChooseFile5044), NULL, receiveBuffer, receivedBytes);

	return sCardErrorCode;
}
DWORD readSCardPersonalFile(SCARDHANDLE sCardHandle, BYTE recordNumber, LPBYTE receiveBuffer, LPDWORD receivedBytes, DWORD sCardActiveProtocol) {
	DWORD errorValue = 0;

	BYTE *readRecordCmd;
	BYTE *readBytesCmd;

	if (sCardActiveProtocol == SCARD_PROTOCOL_T0) {
		readRecordCmd = new byte[4] { 0 };
		readBytesCmd = new byte[5] { 0 };
	}
	else {
		readRecordCmd = new byte[5] { 0 };
		readBytesCmd = new byte[6] { 0 };
	}
	
	memcpy(readRecordCmd, t0CmdRequestRecord, 4);
	memcpy(readBytesCmd, t0CmdReadBytes, 5);

	readRecordCmd[2] = recordNumber;

	*receivedBytes = SCARD_RECVSIZE;
	errorValue = SCardTransmit(sCardHandle, 
		sCardActiveProtocol == SCARD_PROTOCOL_T0 ? SCARD_PCI_T0 : SCARD_PCI_T1, 
		readRecordCmd,
		sCardActiveProtocol == SCARD_PROTOCOL_T0 ? 4 : 5, 
		NULL, 
		receiveBuffer, 
		receivedBytes);

	if (errorValue != SCARD_S_SUCCESS) {
		return errorValue;
	}

	// T0 needs an extra step where we read the data previously requested
	if (sCardActiveProtocol == SCARD_PROTOCOL_T0) {
		readBytesCmd[4] = receiveBuffer[1];
		*receivedBytes = SCARD_RECVSIZE;

		errorValue = SCardTransmit(sCardHandle,
			sCardActiveProtocol == SCARD_PROTOCOL_T0 ? SCARD_PCI_T0 : SCARD_PCI_T1,
			readBytesCmd,
			sCardActiveProtocol == SCARD_PROTOCOL_T0 ? 5 : 6, NULL, receiveBuffer, receivedBytes);
	}
	return errorValue;
}
void showSCardErrorMessage(std::wofstream *outputLog, DWORD errorCode) {
	const wchar_t* sCardErrorMessage = L"Viga :(";

	switch (errorCode) {
		case ERROR_BROKEN_PIPE:
			sCardErrorMessage = L"See rakendus ei toeta kaugühendusi. (ERROR_BROKEN_PIPE)";
			break;

		case SCARD_E_CARD_UNSUPPORTED:
			sCardErrorMessage = L"Seda kaarti ei toetata. (SCARD_E_CARD_UNSUPPORTED)";
			break;
		case SCARD_E_NO_READERS_AVAILABLE:
			sCardErrorMessage = L"Ühtki ID-lugejat ei ole ühendatud. (SCARD_E_NO_READERS_AVAILABLE)";
			break;
		case SCARD_E_READER_UNAVAILABLE:
			sCardErrorMessage = L"Lugejat ei saa hetkel kasutada. (SCARD_E_READER_UNAVAILABLE)";
			break;
		case SCARD_E_BAD_SEEK: 
			sCardErrorMessage = L"Lugejat ei saanud õigesti valida. (SCARD_E_BAD_SEEK)";
		case SCARD_E_CANCELLED:
			sCardErrorMessage = L"Tegevus peatati. (SCARD_E_CANCELLED)";
			break;
		case SCARD_E_CANT_DISPOSE:
			sCardErrorMessage = L"Viga mingite andmete mälust eemaldamisel. (SCARD_E_CANT_DISPOSE)";
			break;
		case SCARD_E_NO_DIR:
		case SCARD_E_DIR_NOT_FOUND:
			sCardErrorMessage = L"Kaardil puudub isikufail. (SCARD_E_NO_DIR / SCARD_E_DIR_NOT_FOUND)";
			break;
		case SCARD_E_NO_FILE:
		case SCARD_E_FILE_NOT_FOUND:
			sCardErrorMessage = L"Kaardil puudub isikufail. (SCARD_E_NO_FILE / SCARD_E_FILE_NOT_FOUND)";
			break;
		case SCARD_E_INSUFFICIENT_BUFFER:
			sCardErrorMessage = L"Puhvri suurus ei ole piisav, et andmeid sisse lugeda. (SCARD_E_INSUFFICIENT_BUFFER)";
			break;
		case SCARD_E_INVALID_HANDLE:
			sCardErrorMessage = L"Kaardile ligipääsemiseks ei saadud korrektset viidet. (SCARD_E_INVALID_HANDLE)";
			break;
		case SCARD_E_NO_ACCESS:
			sCardErrorMessage = L"Kaart ei lase ligi pääseda isikufailile. (SCARD_E_NO_ACCESS)";
			break;
		case SCARD_E_NO_MEMORY:
			sCardErrorMessage = L"Puudub piisav vahemälu (RAM), et täita tööülesandeid. (SCARD_E_NO_MEMORY)";
			break;
		case SCARD_E_PROTO_MISMATCH:
			sCardErrorMessage = L"Lugeja või kaart keeldub ühendumast õige suhtluskeelega. (SCARD_E_PROTO_MISMATCH)";
			break;
		case SCARD_E_UNKNOWN_READER:
			sCardErrorMessage = L"Lugeja või kaart ühendati lahti. (SCARD_E_UNKNOWN_READER)";
			break;
		case SCARD_W_REMOVED_CARD:
			sCardErrorMessage = L"Praegu ei ole kaarti ühendatud. (SCARD_W_REMOVED_CARD)";
			break;
		case SCARD_E_NO_SMARTCARD:
			sCardErrorMessage = L"Praegu ei ole kaarti ühendatud. (SCARD_E_NO_SMARTCARD)";
			break;
		case SCARD_E_NOT_READY:
			sCardErrorMessage = L"ID-kaardi lugeja ei ole veel valmis lugema. (SCARD_E_NOT_READY)";
			break;
	}

	StringCbPrintf(szFormatBuffer, FORMAT_BUFFER_SIZE, L"Fatal error (%x)\n", errorCode);
	OutputDebugString(szFormatBuffer);

	*outputLog << L"Viga ID-kaarti lugedes: " << sCardErrorMessage << std::endl;
	*outputLog << L"Veakood: " << errorCode << std::endl;
}
bool sCardHandleReaderErrors(DWORD errorCode, enum readerState *rs) {
	if (errorCode == SCARD_E_READER_UNAVAILABLE || 
		errorCode == SCARD_W_REMOVED_CARD || 
		errorCode == SCARD_E_READER_UNSUPPORTED) {
		*rs = WAITING_FOR_READER;
		return true;
	}
	return false;
}
bool sCardHandleCardPull(DWORD errorCode, enum readerState *rs) {
	if (errorCode == SCARD_W_REMOVED_CARD || 
		errorCode == SCARD_E_NO_SMARTCARD) {
		*rs = WAITING_FOR_READER;
		return true;
	}
	return false;
}
bool hasIdCodeBeenScanned(uint64_t idCode) {
	for (int i = 0; i < lastIdCodeIndex; i++) {
		if (idCodeBuffer[i] == idCode) {
			return true;
		}
	}
	return false;
}