#pragma once

typedef struct {
  unsigned char EnhancedFIRQFlag;
  unsigned char EnhancedIRQFlag;
  unsigned char LastIrq;
  unsigned char LastFirq;

  unsigned char GimeRegisters[256];
  unsigned char* Rom;
} TC1014RegistersState;
