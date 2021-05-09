#include "MC6809.h"

MC6809State* InitializeInstance(MC6809State*);

//TODO: Startup doesn't initialize this instance in the expected order

static MC6809State* GetInstance();

static MC6809State* instance = GetInstance();

MC6809State* GetInstance() {
  return (instance ? instance : (instance = InitializeInstance(new MC6809State())));
}

extern "C" {
  __declspec(dllexport) MC6809State* __cdecl GetMC6809State() {
    return GetInstance();
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

extern "C" {
  __declspec(dllexport) void __cdecl MC6809AssertInterrupt(unsigned char interrupt, unsigned char waiter) // 4 nmi 2 firq 1 irq
  {
    instance->SyncWaiting = 0;
    instance->PendingInterrupts |= (1 << (interrupt - 1));
    instance->IRQWaiter = waiter;
  }
}
