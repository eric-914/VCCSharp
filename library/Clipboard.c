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
