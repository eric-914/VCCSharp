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
