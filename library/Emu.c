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

  p->FrameCounter = 0;
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
    CoCoState* cocoState = GetCoCoState();

    cocoState->OverClock = 1;

    instance->DoubleSpeedFlag = double_speed;

    if (instance->DoubleSpeedFlag) {
      cocoState->OverClock = instance->DoubleSpeedMultiplier * instance->TurboSpeedFlag;
    }

    instance->CPUCurrentSpeed = .894;

    if (instance->DoubleSpeedFlag) {
      instance->CPUCurrentSpeed *= ((double)instance->DoubleSpeedMultiplier * (double)instance->TurboSpeedFlag);
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetCPUMultiplier(unsigned char multiplier)
  {
    instance->DoubleSpeedMultiplier = multiplier;

    SetCPUMultiplierFlag(instance->DoubleSpeedFlag);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetTurboMode(unsigned char data)
  {
    CoCoState* cocoState = GetCoCoState();

    cocoState->OverClock = 1;

    instance->TurboSpeedFlag = (data & 1) + 1;

    if (instance->DoubleSpeedFlag) {
      cocoState->OverClock = instance->DoubleSpeedMultiplier * instance->TurboSpeedFlag;
    }

    instance->CPUCurrentSpeed = .894;

    if (instance->DoubleSpeedFlag) {
      instance->CPUCurrentSpeed *= ((double)instance->DoubleSpeedMultiplier * (double)instance->TurboSpeedFlag);
    }
  }
}

