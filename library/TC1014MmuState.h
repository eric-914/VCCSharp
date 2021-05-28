#pragma once

typedef struct {
  unsigned char MmuState;
  unsigned short MemPageOffsets[1024];

  unsigned char MapType;

  unsigned char CurrentRamConfig;

  unsigned char VectorMask[4];
  unsigned char VectorMaska[4];

  unsigned char* MemPages[1024];
  unsigned short MmuRegisters[4][8];
} TC1014MmuState;
