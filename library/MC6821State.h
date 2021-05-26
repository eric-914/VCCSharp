#pragma once

#include <windows.h>

typedef struct {
  unsigned char Asample;
  unsigned char Ssample;
  unsigned char Csample;
  unsigned char CartInserted;
  unsigned char CartAutoStart;

  unsigned char rega[4];
  unsigned char regb[4];
} MC6821State;
