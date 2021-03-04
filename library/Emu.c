#include "Emu.h"

EmuState* InitializeInstance(EmuState*);

static EmuState* instance = InitializeInstance(new EmuState());

extern "C" {
  __declspec(dllexport) EmuState* __cdecl GetEmuState() {
    return instance;
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
  p->ResetPending = RESET_CLEAR;
  p->FullScreen = false;

  strcpy(p->StatusLine, "");

  return p;
}
