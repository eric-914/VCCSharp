/*
Copyright 2015 by Joseph Forgione
This file is part of VCC (Virtual Color Computer).

    VCC (Virtual Color Computer) is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    VCC (Virtual Color Computer) is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with VCC (Virtual Color Computer).  If not, see <http://www.gnu.org/licenses/>.
*/
#include <windows.h>
#include <stdio.h>

static HINSTANCE g_hinstDLL = NULL;

BOOL WINAPI DllMain(
  HINSTANCE hinstDLL,  // handle to DLL module
  DWORD fdwReason,     // reason for calling function
  LPVOID lpReserved)   // reserved
{
  switch (fdwReason)
  {
  case DLL_PROCESS_ATTACH:
  case DLL_THREAD_ATTACH:
  case DLL_THREAD_DETACH:
    g_hinstDLL = hinstDLL;
    break;

  case DLL_PROCESS_DETACH:
    break;
  }

  return TRUE;
}

extern "C" {
  __declspec(dllexport) void* GetFunction(HMODULE hModule, LPCSTR  lpProcName) {
    return GetProcAddress(hModule, lpProcName);
  }
}

extern "C" {
  __declspec(dllexport) HINSTANCE __cdecl PAKLoadLibrary(char* modulePath) {
    return LoadLibrary(modulePath);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl PAKFreeLibrary(HINSTANCE hInstLib) {
    FreeLibrary(hInstLib);
  }
}

extern "C" {
  __declspec(dllexport) HANDLE __cdecl FileOpenFile(char* filename, long desiredAccess) {
    return CreateFile(filename, desiredAccess, 0, 0, OPEN_ALWAYS, FILE_ATTRIBUTE_NORMAL, 0);
  }
}

extern "C" {
  __declspec(dllexport) HANDLE __cdecl FileCreateFile(char* filename, long desiredAccess) {
    return CreateFile(filename, desiredAccess, 0, 0, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, 0);
  }
}

extern "C" {
  __declspec(dllexport) DWORD __cdecl FileSetFilePointer(HANDLE handle, DWORD moveMethod, long offset) {
    return SetFilePointer(handle, offset, 0, moveMethod);
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl FileReadFile(HANDLE handle, unsigned char* buffer, unsigned long size, unsigned long* moved) {
    return ReadFile(handle, buffer, size, moved, NULL);	//Read the whole file in for .CAS files
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl FileCloseHandle(HANDLE handle) {
    return CloseHandle(handle);
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl FileFlushFileBuffers(HANDLE handle) {
    return FlushFileBuffers(handle);
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl FileWriteFile(HANDLE handle, unsigned char* buffer, int size) {
    unsigned long bytesMoved = 0;

    return WriteFile(handle, buffer, 4, &bytesMoved, NULL);
  }
}

//--Stuff from wingdi.h

extern "C" {
  __declspec(dllexport) void __cdecl GDIWriteTextOut(HDC hdc, unsigned short x, unsigned short y, const char* message)
  {
    TextOut(hdc, x, y, message, (int)strlen(message));
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl GDISetBkColor(HDC hdc, COLORREF color)
  {
    SetBkColor(hdc, color);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl GDISetTextColor(HDC hdc, COLORREF color)
  {
    SetTextColor(hdc, color);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl GDITextOut(HDC hdc, int x, int y, char* text, int textLength)
  {
    TextOut(hdc, x, y, text, textLength);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl GDIGetClientRect(HWND hwnd, RECT* clientSize) {
    GetClientRect(hwnd, clientSize);
  }
}
