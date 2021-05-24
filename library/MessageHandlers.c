#include <windows.h>

#include "Keyboard.h"
#include "Joystick.h"
#include "Emu.h"

#include "ProcessCommandMessage.h"

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
