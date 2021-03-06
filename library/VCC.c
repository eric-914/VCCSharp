#include <stdio.h>
#include <process.h>

#include "VCC.h"

#include "Config.h"
#include "Coco.h"
#include "PAKInterface.h"
#include "Keyboard.h"
#include "Graphics.h"
#include "Audio.h"
#include "MC6821.h"
#include "MC6809.h"
#include "HD6309.h"
#include "TC1014Registers.h"
#include "TC1014MMU.h"
#include "QuickLoad.h"
#include "DirectDraw.h"
#include "Throttle.h"

#include "cpudef.h"
#include "fileoperations.h"
#include "ProcessMessage.h"
#include "Emu.h"
#include "MenuCallbacks.h"

#include "resource.h"

VccState* InitializeInstance(VccState*);

static VccState* instance = InitializeInstance(new VccState());

extern "C" {
  __declspec(dllexport) VccState* __cdecl GetVccState() {
    return instance;
  }
}

VccState* InitializeInstance(VccState* p) {
  p->DialogOpen = false;

  p->AutoStart = true;
  p->KB_save1 = 0;
  p->KB_save2 = 0;
  p->KeySaveToggle = 0;
  p->SC_save1 = 0;
  p->SC_save2 = 0;
  p->Throttle = 0;

  p->RunState = EMU_RUNSTATE_RUNNING;

  strcpy(p->CpuName, "CPUNAME");
  strcpy(p->AppName, "");

  return p;
}

// Save last two key down events
extern "C" {
  __declspec(dllexport) void __cdecl SaveLastTwoKeyDownEvents(unsigned char kb_char, unsigned char oemScan) {
    // Ignore zero scan code
    if (oemScan == 0) {
      return;
    }

    // Remember it
    instance->KeySaveToggle = !instance->KeySaveToggle;

    if (instance->KeySaveToggle) {
      instance->KB_save1 = kb_char;
      instance->SC_save1 = oemScan;
    }
    else {
      instance->KB_save2 = kb_char;
      instance->SC_save2 = oemScan;
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetAutoStart(unsigned char autostart)
  {
    instance->AutoStart = autostart;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetCpuType(unsigned char cpuType)
  {
    static EmuState* _emu = GetEmuState();

    switch (cpuType)
    {
    case 0:
      _emu->CpuType = 0;

      strcpy(instance->CpuName, "MC6809");

      break;

    case 1:
      _emu->CpuType = 1;

      strcpy(instance->CpuName, "HD6309");

      break;
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetSpeedThrottle(unsigned char throttle)
  {
    instance->Throttle = throttle;
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl GetSpeedThrottle()
  {
    return(instance->Throttle);
  }
}

extern "C" {
  __declspec(dllexport) unsigned __stdcall CartLoad(void* dummy)
  {
    static EmuState* _emu = GetEmuState();

    LoadCart(_emu);

    _emu->EmulationRunning = true;
    instance->DialogOpen = false;

    return(NULL);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl LoadPack(void)
  {
    unsigned threadID;

    if (instance->DialogOpen) {
      return;
    }

    instance->DialogOpen = true;

    _beginthreadex(NULL, 0, &CartLoad, CreateEvent(NULL, FALSE, FALSE, NULL), 0, &threadID);
  }
}

// Force keys up if main widow keyboard focus is lost.  Otherwise
// down keys will cause issues with OS-9 on return
// Send key up events to keyboard handler for saved keys
extern "C" {
  __declspec(dllexport) void __cdecl SendSavedKeyEvents() {
    if (instance->SC_save1) {
      vccKeyboardHandleKey(instance->KB_save1, instance->SC_save1, kEventKeyUp);
    }

    if (instance->SC_save2) {
      vccKeyboardHandleKey(instance->KB_save2, instance->SC_save2, kEventKeyUp);
    }

    instance->SC_save1 = 0;
    instance->SC_save2 = 0;
  }
}
