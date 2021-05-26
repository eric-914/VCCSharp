#include <windows.h>
#include <stdio.h>

extern "C" {
  __declspec(dllexport) BOOL __cdecl FilePathRemoveFileSpec(char* path) {
    size_t index = strlen(path), length = index;

    if ((index == 0) || (index > MAX_PATH)) {
      return(false);
    }

    while ((index > 0) && (path[index] != '\\')) {
      index--;
    }

    while ((index > 0) && (path[index] == '\\')) {
      index--;
    }

    if (index == 0) {
      return(false);
    }

    path[index + 2] = 0;

    return(!(strlen(path) == length));
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl FilePathStripPath(char* path) {
    char buffer[MAX_PATH] = "";
    short index = (short)strlen(path);

    if ((index > MAX_PATH) || (index == 0)) { //Test for overflow
      return;
    }

    for (; index >= 0; index--) {
      if (path[index] == '\\') {
        break;
      }
    }

    if (index < 0) {	//delimiter not found
      return;
    }

    strcpy(buffer, &path[index + 1]);
    strcpy(path, buffer);
  }
}

extern "C" {
  __declspec(dllexport) HANDLE __cdecl FileCreateFile(char* filename, long desiredAccess) {
    return CreateFile(filename, desiredAccess, 0, 0, OPEN_ALWAYS, FILE_ATTRIBUTE_NORMAL, 0);
  }
}

extern "C" {
  __declspec(dllexport) DWORD __cdecl FileSetFilePointer(HANDLE handle, DWORD moveMethod) {
    return SetFilePointer(handle, 0, 0, moveMethod);
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
    return FileFlushFileBuffers(handle);
  }
}
