#include "Emu.h"

#include "CoCo.h"

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
