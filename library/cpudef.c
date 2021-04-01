#include <windows.h>

#include "cpudef.h"

#include "HD6309.h"
#include "MC6809.h"

CPU* InitializeInstance(CPU*);

static CPU* instance = InitializeInstance(new CPU());

CPU* InitializeInstance(CPU* p) {
  p->CPUAssertInterrupt = NULL;

  return p;
}

extern "C" {
  __declspec(dllexport) void __cdecl SetCPUToHD6309() {
    instance->CPUAssertInterrupt = HD6309AssertInterrupt;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetCPUToMC6809() {
    instance->CPUAssertInterrupt = MC6809AssertInterrupt;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl CPUAssertInterrupt(unsigned char irq, unsigned char flag)
  {
    instance->CPUAssertInterrupt(irq, flag);
  }
}
