#pragma once

typedef struct {
  unsigned char MmuState;
  unsigned short MemPageOffsets[1024];
  unsigned char* Memory;
  unsigned char* InternalRomBuffer;

  unsigned char MmuTask;
  unsigned char MmuEnabled;
  unsigned char RamVectors;

  unsigned char RomMap;
  unsigned char MapType;

  unsigned char CurrentRamConfig;
  unsigned short MmuPrefix;

  unsigned int MemConfig[4];
  unsigned short RamMask[4];
  unsigned char StateSwitch[4];
  unsigned char VectorMask[4];
  unsigned char VectorMaska[4];
  unsigned long VidMask[4];

  unsigned char* MemPages[1024];
  unsigned short MmuRegisters[4][8];
} TC1014MmuState;
