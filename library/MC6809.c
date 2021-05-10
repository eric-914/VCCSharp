#include "MC6809.h"

MC6809State* InitializeInstance(MC6809State*);

//TODO: Startup doesn't initialize this instance in the expected order

extern "C" {
  __declspec(dllexport) MC6809State* __cdecl GetMC6809State() {
    return InitializeInstance(new MC6809State());
  }
}

MC6809State* InitializeInstance(MC6809State* p) {
  p->InInterrupt = 0;
  p->IRQWaiter = 0;
  p->PendingInterrupts = 0;
  p->SyncWaiting = 0;
  p->CycleCounter = 0;

  return p;
}
