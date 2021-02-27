#include <windows.h>
#include <process.h>

#include "VCC.h"

extern "C" {
  __declspec(dllexport) INT __cdecl AppRun(HINSTANCE hInstance, char* lpCmdLine) {
    VccStartup(hInstance, lpCmdLine);

    VccRun();

    return VccShutdown();
  }
}