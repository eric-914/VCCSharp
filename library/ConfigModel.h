#pragma once

#include "defines.h"

typedef struct {
  char Release[MAX_LOADSTRING];

  unsigned char	CPUMultiplier;
  unsigned char	FrameSkip;
  unsigned char	SpeedThrottle;
  unsigned char	CpuType;
  unsigned short MaxOverclock;

  char SoundCardName[MAX_LOADSTRING];
  unsigned short AudioRate;

  unsigned char	MonitorType;
  unsigned char PaletteType;
  unsigned char	ScanLines;
  unsigned char	ForceAspect;
  unsigned short RememberSize;

  short WindowSizeX;
  short WindowSizeY;

  unsigned char	RamSize;
  char ExternalBasicImage[MAX_PATH];

  unsigned char	AutoStart;
  unsigned char	CartAutoStart;
  unsigned char	KeyMapIndex;

  char ModulePath[MAX_PATH];

  char CassPath[MAX_PATH];
  char PakPath[MAX_PATH];
  char FloppyPath[MAX_PATH];
  char CoCoRomPath[MAX_PATH];
  char SerialCaptureFilePath[MAX_PATH];
} ConfigModel;
