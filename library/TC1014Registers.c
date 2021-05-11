#include "TC1014Registers.h"
#include "TC1014MMU.h"
#include "Graphics.h"
#include "Keyboard.h"
#include "CoCo.h"
#include "Emu.h"

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
  p->VDG_Mode = 0;
  p->Dis_Offset = 0;
  p->MPU_Rate = 0;
  p->LastIrq = 0;
  p->LastFirq = 0;
  p->VerticalOffsetRegister = 0;
  p->InterruptTimer = 0;

  return p;
}

extern "C" {
  __declspec(dllexport) void __cdecl GimeAssertKeyboardInterrupt(void)
  {
    TC1014RegistersState* registersState = GetTC1014RegistersState();

    if (((registersState->GimeRegisters[0x93] & 2) != 0) && (registersState->EnhancedFIRQFlag == 1)) {
      CPUAssertInterrupt(FIRQ, 0);

      registersState->LastFirq = registersState->LastFirq | 2;
    }
    else if (((registersState->GimeRegisters[0x92] & 2) != 0) && (registersState->EnhancedIRQFlag == 1)) {
      CPUAssertInterrupt(IRQ, 0);

      registersState->LastIrq = registersState->LastIrq | 2;
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl GimeAssertTimerInterrupt()
  {
    TC1014RegistersState* registersState = GetTC1014RegistersState();

    if (((registersState->GimeRegisters[0x93] & 32) != 0) && (registersState->EnhancedFIRQFlag == 1)) {
      CPUAssertInterrupt(FIRQ, 0);

      registersState->LastFirq = registersState->LastFirq | 32;
    }
    else if (((registersState->GimeRegisters[0x92] & 32) != 0) && (registersState->EnhancedIRQFlag == 1)) {
      CPUAssertInterrupt(IRQ, 0);

      registersState->LastIrq = registersState->LastIrq | 32;
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl GimeAssertHorzInterrupt()
  {
    TC1014RegistersState* registersState = GetTC1014RegistersState();

    if (((registersState->GimeRegisters[0x93] & 16) != 0) && (registersState->EnhancedFIRQFlag == 1)) {
      CPUAssertInterrupt(FIRQ, 0);

      registersState->LastFirq = registersState->LastFirq | 16;
    }
    else if (((registersState->GimeRegisters[0x92] & 16) != 0) && (registersState->EnhancedIRQFlag == 1)) {
      CPUAssertInterrupt(IRQ, 0);

      registersState->LastIrq = registersState->LastIrq | 16;
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl MC6883Reset()
  {
    TC1014RegistersState* registersState = GetTC1014RegistersState();

    registersState->VDG_Mode = 0;
    registersState->Dis_Offset = 0;
    registersState->MPU_Rate = 0;

    registersState->Rom = GetInternalRomPointer();
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetGimeFIRQSteering(unsigned char data) //93
  {
    TC1014RegistersState* registersState = GetTC1014RegistersState();

    if ((registersState->GimeRegisters[0x92] & 2) | (registersState->GimeRegisters[0x93] & 2)) {
      GimeSetKeyboardInterruptState(1);
    }
    else {
      GimeSetKeyboardInterruptState(0);
    }

    if ((registersState->GimeRegisters[0x92] & 8) | (registersState->GimeRegisters[0x93] & 8)) {
      SetVertInterruptState(1);
    }
    else {
      SetVertInterruptState(0);
    }

    if ((registersState->GimeRegisters[0x92] & 16) | (registersState->GimeRegisters[0x93] & 16)) {
      SetHorzInterruptState(1);
    }
    else {
      SetHorzInterruptState(0);
    }

    // Moon Patrol Demo Using Timer for FIRQ Side Scroll 
    if ((registersState->GimeRegisters[0x92] & 32) | (registersState->GimeRegisters[0x93] & 32)) {
      SetTimerInterruptState(1);
    }
    else {
      SetTimerInterruptState(0);
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetGimeIRQSteering(unsigned char data) //92
  {
    TC1014RegistersState* registersState = GetTC1014RegistersState();

    if ((registersState->GimeRegisters[0x92] & 2) | (registersState->GimeRegisters[0x93] & 2)) {
      GimeSetKeyboardInterruptState(1);
    }
    else {
      GimeSetKeyboardInterruptState(0);
    }

    if ((registersState->GimeRegisters[0x92] & 8) | (registersState->GimeRegisters[0x93] & 8)) {
      SetVertInterruptState(1);
    }
    else {
      SetVertInterruptState(0);
    }

    if ((registersState->GimeRegisters[0x92] & 16) | (registersState->GimeRegisters[0x93] & 16)) {
      SetHorzInterruptState(1);
    }
    else {
      SetHorzInterruptState(0);
    }

    if ((registersState->GimeRegisters[0x92] & 32) | (registersState->GimeRegisters[0x93] & 32)) {
      SetTimerInterruptState(1);
    }
    else {
      SetTimerInterruptState(0);
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetInit0(unsigned char data)
  {
    TC1014RegistersState* registersState = GetTC1014RegistersState();

    SetCompatMode(!!(data & 128));
    SetMmuEnabled(!!(data & 64)); //MMUEN
    SetRomMap(data & 3);			//MC0-MC1
    SetVectors(data & 8);			//MC3

    registersState->EnhancedFIRQFlag = (data & 16) >> 4;
    registersState->EnhancedIRQFlag = (data & 32) >> 5;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetInit1(unsigned char data)
  {
    SetMmuTask(data & 1);			//TR
    SetTimerClockRate(data & 32);	//TINS
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetTimerLSB(unsigned char data) //95
  {
    unsigned short temp;

    TC1014RegistersState* registersState = GetTC1014RegistersState();

    temp = ((registersState->GimeRegisters[0x94] << 8) + registersState->GimeRegisters[0x95]) & 4095;

    SetInterruptTimer(temp);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetTimerMSB(unsigned char data) //94
  {
    unsigned short temp;

    TC1014RegistersState* registersState = GetTC1014RegistersState();

    temp = ((registersState->GimeRegisters[0x94] << 8) + registersState->GimeRegisters[0x95]) & 4095;

    SetInterruptTimer(temp);
  }
}
