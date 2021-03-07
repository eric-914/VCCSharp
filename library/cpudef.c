#include <windows.h>

#include "cpudef.h"

CPU* InitializeInstance(CPU*);

static CPU* instance = InitializeInstance(new CPU());

extern "C" {
  __declspec(dllexport) CPU* __cdecl GetCPU() {
    return instance;
  }
}

CPU* InitializeInstance(CPU* p) {
  p->CPUAssertInterrupt = NULL;
  p->CPUDeAssertInterrupt = NULL;
  p->CPUExec = NULL;
  p->CPUForcePC = NULL;
  p->CPUInit = NULL;
  p->CPUReset = NULL;

  return p;
}

extern "C" {
  __declspec(dllexport) void __cdecl CPUReset()
  {
    instance->CPUReset();
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl CPUInit()
  {
    instance->CPUInit();
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl CPUForcePC(unsigned short xferAddress)
  {
    instance->CPUForcePC(xferAddress);
  }
}
