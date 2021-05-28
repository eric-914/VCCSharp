#pragma once

typedef struct {
  unsigned char EnhancedFIRQFlag;
  unsigned char EnhancedIRQFlag;
  unsigned char LastIrq;
  unsigned char LastFirq;

  unsigned char GimeRegisters[256];

  unsigned char MmuState;
  unsigned short MemPageOffsets[1024];

  unsigned char MapType;

  unsigned char CurrentRamConfig;

  unsigned char VectorMask[4];
  unsigned char VectorMaska[4];

  unsigned char* MemPages[1024];
  unsigned short MmuRegisters[4][8];
} TC1014State;

extern "C" __declspec(dllexport) void __cdecl GimeAssertKeyboardInterrupt(void);

extern "C" __declspec(dllexport) unsigned char __cdecl MemRead8(unsigned short);
extern "C" __declspec(dllexport) void __cdecl MemWrite8(unsigned char, unsigned short);
