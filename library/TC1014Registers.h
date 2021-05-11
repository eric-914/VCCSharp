#pragma once

typedef struct {
  unsigned char EnhancedFIRQFlag;
  unsigned char EnhancedIRQFlag;
  unsigned char VDG_Mode;
  unsigned char Dis_Offset;
  unsigned char MPU_Rate;
  unsigned char LastIrq;
  unsigned char LastFirq;

  unsigned short VerticalOffsetRegister;

  short InterruptTimer;

  unsigned char GimeRegisters[256];
  unsigned char* Rom;
} TC1014RegistersState;

extern "C" __declspec(dllexport) void __cdecl GimeAssertKeyboardInterrupt(void);
