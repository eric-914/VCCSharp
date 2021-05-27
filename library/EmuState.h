#pragma once

#include <windows.h>

#define RESET_NONE      0
#define RESET_HARD      2

typedef struct
{
  unsigned char	EmulationRunning;
  unsigned char	ResetPending;

  unsigned char PakPath[256];
} EmuState;
