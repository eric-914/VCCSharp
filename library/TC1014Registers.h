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

extern "C" __declspec(dllexport) TC1014RegistersState * __cdecl GetTC1014RegistersState();

extern "C" __declspec(dllexport) void __cdecl GimeAssertHorzInterrupt(void);
extern "C" __declspec(dllexport) void __cdecl GimeAssertKeyboardInterrupt(void);
extern "C" __declspec(dllexport) void __cdecl GimeAssertTimerInterrupt(void);
extern "C" __declspec(dllexport) void __cdecl MC6883Reset();
extern "C" __declspec(dllexport) void __cdecl SetGimeFIRQSteering(unsigned char data);
extern "C" __declspec(dllexport) void __cdecl SetGimeIRQSteering(unsigned char data);
extern "C" __declspec(dllexport) void __cdecl SetInit0(unsigned char data);
extern "C" __declspec(dllexport) void __cdecl SetInit1(unsigned char data);
extern "C" __declspec(dllexport) void __cdecl SetTimerLSB(unsigned char data);
extern "C" __declspec(dllexport) void __cdecl SetTimerMSB(unsigned char data);
