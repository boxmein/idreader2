// idreader2.cpp : Defines the entry point for the application.
//

#include "stdafx.h"
#include "idreader2.h"


HINSTANCE hInst;
WCHAR szTitle[MAX_LOADSTRING];
WCHAR szWindowClass[MAX_LOADSTRING];

CHAR cFormatBuffer[FORMAT_BUFFER_SIZE];
WCHAR szFormatBuffer[FORMAT_BUFFER_SIZE];

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
enum readerState { INITIAL, WAITING_FOR_CARD, CARD_ATTACHED, DONE_READING };

/// This is the data we get to collect off the reader.
struct person {
	wchar_t idNumber[EID_LEN_IDNUMBER + 1];
};

int APIENTRY wWinMain(_In_ HINSTANCE hInstance,
                     _In_opt_ HINSTANCE hPrevInstance,
                     _In_ LPWSTR    lpCmdLine,
                     _In_ int       nCmdShow)
{
	MSG msg;
	HWND hWnd;

	LoadStringW(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
    LoadStringW(hInstance, IDC_IDREADER2, szWindowClass, MAX_LOADSTRING);

    registerWndCls(hInstance);

	hInst = hInstance;
	hWnd = CreateWindowW(szWindowClass, szTitle, WS_OVERLAPPEDWINDOW,
		CW_USEDEFAULT, 0, CW_USEDEFAULT, 0, nullptr, nullptr, hInstance, nullptr);

	if (!hWnd)
	{
		return EXIT_FAILURE;
	}

	ShowWindow(hWnd, nCmdShow);
	UpdateWindow(hWnd);

	std::thread sCardThread(sCardReaderThread, hWnd);
    while (GetMessage(&msg, nullptr, 0, 0))
    {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }

	sCardThread.join();

	OutputDebugString(L"UI thread finished.\n");

    return (int) msg.wParam;
}


void sCardReaderThread(HWND hWnd) {
	enum readerState readerState = INITIAL;
	struct person person;
	
	ZeroMemory(&person, sizeof(struct person));
	/// Any smart card related function will return an error code.
	LONG sCardErrorCode;
	/// To interact with the smart card, a context is required.
	SCARDCONTEXT sCardContext;
	/// Size of the LPTSTR that receives all reader names.
	/// Setting it to SCARD_AUTOALLOCATE will let the SCard library allocate the LPTSTR,
	/// and when it does, override the value.
	DWORD sCardReaders = SCARD_AUTOALLOCATE;
	/// Receives all available readers as names. ["Reader 1", "Reader 2", ""]
	/// ['R', 'e', ..., '1', '\0', 'R', 'e', ..., '2', '\0', '\0']
	LPTSTR sCardSzReaders;
	LPTSTR readerNames[MAX_READERS];

	/// Handle to a Smart Card that we've connected to.
	SCARDHANDLE sCardHandle;
	LPTSTR currentReaderName;

	/// Receives the protocol that we've used, will be one from the list that we accept.
	DWORD sCardActiveProtocol;
	/// Generic byte buffer for data that's received from the smart card.
	BYTE sCardRecvBuffer[SCARD_RECVSIZE];

	/// Use for passing the size of the receive buffer, and receiving the amount of data
	/// received.
	DWORD sCardRecvBytes = 0xFFFFFFFF;
	
	/// Time handling structs
	time_t currentTimet;
	tm currentTime;

	/// File handle for output
	std::wofstream outputFile;

	// TODO: file output for data/logs

	// 0. open file for output
	currentTimet = time(0);
	localtime_s(&currentTime, &currentTimet);
	wcsftime(szFormatBuffer, FORMAT_BUFFER_SIZE, L"andmed-%Y-%m-%d-%H-%M-%S.csv", &currentTime);
	outputFile.open(szFormatBuffer, std::ios_base::app);
	// enable throwing on the failbit
	outputFile.exceptions(std::ios::badbit | std::ios::failbit);

	if (!outputFile.is_open()) {
		OutputDebugString(L"data file did not open. :/\n");
		goto teardown;
	}

	// 1. establish a security context
	sCardErrorCode = SCardEstablishContext(SCARD_SCOPE_SYSTEM, NULL, NULL, &sCardContext);
	SCARD_FATAL_ERROR(hWnd, sCardErrorCode);
	OutputDebugString(L"established SCard memory context!\n");

	while (true) {
#pragma region List all card readers attached to the computer
		// wait until there's a reader connected
		if (readerState == INITIAL) {
			
			OutputDebugString(L"Finding card readers...\n");
			setStatusString(hWnd, L"Finding card readers...");

			sCardErrorCode = SCardListReaders(sCardContext, NULL, (LPTSTR)&sCardSzReaders, &sCardReaders);

			if (sCardErrorCode == SCARD_S_SUCCESS) {
				OutputDebugString(L"Found card readers! Getting reference to each of their names...\n");
				currentReaderName = sCardSzReaders;
				
				ZeroMemory(readerNames, sizeof(LPTSTR) * MAX_READERS);
				readerNames[0] = sCardSzReaders;
				
				for (int i = 1, j = 1; i < MAX_READERS; i++) {
					currentReaderName += wcslen(sCardSzReaders) + 1;
					if (*sCardSzReaders == '\0') {
						break;
					}
					
					readerNames[j] = currentReaderName;
					j += 1;
				}
				OutputDebugString(L"Now waiting for cards to be attached...\n");
				readerState = WAITING_FOR_CARD;
			}
			else if (sCardErrorCode == SCARD_E_NO_READERS_AVAILABLE || 
					 sCardErrorCode == SCARD_E_READER_UNAVAILABLE) {
				OutputDebugString(L"No readers available. Finding readers...\n");
				continue;
			}
			
			SCARD_FATAL_ERROR(hWnd, sCardErrorCode);
		}
#pragma endregion
#pragma region Find an ID card to connect to in all of the readers
		// if there's a reader connected, find cards in all readers
		else if (readerState == WAITING_FOR_CARD) {
			OutputDebugString(L"Waiting for card to be connected...\n");
			setStatusString(hWnd, L"Waiting for card...");
			int i = 0;
			for (int i = 0; i < MAX_READERS; i++) {
				if (readerNames[i] == 0) {
					continue;
				}

				currentReaderName = readerNames[i];

				sCardErrorCode = SCardConnect(sCardContext, currentReaderName, SCARD_SHARE_SHARED,
					SCARD_PROTOCOL_T0, &sCardHandle, &sCardActiveProtocol);
				
				StringCbPrintf(szFormatBuffer, FORMAT_BUFFER_SIZE, L"Checking reader '%hs' for card...", currentReaderName);
				OutputDebugString(szFormatBuffer);

				if (sCardErrorCode == SCARD_S_SUCCESS) {
					OutputDebugString(L"Successfully connected to card! Reading...\n");
					readerState = CARD_ATTACHED;
					break;
				}

				else if (sCardErrorCode == SCARD_E_PROTO_MISMATCH ||
						 sCardActiveProtocol != SCARD_PROTOCOL_T0) {
					OutputDebugString(L"Card didn't want to talk to us over T0. Retrying to connect with T0...\n");
					sCardErrorCode = SCardReconnect(sCardHandle, SCARD_SHARE_SHARED, SCARD_PROTOCOL_T0, SCARD_UNPOWER_CARD, &sCardActiveProtocol);
				}
				else if (sCardErrorCode != SCARD_W_REMOVED_CARD && 
					sCardErrorCode != SCARD_E_NO_SMARTCARD &&
					sCardErrorCode != SCARD_E_UNKNOWN_READER) {
					OutputDebugString(L"Error in WAITING_FOR_CARD\n");
					SCARD_FATAL_ERROR(hWnd, sCardErrorCode);
				}
			}
		}
#pragma endregion
#pragma region Read data off ID card
		// if there's a card attached, read data off of it
		else if (readerState == CARD_ATTACHED) {

			OutputDebugString(L"Reading data from card...\n");
			setStatusString(hWnd, L"Reading data from card...");

			// 0. open the smart card personal file
			sCardErrorCode = openSCardPersonalFile(sCardHandle, sCardRecvBuffer, &sCardRecvBytes);
			
			if (sCardErrorCode == SCARD_W_REMOVED_CARD || sCardErrorCode == SCARD_E_NO_SMARTCARD) {
				OutputDebugString(L"Card was removed - waiting for new card...\n");
				readerState = WAITING_FOR_CARD;
				continue;
			}

			SCARD_FATAL_ERROR(hWnd, sCardErrorCode);

			// 1. Read ID number, convert it to multibyte string and copy it to the person struct
			sCardErrorCode = readSCardPersonalFile(sCardHandle, EID_IDNUMBER, sCardRecvBuffer, &sCardRecvBytes);
			MultiByteToWideChar(CP_UTF8, 0, (LPCCH)sCardRecvBuffer, sCardRecvBytes, szFormatBuffer, EID_LEN_IDNUMBER);

			if (wmemcpy_s(person.idNumber, EID_LEN_IDNUMBER, szFormatBuffer, EID_LEN_IDNUMBER) != 0) {
				OutputDebugString(L"Failed to copy received ID number over to person object.\n");
			}

			if (sCardErrorCode == SCARD_W_REMOVED_CARD || sCardErrorCode == SCARD_E_NO_SMARTCARD) {
				OutputDebugString(L"Card was removed - waiting for new card...\n");
				readerState = WAITING_FOR_CARD;
				continue;
			}

			// if successfully read data, can move to DONE_READING
			if (sCardErrorCode == SCARD_S_SUCCESS) {
				OutputDebugString(L"Successfully read personal data!\n");
				setStatusString(hWnd, szFormatBuffer);
				readerState = DONE_READING;
			}

			// if card gets detached, go back to waiting for card
			else if (sCardErrorCode == SCARD_W_REMOVED_CARD || sCardErrorCode == SCARD_E_NO_SMARTCARD) {
				OutputDebugString(L"Card was removed - waiting for new card...\n");
				readerState = WAITING_FOR_CARD;
				continue;
			}

			SCARD_FATAL_ERROR(hWnd, sCardErrorCode); 
		}
		else if (readerState == DONE_READING) {
			OutputDebugString(L"Done reading this card.\n");
			SCardDisconnect(sCardHandle, SCARD_UNPOWER_CARD);
			setStatusString(hWnd, L"Done reading card.");

			// TODO: handle errors.
			currentTimet = time(0);
			localtime_s(&currentTime, &currentTimet);
			wcsftime(szFormatBuffer, FORMAT_BUFFER_SIZE, L"%Y-%m-%d %H:%M:%S %z", &currentTime);

			OutputDebugString(L"Time is:\n\t");
			OutputDebugString(szFormatBuffer);

			OutputDebugString(L"\nID code is:\n\t");
			OutputDebugString(person.idNumber);
			OutputDebugString(L"\n");

			// Output person structure to file
			try {
				outputFile << szFormatBuffer << L"," << person.idNumber << std::endl;
				outputFile.flush();

				if (outputFile.fail()) {
					OutputDebugString(L"Some I/O error on output file happened\n");
				}

				readerState = WAITING_FOR_CARD;
			}
			catch (const std::exception &e) {
				OutputDebugString(L"IO error thrown:\n\t");
				const char* errorMessage = e.what();

				MultiByteToWideChar(CP_UTF8, 0, errorMessage, strlen(errorMessage), szFormatBuffer, FORMAT_BUFFER_SIZE);
				OutputDebugString(szFormatBuffer);
				
				StringCbPrintf(szFormatBuffer, FORMAT_BUFFER_SIZE, L"\nerrno: %d\n\t", errno);
				OutputDebugString(szFormatBuffer);

				strerror_s(cFormatBuffer, FORMAT_BUFFER_SIZE);
				MultiByteToWideChar(CP_UTF8, 0, cFormatBuffer, strlen(errorMessage), szFormatBuffer, FORMAT_BUFFER_SIZE);
				
				OutputDebugString(szFormatBuffer);
				
				DWORD errCode = GetLastError();
				StringCbPrintf(szFormatBuffer, FORMAT_BUFFER_SIZE, L"\nGetLastError(): %d\n\t", errCode);
				OutputDebugString(szFormatBuffer);

				goto teardown;
			}
		}
#pragma endregion
		if (shouldClose) {
			break;
		}
		Sleep(SLEEP_TIME);
	}
teardown:
	outputFile.close();
	OutputDebugString(L"ID card reading thread finished.");
	return;
}

BOOL setStatusString(HWND hWnd, LPTSTR text) {
	statusString = text;
	UpdateWindow(hWnd);
	return true;
}
DWORD openSCardPersonalFile(SCARDHANDLE sCardHandle, LPBYTE receiveBuffer, LPDWORD receivedBytes) {
	DWORD sCardErrorCode;

	*receivedBytes = SCARD_RECVSIZE;
	sCardErrorCode = SCardTransmit(sCardHandle, SCARD_PCI_T0, t0CmdChooseRootFolder,
		sizeof(t0CmdChooseRootFolder), NULL, receiveBuffer, receivedBytes);

	if (sCardErrorCode != SCARD_S_SUCCESS) {
		return sCardErrorCode;
	}

	*receivedBytes = SCARD_RECVSIZE;
	sCardErrorCode = SCardTransmit(sCardHandle, SCARD_PCI_T0, t0CmdChooseFolderEEEE,
		sizeof(t0CmdChooseFolderEEEE), NULL, receiveBuffer, receivedBytes);

	if (sCardErrorCode != SCARD_S_SUCCESS) {
		return sCardErrorCode;
	}

	*receivedBytes = SCARD_RECVSIZE;
	sCardErrorCode = SCardTransmit(sCardHandle, SCARD_PCI_T0, t0CmdChooseFile5044,
		sizeof(t0CmdChooseFile5044), NULL, receiveBuffer, receivedBytes);

	return sCardErrorCode;
}
DWORD readSCardPersonalFile(SCARDHANDLE sCardHandle, BYTE recordNumber, LPBYTE receiveBuffer, LPDWORD receivedBytes) {
	DWORD errorValue = 0;
	
	BYTE readRecordCmd[4] = { 0 };
	BYTE readBytesCmd[5] = { 0 };

	memcpy(readRecordCmd, t0CmdRequestRecord, 4);
	memcpy(readBytesCmd, t0CmdReadBytes, 5);

	readRecordCmd[2] = recordNumber;

	*receivedBytes = SCARD_RECVSIZE;
	errorValue = SCardTransmit(sCardHandle, SCARD_PCI_T0, readRecordCmd,
		sizeof(readRecordCmd), NULL, receiveBuffer, receivedBytes);

	if (errorValue != SCARD_S_SUCCESS) {
		return errorValue;
	}

	_ASSERT(*receivedBytes == 2);
	
	readBytesCmd[4] = receiveBuffer[1];
	*receivedBytes = SCARD_RECVSIZE;

	errorValue = SCardTransmit(sCardHandle, SCARD_PCI_T0, readBytesCmd,
		sizeof(readBytesCmd), NULL, receiveBuffer, receivedBytes);

	return errorValue;
}
void showSCardErrorMessage(HWND hWnd, DWORD errorCode) {
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
			break;
		case SCARD_E_NO_ACCESS:
			break;
		case SCARD_E_NO_MEMORY:
			break;
		case SCARD_E_PROTO_MISMATCH:
			break;
		case SCARD_E_UNKNOWN_READER:
			sCardErrorMessage = L"Lugeja või kaart ühendati lahti. (SCARD_E_UNKNOWN_READER)";
		case SCARD_W_REMOVED_CARD:
			sCardErrorMessage = L"Praegu ei ole kaarti ühendatud. (SCARD_W_REMOVED_CARD)";
		case SCARD_E_NO_SMARTCARD:
			sCardErrorMessage = L"Praegu ei ole kaarti ühendatud. (SCARD_E_NO_SMARTCARD)";
			break;
		case SCARD_E_NOT_READY:
			sCardErrorMessage = L"ID-kaardi lugeja ei ole veel valmis lugema. (SCARD_E_NOT_READY)";
			break;
	}
	MessageBox(hWnd, sCardErrorMessage, L"Raske viga alustamisel", MB_OK | MB_ICONERROR);

	StringCbPrintf(szFormatBuffer, FORMAT_BUFFER_SIZE, L"Fatal error (%x)\n", errorCode);
	OutputDebugString(szFormatBuffer);
}
ATOM registerWndCls(HINSTANCE hInstance)
{
    WNDCLASSEXW wcex;

    wcex.cbSize = sizeof(WNDCLASSEX);

    wcex.style          = CS_HREDRAW | CS_VREDRAW;
    wcex.lpfnWndProc    = WndProc;
    wcex.cbClsExtra     = 0;
    wcex.cbWndExtra     = 0;
    wcex.hInstance      = hInstance;
    wcex.hIcon          = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_IDREADER2));
    wcex.hCursor        = LoadCursor(nullptr, IDC_ARROW);
    wcex.hbrBackground  = (HBRUSH)(COLOR_WINDOW+1);
    wcex.lpszMenuName   = MAKEINTRESOURCEW(IDC_IDREADER2);
    wcex.lpszClassName  = szWindowClass;
    wcex.hIconSm        = LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

    return RegisterClassExW(&wcex);
}
LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    switch (message)
    {
	
    case WM_COMMAND:
        {
            int wmId = LOWORD(wParam);
            // Parse the menu selections:
            switch (wmId)
            {
            case IDM_ABOUT:
                DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, showAboutDialog);
                break;
            case IDM_EXIT:
                DestroyWindow(hWnd);
                break;
            default:
                return DefWindowProc(hWnd, message, wParam, lParam);
            }
        }
        break;
    case WM_PAINT:
        {
			RECT rect;
            PAINTSTRUCT ps;
            HDC hdc = BeginPaint(hWnd, &ps);
			GetClientRect(hWnd, &rect);
			DrawText(hdc, statusString, -1, &rect, DT_CENTER | DT_VCENTER);
            EndPaint(hWnd, &ps);
        }
        break;
    case WM_DESTROY:
        PostQuitMessage(0);
        break;
    default:
        return DefWindowProc(hWnd, message, wParam, lParam);
    }
    return 0;
}
INT_PTR CALLBACK showAboutDialog(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
    UNREFERENCED_PARAMETER(lParam);
    switch (message)
    {
    case WM_INITDIALOG:
        return (INT_PTR)TRUE;

    case WM_COMMAND:
        if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
        {
            EndDialog(hDlg, LOWORD(wParam));
            return (INT_PTR)TRUE;
        }
        break;
    }
    return (INT_PTR)FALSE;
}
