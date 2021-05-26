#include "TC1014RegistersState.h"

#include "cpudef.h"
#include "defines.h"

TC1014RegistersState* InitializeInstance(TC1014RegistersState*);

static TC1014RegistersState* instance = InitializeInstance(new TC1014RegistersState());

extern "C" {
  __declspec(dllexport) TC1014RegistersState* __cdecl GetTC1014RegistersState() {
    return instance;
  }
}

TC1014RegistersState* InitializeInstance(TC1014RegistersState* p) {
  p->EnhancedFIRQFlag = 0;
  p->EnhancedIRQFlag = 0;
  p->LastIrq = 0;
  p->LastFirq = 0;

  return p;
}

/***************************************************
* Used by Keyboard.c
***************************************************/

extern "C" {
  __declspec(dllexport) void __cdecl GimeAssertKeyboardInterrupt(void)
  {
    TC1014RegistersState* registersState = GetTC1014RegistersState();

    if (((registersState->GimeRegisters[0x93] & 2) != 0) && (registersState->EnhancedFIRQFlag == 1)) {
      CPUAssertInterrupt(FIRQ, 0);

      registersState->LastFirq |= 2;
    }
    else if (((registersState->GimeRegisters[0x92] & 2) != 0) && (registersState->EnhancedIRQFlag == 1)) {
      CPUAssertInterrupt(IRQ, 0);

      registersState->LastIrq |= 2;
    }
  }
}
