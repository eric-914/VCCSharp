#include "Emu.h"

#include "CoCo.h"

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
  p->CPUCurrentSpeed = .894;

  p->DoubleSpeedMultiplier = 2;
  p->DoubleSpeedFlag = 0;
  p->TurboSpeedFlag = 1;

  p->EmulationRunning = false;
  p->ResetPending = RESET_NONE;

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
