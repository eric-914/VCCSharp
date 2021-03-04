#pragma once

#include <windows.h>

//Reset Pending states
#define RESET_CLEAR     0
#define RESET_SOFT      1
#define RESET_HARD      2
#define RESET_CLS       3
#define RESET_CLS_SYNCH 4

typedef struct
{
  HINSTANCE Resources;

  HWND WindowHandle;
  HWND ConfigDialog;

  POINT WindowSize;

  unsigned char	RamSize;

  double CPUCurrentSpeed;

  unsigned char	DoubleSpeedMultiplier;
  unsigned char	DoubleSpeedFlag;
  unsigned char	TurboSpeedFlag;
  unsigned char	CpuType;
  unsigned char	FrameSkip;
  unsigned char	BitDepth;

  long SurfacePitch;

  unsigned short LineCounter;

  unsigned char	ScanLines;
  unsigned char	EmulationRunning;
  unsigned char	ResetPending;

  unsigned char	FullScreen;

  char StatusLine[256];
} EmuState;
