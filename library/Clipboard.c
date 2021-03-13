#include <string>

#include "Clipboard.h"
#include "Config.h"
#include "Keyboard.h"
#include "Graphics.h"
#include "TC1014MMU.h"

ClipboardState* InitializeInstance(ClipboardState*);

static ClipboardState* instance = InitializeInstance(new ClipboardState());

extern "C" {
  __declspec(dllexport) ClipboardState* __cdecl GetClipboardState() {
    return instance;
  }
}

ClipboardState* InitializeInstance(ClipboardState* p) {
  p->PasteWithNew = false;

  return p;
}

extern "C" {
  __declspec(dllexport) int __cdecl GetCurrentKeyMap() {
    return instance->CurrentKeyMap;
  }
}

extern "C" {
  __declspec(dllexport) bool __cdecl SetClipboard(const char* clipout) {
    const size_t len = strlen(clipout) + 1;
    HGLOBAL hMem = GlobalAlloc(GMEM_MOVEABLE, len);

    if (hMem == 0) {
      MessageBox(0, "Failed to access clipboard.", "Clipboard", 0);
      return false;
    }

    memcpy(GlobalLock(hMem), clipout, len);

    GlobalUnlock(hMem);
    OpenClipboard(0);
    EmptyClipboard();
    SetClipboardData(CF_TEXT, hMem);
    CloseClipboard();

    return TRUE;
  }
}
