#pragma once

#include <windows.h>

typedef struct {
  //[Version]
  //"Release" //ConfigState ==> char AppName[MAX_LOADSTRING]; //## WRITE-ONLY ##//

  //[CPU]
  unsigned char	CPUMultiplier;
  unsigned char	FrameSkip;
  unsigned char	SpeedThrottle;
  unsigned char	CpuType;
  unsigned short MaxOverclock;

  //[Audio]
  char SoundCardName[64];
  unsigned short AudioRate;

  //[Video]
  unsigned char	MonitorType;
  unsigned char PaletteType;
  unsigned char	ScanLines;
  unsigned char	AllowResize;
  unsigned char	ForceAspect;
  unsigned short RememberSize;
  unsigned short WindowSizeX;
  unsigned short WindowSizeY;

  //[Memory]
  unsigned char	RamSize;
  char ExternalBasicImage[MAX_PATH];

  //[Misc]
  unsigned char	AutoStart;
  unsigned char	CartAutoStart;
  unsigned char	KeyMapIndex;

  //[Module]
  char ModulePath[MAX_PATH];

  //[LeftJoyStick]  //struct {} JoyStick
  //UseMouse=1
  //Left=75
  //Right=77
  //Up=72
  //Down=80
  //Fire1=59
  //Fire2=60
  //DiDevice=0
  //HiResDevice=0

  //[RightJoyStick]  //struct {} JoyStick
  //UseMouse=1
  //Left=75
  //Right=77
  //Up=72
  //Down=80
  //Fire1=59
  //Fire2=60
  //DiDevice=0
  //HiResDevice=0

  //[DefaultPaths]
  char CassPath[MAX_PATH];
  //"PakPath" //PakInterfaceState ==> char PakPath[MAX_PATH]  
  char FloppyPath[MAX_PATH];
  char CoCoRomPath[MAX_PATH];
  //"SerialCaptureFilePath" //SelectSerialCaptureFile(...) ==> char captureFilePath[MAX_PATH];
 
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

  char PathtoExe[MAX_PATH]; //**DERIVED**//
} ConfigModel;
