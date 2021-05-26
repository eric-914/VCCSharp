#include "Emu.h"

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
  p->EmulationRunning = false;
  p->ResetPending = RESET_NONE;

  return p;
}
