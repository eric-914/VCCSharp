#include <windows.h>

typedef struct
{
  void (*CPUAssertInterrupt)(unsigned char, unsigned char);
} CPU;

CPU* InitializeInstance(CPU*);

static CPU* instance = InitializeInstance(new CPU());

CPU* InitializeInstance(CPU* p) {
  p->CPUAssertInterrupt = NULL;

  return p;
}

extern "C" {
  __declspec(dllexport) void __cdecl CPUAssertInterrupt(unsigned char irq, unsigned char flag)
  {
    instance->CPUAssertInterrupt(irq, flag);
  }
}
