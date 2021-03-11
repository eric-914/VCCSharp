#include "MessageHandlers.h"

extern "C" {
  __declspec(dllexport) void __cdecl ProcessKeyDownMessage(WPARAM wParam, LPARAM lParam) {
    KeyDown(wParam, lParam);
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
