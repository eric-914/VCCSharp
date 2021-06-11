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
  __declspec(dllexport) BOOL __cdecl FileWriteFile(HANDLE handle, unsigned char* buffer, int size) {
    unsigned long bytesMoved = 0;

    return WriteFile(handle, buffer, 4, &bytesMoved, NULL);
  }
}
