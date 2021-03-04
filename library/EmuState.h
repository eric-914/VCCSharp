#pragma once

#include <windows.h>

typedef struct
{
  HINSTANCE Resources;

  HWND WindowHandle;
  HWND ConfigDialog;

  POINT WindowSize;

  unsigned char	RamSize;

  double CPUCurrentSpeed;

  unsigned char	DoubleSpeedMultiplyer;
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
