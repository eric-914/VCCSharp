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
    static EmuState* _emu = GetEmuState();

    _emu->TurboSpeedFlag = (data & 1) + 1;

    SetClockSpeed(1);

    if (_emu->DoubleSpeedFlag) {
      SetClockSpeed(_emu->DoubleSpeedMultiplier * _emu->TurboSpeedFlag);
    }

    _emu->CPUCurrentSpeed = .894;

    if (_emu->DoubleSpeedFlag) {
      _emu->CPUCurrentSpeed *= ((double)_emu->DoubleSpeedMultiplier * (double)_emu->TurboSpeedFlag);
    }
  }
}
