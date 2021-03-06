#include "Emu.h"

#include "CoCo.h"
#include "Graphics.h"
#include "PAKInterface.h"
#include "TC1014MMU.h"
#include "TC1014Registers.h"
#include "Config.h"
#include "Audio.h"

#include "MC6821.h"
#include "HD6309.h"
#include "MC6809.h"

#include "cpudef.h"
#include "defines.h"

EmuState* InitializeInstance(EmuState*);

static EmuState* instance;

EmuState* GetInstance() {
  if (instance == NULL) {
    instance = InitializeInstance(new EmuState());
  }

  return instance;
}

extern "C" {
  __declspec(dllexport) EmuState* __cdecl GetEmuState() {
    return GetInstance();
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetEmuState(EmuState* emuState) {
    instance = emuState;
  }
}

EmuState* InitializeInstance(EmuState* p) {
  p->RamSize;

  p->CPUCurrentSpeed = .894;

  p->DoubleSpeedMultiplier = 2;
  p->DoubleSpeedFlag = 0;
  p->TurboSpeedFlag = 1;
  p->FrameSkip = 0;

  p->SurfacePitch = 0;

  p->LineCounter = 0;
  p->ScanLines = false;
  p->EmulationRunning = false;
  p->ResetPending = RESET_NONE;
  p->FullScreen = false;

  strcpy(p->StatusLine, "");

  return p;
}

extern "C" {
  __declspec(dllexport) void __cdecl SetCPUMultiplierFlag(unsigned char double_speed)
  {
    static EmuState* _emu = GetEmuState();

    SetClockSpeed(1);

    _emu->DoubleSpeedFlag = double_speed;

    if (_emu->DoubleSpeedFlag) {
      SetClockSpeed(_emu->DoubleSpeedMultiplier * _emu->TurboSpeedFlag);
    }

    _emu->CPUCurrentSpeed = .894;

    if (_emu->DoubleSpeedFlag) {
      _emu->CPUCurrentSpeed *= ((double)_emu->DoubleSpeedMultiplier * (double)_emu->TurboSpeedFlag);
    }
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl SetCPUMultiplier(unsigned char multiplier)
  {
    static EmuState* _emu = GetEmuState();

    if (multiplier != QUERY)
    {
      _emu->DoubleSpeedMultiplier = multiplier;

      SetCPUMultiplierFlag(_emu->DoubleSpeedFlag);
    }

    return(_emu->DoubleSpeedMultiplier);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetTurboMode(unsigned char data)
  {
    instance->TurboSpeedFlag = (data & 1) + 1;

    SetClockSpeed(1);

    if (instance->DoubleSpeedFlag) {
      SetClockSpeed(instance->DoubleSpeedMultiplier * instance->TurboSpeedFlag);
    }

    instance->CPUCurrentSpeed = .894;

    if (instance->DoubleSpeedFlag) {
      instance->CPUCurrentSpeed *= ((double)instance->DoubleSpeedMultiplier * (double)instance->TurboSpeedFlag);
    }
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl SetFrameSkip(unsigned char skip)
  {
    if (skip != QUERY) {
      instance->FrameSkip = skip;
    }

    return(instance->FrameSkip);
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl SetRamSize(unsigned char size)
  {
    if (size != QUERY) {
      instance->RamSize = size;
    }

    return(instance->RamSize);
  }
}

void GimeReset(void)
{
  ResetGraphicsState();

  MakeRGBPalette();
  MakeCMPpalette(GetPaletteType());

  CocoReset();
  ResetAudio();
}

extern "C" {
  __declspec(dllexport) void __cdecl SoftReset()
  {
    MC6883Reset();
    MC6821_PiaReset();

    GetCPU()->CPUReset();

    GimeReset();
    MmuReset();
    CopyRom();
    ResetBus();

    static EmuState* _emu = GetEmuState();

    _emu->TurboSpeedFlag = 1;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl HardReset(EmuState* emuState)
  {
    //Allocate RAM/ROM & copy ROM Images from source
    if (MmuInit(emuState->RamSize) == NULL)
    {
      MessageBox(NULL, "Can't allocate enough RAM, out of memory", "Error", 0);

      exit(0);
    }

    CPU* cpu = GetCPU();

    if (emuState->CpuType == 1)
    {
      cpu->CPUInit = HD6309Init;
      cpu->CPUExec = HD6309Exec;
      cpu->CPUReset = HD6309Reset;
      cpu->CPUAssertInterrupt = HD6309AssertInterrupt;
      cpu->CPUDeAssertInterrupt = HD6309DeAssertInterrupt;
      cpu->CPUForcePC = HD6309ForcePC;
    }
    else
    {
      cpu->CPUInit = MC6809Init;
      cpu->CPUExec = MC6809Exec;
      cpu->CPUReset = MC6809Reset;
      cpu->CPUAssertInterrupt = MC6809AssertInterrupt;
      cpu->CPUDeAssertInterrupt = MC6809DeAssertInterrupt;
      cpu->CPUForcePC = MC6809ForcePC;
    }

    MC6821_PiaReset();
    MC6883Reset();	//Captures interal rom pointer for CPU Interrupt Vectors

    cpu->CPUInit();
    cpu->CPUReset();		// Zero all CPU Registers and sets the PC to VRESET

    GimeReset();
    UpdateBusPointer();

    static EmuState* _emu = GetEmuState();

    _emu->TurboSpeedFlag = 1;

    ResetBus();
    SetClockSpeed(1);
  }
}
