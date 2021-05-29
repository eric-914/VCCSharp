#include <windows.h>
#include <stdio.h>

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
