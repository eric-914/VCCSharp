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

  return p;
}

extern "C" {
  __declspec(dllexport) void __cdecl SetCPUToHD6309() {
    instance->CPUAssertInterrupt = HD6309AssertInterrupt;
    instance->CPUDeAssertInterrupt = HD6309DeAssertInterrupt;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetCPUToMC6809() {
    instance->CPUAssertInterrupt = MC6809AssertInterrupt;
    instance->CPUDeAssertInterrupt = MC6809DeAssertInterrupt;
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
