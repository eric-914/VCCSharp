#include "defines.h"
#include "../resources/resource.h"

#include "EmuState.h"

extern "C" {
  __declspec(dllexport) void __cdecl ResourceAppTitle(HINSTANCE hResources, char* buffer) {
    LoadString(hResources, IDS_APP_TITLE, buffer, MAX_LOADSTRING);
  }
}