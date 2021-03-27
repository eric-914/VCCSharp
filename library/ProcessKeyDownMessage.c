#include "di.version.h"
#include <dinput.h>

#include "MessageHandlers.h"

#include "VccState.h"
#include "Emu.h"

//--TODO: Adding this back for now.
void ToggleFullScreenState() {
  VccState* vccState = GetVccState();
  EmuState* emuState = GetEmuState();

  if (vccState->RunState == 0)
  {
    vccState->RunState = 1;
    emuState->FullScreen = !emuState->FullScreen;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl ProcessKeyDownMessage(WPARAM wParam, LPARAM lParam) {
    // get key scan code for emulator control keys
    unsigned char OEMscan = (unsigned char)((lParam & 0x00FF0000) >> 16); // just get the scan code

    switch (OEMscan)
    {
    case DIK_F11:
      ToggleFullScreenState();
      break;

    default:
      KeyDown(wParam, lParam);
      break;
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl ProcessSysKeyDownMessage(WPARAM wParam, LPARAM lParam) {
    // Ignore repeated system keys
    if (!(lParam >> 30)) {
      KeyDown(wParam, lParam);
    }
  }
}
