#include <dinput.h>

#include "MessageHandlers.h"

#include "VccState.h"
#include "Emu.h"

extern "C" {
  __declspec(dllexport) void __cdecl ProcessKeyDownMessage(WPARAM wParam, LPARAM lParam) {
    // get key scan code for emulator control keys
    unsigned char OEMscan = (unsigned char)((lParam & 0x00FF0000) >> 16); // just get the scan code

    VccState* vccState = GetVccState();
    EmuState* emuState = GetEmuState();

    switch (OEMscan)
    {
      //case DIK_F3:    SlowDown();           break;
      //case DIK_F4:    SpeedUp();            break;
      //case DIK_F5:    EmuReset(1);          break;
      //case DIK_F6:    ToggleMonitorType();  break;
      //case DIK_F8:    ToggleThrottle();     break;
      //case DIK_F9:    ToggleOnOff();        break;
      //case DIK_F10:   ToggleInfoBand();     break;
    case DIK_F11:
      if (vccState->RunState == 0)
      {
        vccState->RunState = 1;
        emuState->FullScreen = !emuState->FullScreen;
      }

      break;

    default:
      KeyDown(wParam, lParam);
      break;
    }

    //KeyDown(wParam, lParam);
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
