#pragma once

#include "resource.h"

#define MAX_LOADSTRING 100
#define SCARD_RECVSIZE 180

#define READER_NAME_BUFFER_SIZE 255
#define FORMAT_BUFFER_SIZE 255

#define SCARD_NONFATAL_ERROR   SCARD_W_REMOVED_CARD | SCARD_W_UNSUPPORTED_CARD | SCARD_E_NO_READERS_AVAILABLE | SCARD_E_PROTO_MISMATCH | SCARD_E_NO_SMARTCARD
#define SCARD_FATAL_ERROR(hWnd, outputFile, code) \
	if (code != SCARD_S_SUCCESS && !(code & (SCARD_NONFATAL_ERROR))) { \
		std::wcerr << code << " FATAL: Error reading ID card" << std::endl; \
		goto teardown; \
	}

#define SCARD_LOG_ERROR(code, message) \
	if (code != SCARD_S_SUCCESS && !(code & (SCARD_NONFATAL_ERROR)) { \
		std::wcerr << code << " " << message << std::endl; \
	}
#define SCARD_LOG_ERROR_CODE(receivedCode, errorCode, message) \
	if (receivedCode == errorCode) { \
		std::wcerr << errorCode << " " << message << std::endl; \
	}


#define SLEEP_TIME 100
#define MAX_READERS 16

// Estonian ID card record numbers
#define EID_LAST_NAME 0x01
#define EID_NAME_1 0x02
#define EID_NAME_2 0x03
#define EID_GENDER 0x04
#define EID_CITIZENSHIP 0x05
#define EID_BIRTH_DATE 0x06 // dd.mm.yyyy
#define EID_IDNUMBER 0x07
#define EID_DOCNUMBER 0x08
#define EID_VALID_UNTIL 0x09 // dd.mm.yyyy
#define EID_BIRTH_PLACE 0x0A
#define EID_ISSUED 0x0B // dd.mm.yyyy
#define EID_TYPE_OF_PERMIT 0x0C
#define EID_NOTES_1 0x0D
#define EID_NOTES_2 0x0E
#define EID_NOTES_3 0x0F
#define EID_NOTES_4 0x10

// Estonian ID card record lengths
#define EID_LEN_LAST_NAME 28
#define EID_LEN_NAME_1 15
#define EID_LEN_NAME_2 15
#define EID_LEN_GENDER 1
#define EID_LEN_IDNUMBER 11

// Forward declarations
DWORD				readSCardPersonalFile(SCARDHANDLE sCardHandle, BYTE recordNumber, LPBYTE receiveBuffer, LPDWORD receivedBytes, DWORD sCardActiveProtocol);
DWORD				openSCardPersonalFile(SCARDHANDLE sCardHandle, LPBYTE receiveBuffer, LPDWORD receivedBytes, DWORD sCardActiveProtocol);
void				showSCardErrorMessage(std::wofstream* outputLog, DWORD errorCode);
void				sCardReaderThread();
bool				sCardHandleReaderErrors(DWORD sCardErrorCode, enum readerState *readerState);
bool				sCardHandleCardPull(DWORD sCardErrorCode, enum readerState *readerState);
bool                hasIdCodeBeenScanned(uint64_t idCode);