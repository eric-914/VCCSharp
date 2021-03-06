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

  //p->hEmuThread = NULL;
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
  __declspec(dllexport) unsigned char __cdecl SetAutoStart(unsigned char autostart)
  {
    if (autostart != QUERY) {
      instance->AutoStart = autostart;
    }

    return(instance->AutoStart);
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl SetCpuType(unsigned char cpuType)
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

    return(_emu->CpuType);
  }
}


extern "C" {
  __declspec(dllexport) unsigned char __cdecl SetSpeedThrottle(unsigned char throttle)
  {
    if (throttle != QUERY) {
      instance->Throttle = throttle;
    }

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


extern "C" {
  __declspec(dllexport) void __cdecl EmuLoop() {
    static float fps;
    static unsigned int frameCounter = 0;

    static EmuState* _emu = GetEmuState();

    while (true)
    {
      if (instance->RunState == EMU_RUNSTATE_REQWAIT)
      {
        instance->RunState = EMU_RUNSTATE_WAITING; //Signal Main thread we are waiting

        while (instance->RunState == EMU_RUNSTATE_WAITING) {
          Sleep(1);
        }
      }

      fps = 0;

      StartRender();

      for (uint8_t frames = 1; frames <= _emu->FrameSkip; frames++)
      {
        frameCounter++;

        if (_emu->ResetPending != RESET_NONE) {
          switch (_emu->ResetPending)
          {
          case RESET_SOFT:	//Soft Reset
            SoftReset();
            break;

          case RESET_HARD:	//Hard Reset
            SynchSystemWithConfig(_emu);
            DoCls(_emu);
            HardReset(_emu);

            break;

          case RESET_CLS:
            DoCls(_emu);
            break;

          case RESET_CLS_SYNCH:
            SynchSystemWithConfig(_emu);
            DoCls(_emu);

            break;

          default:
            break;
          }

          _emu->ResetPending = RESET_NONE;
        }

        if (_emu->EmulationRunning) {
          fps += RenderFrame(_emu);
        }
        else {
          fps += Static(_emu);
        }
      }

      EndRender(_emu->FrameSkip);

      fps /= _emu->FrameSkip;

      GetModuleStatus(_emu);

      char ttbuff[256];

      snprintf(ttbuff, sizeof(ttbuff), "Skip:%2.2i | FPS:%3.0f | %s @ %2.2fMhz| %s", _emu->FrameSkip, fps, instance->CpuName, _emu->CPUCurrentSpeed, _emu->StatusLine);

      SetStatusBarText(ttbuff, _emu);

      if (instance->Throttle) { //Do nothing until the frame is over returning unused time to OS
        FrameWait();
      }
    }
  }
}

extern "C" {
  __declspec(dllexport) void __stdcall EmuLoopRun(HANDLE hEvent)
  {
    Sleep(30);
    SetEvent(hEvent);

    EmuLoop();
  }
}

unsigned ThreadLoopRun(void* dummy) {
  HANDLE hEvent = (HANDLE)dummy;

  EmuLoopRun(hEvent);

  return NULL;
}

extern "C" {
  __declspec(dllexport) HANDLE __cdecl CreateThreadHandle(HANDLE hEvent) {
    unsigned threadID;

    return (HANDLE)_beginthreadex(NULL, 0, &ThreadLoopRun, hEvent, 0, &threadID);
  }
}
