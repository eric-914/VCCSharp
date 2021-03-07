#pragma once

#include <windows.h>

//Reset Pending states
#define RESET_NONE      0
#define RESET_SOFT      1
#define RESET_HARD      2
#define RESET_CLS       3
#define RESET_CLS_SYNCH 4

//Bit Depth states
#define BIT_8   0
#define BIT_16  1
#define BIT_24  2
#define BIT_32  3

typedef struct
{
  HINSTANCE Resources;

  HWND WindowHandle;
  HWND ConfigDialog;

  POINT WindowSize;

  unsigned char	BitDepth;
  unsigned char	CpuType;
  unsigned char	DoubleSpeedFlag;
  unsigned char	DoubleSpeedMultiplier;
  unsigned char	EmulationRunning;
  unsigned char	FrameSkip;
  unsigned char	FullScreen;
  unsigned char	RamSize;
  unsigned char	ResetPending;
  unsigned char	ScanLines;
  unsigned char	TurboSpeedFlag;

  unsigned short FrameCounter;
  unsigned short LineCounter;

  double CPUCurrentSpeed;
  
  long SurfacePitch;

  char StatusLine[256];
} EmuState;
