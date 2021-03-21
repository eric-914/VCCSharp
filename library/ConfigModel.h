#pragma once

#include <windows.h>

#include "defines.h"

#include "JoyStickModel.h"

typedef struct {
  //[LeftJoyStick]  //struct {} JoyStick
  JoystickModel* Left;

  //[RightJoyStick]
  JoystickModel* Right;

  //[Version]
  char Release[MAX_LOADSTRING]; //## WRITE-ONLY ##//

  //[CPU]
  unsigned char	CPUMultiplier;
  unsigned char	FrameSkip;
  unsigned char	SpeedThrottle;
  unsigned char	CpuType;
  unsigned short MaxOverclock;

  //[Audio]
  char SoundCardName[MAX_LOADSTRING];
  unsigned short AudioRate;

  //[Video]
  unsigned char	MonitorType;
  unsigned char PaletteType;
  unsigned char	ScanLines;
  unsigned char	ForceAspect;
  unsigned short RememberSize;

  short WindowSizeX;
  short WindowSizeY;

  //[Memory]
  unsigned char	RamSize;
  char ExternalBasicImage[MAX_PATH]; //## READ-ONLY ##//

  //[Misc]
  unsigned char	AutoStart;
  unsigned char	CartAutoStart;
  unsigned char	KeyMapIndex;

  //[Module]
  char ModulePath[MAX_PATH];

  //[DefaultPaths]
  char CassPath[MAX_PATH];
  char PakPath[MAX_PATH];
  char FloppyPath[MAX_PATH];
  char CoCoRomPath[MAX_PATH]; //## READ-ONLY ##//
  char SerialCaptureFilePath[MAX_PATH];
 
  //[FD-502]  //### MODULE SPECIFIC ###//
  //DiskRom=1
  //RomPath=
  //Persist=1
  //Disk#0=
  //Disk#1=
  //Disk#2=
  //Disk#3=
  //ClkEnable=1
  //TurboDisk=1

  //[MPI]     //### MODULE SPECIFIC ###//
  //SWPOSITION=3
  //PesistPaks=1
  //SLOT1=
  //SLOT2=
  //SLOT3=
  //SLOT4=C:\CoCo\Mega-Bug (1982) (26-3076) (Tandy).ccc
  //"MPIPath"   //TODO: Originally in [DefaultPaths]

  //[HardDisk]  //### MODULE SPECIFIC ###// 
  //"HardDiskPath"  //TODO: Originally in [DefaultPaths]

  //[SuperIDE]  //### MODULE SPECIFIC ###//
  //"SuperIDEPath"  //TODO: Originally in [DefaultPaths]
} ConfigModel;
