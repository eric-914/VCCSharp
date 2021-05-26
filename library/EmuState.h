#pragma once

#include <windows.h>

#define RESET_NONE      0
#define RESET_HARD      2

typedef struct
{
  HINSTANCE Resources;

  HWND WindowHandle;
  HWND ConfigDialog;

  POINT WindowSize;

  unsigned char	DoubleSpeedFlag;
  unsigned char	DoubleSpeedMultiplier;
  unsigned char	EmulationRunning;
  unsigned char	ResetPending;
  unsigned char	TurboSpeedFlag;

  double CPUCurrentSpeed;
} EmuState;
