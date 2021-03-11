#include <windows.h>

#include "cpudef.h"

#include "HD6309.h"
#include "MC6809.h"

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
  __declspec(dllexport) void __cdecl SetCPUToHD6309() {
    instance->CPUInit = HD6309Init;
    instance->CPUExec = HD6309Exec;
    instance->CPUReset = HD6309Reset;
    instance->CPUAssertInterrupt = HD6309AssertInterrupt;
    instance->CPUDeAssertInterrupt = HD6309DeAssertInterrupt;
    instance->CPUForcePC = HD6309ForcePC;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetCPUToMC6809() {
    instance->CPUInit = MC6809Init;
    instance->CPUExec = MC6809Exec;
    instance->CPUReset = MC6809Reset;
    instance->CPUAssertInterrupt = MC6809AssertInterrupt;
    instance->CPUDeAssertInterrupt = MC6809DeAssertInterrupt;
    instance->CPUForcePC = MC6809ForcePC;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl CPUAssertInterrupt(unsigned char irq, unsigned char flag)
  {
    instance->CPUAssertInterrupt(irq, flag);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl CPUDeAssertInterrupt(unsigned char irq)
  {
    instance->CPUDeAssertInterrupt(irq);
  }
}

extern "C" {
  __declspec(dllexport) int __cdecl CPUExec(int cycle)
  {
    return instance->CPUExec(cycle);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl CPUForcePC(unsigned short xferAddress)
  {
    instance->CPUForcePC(xferAddress);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl CPUInit()
  {
    instance->CPUInit();
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl CPUReset()
  {
    instance->CPUReset();
  }
}
