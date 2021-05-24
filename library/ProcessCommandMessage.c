#include <windows.h>

#include "../resources/resource.h"

#include "Config.h"
#include "Clipboard.h"
#include "Graphics.h"
#include "Emu.h"
#include "Keyboard.h"

#include "MenuCallbacks.h"

//--------------------------------------------------------------------------
// When the main window is about to lose keyboard focus there are one
// or two keys down in the emulation that must be raised.  These routines
// track the last two key down events so they can be raised when needed.
//--------------------------------------------------------------------------
static unsigned char SC_save1 = 0;
static unsigned char SC_save2 = 0;
static unsigned char KB_save1 = 0;
static unsigned char KB_save2 = 0;

static int KeySaveToggle = 0;

// Save last two key down events
extern "C" {
  __declspec(dllexport) void __cdecl SaveLastTwoKeyDownEvents(unsigned char kb_char, unsigned char oemScan) {
    // Ignore zero scan code
    if (oemScan == 0) {
      return;
    }

    // Remember it
    KeySaveToggle = !KeySaveToggle;

    if (KeySaveToggle) {
      KB_save1 = kb_char;
      SC_save1 = oemScan;
    }
    else {
      KB_save2 = kb_char;
      SC_save2 = oemScan;
    }
  }
}

// Force keys up if main widow keyboard focus is lost.  Otherwise
// down keys will cause issues with OS-9 on return
// Send key up events to keyboard handler for saved keys
extern "C" {
  __declspec(dllexport) void __cdecl SendSavedKeyEvents() {
    if (SC_save1) {
      vccKeyboardHandleKey(KB_save1, SC_save1, kEventKeyUp);
    }

    if (SC_save2) {
      vccKeyboardHandleKey(KB_save2, SC_save2, kEventKeyUp);
    }

    SC_save1 = 0;
    SC_save2 = 0;
  }
}
