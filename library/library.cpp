#define DIRECTINPUT_VERSION 0x0800
#include <dinput.h>

/***********************************************************************************************/

extern "C" { __declspec(dllexport) DIDATAFORMAT GetDataFormat() { return c_dfDIJoystick2; } }

/***********************************************************************************************/

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
