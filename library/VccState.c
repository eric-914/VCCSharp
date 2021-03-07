#include "VccState.h"

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
  p->Throttle = 0;

  p->RunState = EMU_RUNSTATE_RUNNING;

  strcpy(p->CpuName, "CPUNAME");
  strcpy(p->AppName, "");

  return p;
}
