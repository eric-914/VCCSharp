#include <windows.h>

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
