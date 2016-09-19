#pragma once

#include "resource.h"

#define MAX_LOADSTRING 100
#define SCARD_RECVSIZE 180

#define FORMAT_BUFFER_SIZE 255
#define LINE_BUFFER_SIZE 192

#define SCARD_FATAL_ERROR(hWnd, code) if (code != SCARD_S_SUCCESS) { showSCardErrorMessage(hWnd, code); goto teardown; }

#define SLEEP_TIME 1500
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

// Forward declarations
ATOM                registerWndCls(HINSTANCE hInstance);
LRESULT CALLBACK    WndProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK    showAboutDialog(HWND, UINT, WPARAM, LPARAM);
DWORD				readSCardPersonalFile(SCARDHANDLE sCardHandle, BYTE recordNumber, LPBYTE receiveBuffer, LPDWORD receivedBytes);
DWORD				openSCardPersonalFile(SCARDHANDLE sCardHandle, LPBYTE receiveBuffer, LPDWORD receivedBytes);
void				showSCardErrorMessage(HWND hWnd, DWORD errorCode);
void				sCardReaderThread(HWND hWnd);
BOOL				setStatusString(HWND hWnd, LPTSTR text);