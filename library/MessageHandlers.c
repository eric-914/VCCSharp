#include <windows.h>

#include "ConfigDialogCallbacks.h"
#include "AboutBoxCallback.h"
#include "Config.h"
#include "Graphics.h"
#include "Keyboard.h"
#include "Joystick.h"
#include "DirectDraw.h"
#include "Emu.h"

#include "ProcessCommandMessage.h"

#include "VccState.h"

#include "../resources/resource.h"

extern "C" {
  __declspec(dllexport) void __cdecl CreateMainMenu(HWND hWnd) {
    EmuState* emuState = GetEmuState();

    if (!emuState->FullScreen) {
      SetMenu(hWnd, LoadMenu(emuState->Resources, MAKEINTRESOURCE(IDR_MENU)));
    }
    else {
      SetMenu(hWnd, NULL);
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl ShowConfiguration() {
    EmuState* emuState = GetEmuState();

    // open config dialog if not already open
    // opens modeless so you can control the cassette
    // while emulator is still running (assumed)
    if (emuState->ConfigDialog == NULL)
    {
      emuState->ConfigDialog = CreateDialog(emuState->Resources, (LPCTSTR)IDD_TCONFIG, emuState->WindowHandle, (DLGPROC)CreateMainConfigDialogCallback);

      // open modeless
      ShowWindow(emuState->ConfigDialog, SW_SHOWNORMAL);
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl EmuReset(unsigned char state) {
    EmuState* emuState = GetEmuState();

    if (emuState->EmulationRunning) {
      emuState->ResetPending = state;
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl KeyUp(WPARAM wParam, LPARAM lParam) {
    // send emulator key up event to the emulator
    // TODO: Key up checks whether the emulation is running, this does not

    unsigned char OEMscan = (unsigned char)((lParam & 0x00FF0000) >> 16);

    vccKeyboardHandleKey((unsigned char)wParam, OEMscan, kEventKeyUp);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl MouseMove(LPARAM lParam) {
    RECT clientSize;

    EmuState* emuState = GetEmuState();

    if (emuState->EmulationRunning)
    {
      unsigned int x = LOWORD(lParam);
      unsigned int y = HIWORD(lParam);

      GetClientRect(emuState->WindowHandle, &clientSize);

      x /= ((clientSize.right - clientSize.left) >> 6);
      y /= (((clientSize.bottom - clientSize.top) - 20) >> 6);

      SetJoystick(x, y);
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl ToggleFullScreen() {
    VccState* vccState = GetVccState();
    EmuState* emuState = GetEmuState();

    if (vccState->RunState == EMU_RUNSTATE_RUNNING)
    {
      vccState->RunState = EMU_RUNSTATE_REQWAIT;
      emuState->FullScreen = !emuState->FullScreen;
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl KeyDown(WPARAM wParam, LPARAM lParam) {
    unsigned char OEMscan = (unsigned char)((lParam & 0x00FF0000) >> 16); // just get the scan code

    // send other keystrokes to the emulator if it is active
    if (GetEmuState()->EmulationRunning)
    {
      vccKeyboardHandleKey((unsigned char)wParam, OEMscan, kEventKeyDown);

      // Save key down in case focus is lost
      SaveLastTwoKeyDownEvents((unsigned char)wParam, OEMscan);
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl ToggleMonitorType() {
    SetMonitorType(!SetMonitorType(QUERY));
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl ToggleThrottle() {
    VccState* vccState = GetVccState();

    vccState->Throttle = !vccState->Throttle;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl ToggleInfoBand() {
    SetInfoBand(!SetInfoBand(QUERY));
    InvalidateBorder();
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SlowDown() {
    DecreaseOverclockSpeed(GetEmuState());
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SpeedUp() {
    IncreaseOverclockSpeed(GetEmuState());
  }
}
