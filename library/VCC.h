#pragma once

#include <windows.h>

#include "defines.h"
#include "systemstate.h"
#include "CmdLineArguments.h"

#define TH_RUNNING	0
#define TH_REQWAIT	1
#define TH_WAITING	2

//Type 0= HEAD TAG 1= Slave Tag 2=StandAlone
#define	HEAD 0
#define SLAVE 1
#define STANDALONE 2

typedef struct
{
  HANDLE hEventThread;
  HANDLE hEmuThread;  // Message handlers
  MSG  msg;

  char CpuName[20];
  char AppName[MAX_LOADSTRING];
  unsigned char FlagEmuStop;

  char QuickLoadFile[256];
  bool BinaryRunning;
  bool DialogOpen;
  unsigned char Throttle;
  unsigned char AutoStart;
  unsigned char Qflag;

  //--------------------------------------------------------------------------
  // When the main window is about to lose keyboard focus there are one
  // or two keys down in the emulation that must be raised.  These routines
  // track the last two key down events so they can be raised when needed.
  //--------------------------------------------------------------------------
  unsigned char SC_save1;
  unsigned char SC_save2;
  unsigned char KB_save1;
  unsigned char KB_save2;
  int KeySaveToggle;

  SystemState SystemState;
} VccState;

extern "C" __declspec(dllexport) VccState * __cdecl GetVccState();

extern "C" __declspec(dllexport) unsigned __stdcall CartLoad(void*);
extern "C" __declspec(dllexport) unsigned __stdcall EmuLoopRun(void*);

extern "C" __declspec(dllexport) unsigned char __cdecl SetAutoStart(unsigned char);
extern "C" __declspec(dllexport) unsigned char __cdecl SetCPUMultiplayer(unsigned char);
extern "C" __declspec(dllexport) unsigned char __cdecl SetCpuType(unsigned char);
extern "C" __declspec(dllexport) unsigned char __cdecl SetFrameSkip(unsigned char);
extern "C" __declspec(dllexport) unsigned char __cdecl SetRamSize(unsigned char);
extern "C" __declspec(dllexport) unsigned char __cdecl SetSpeedThrottle(unsigned char);

extern "C" __declspec(dllexport) void __cdecl EmuLoop();
extern "C" __declspec(dllexport) void __cdecl HardReset(SystemState*);
extern "C" __declspec(dllexport) void __cdecl LoadIniFile();
extern "C" __declspec(dllexport) void __cdecl LoadPack();
//extern "C" __declspec(dllexport) void __cdecl Reboot();
extern "C" __declspec(dllexport) void __cdecl SaveConfig();
extern "C" __declspec(dllexport) void __cdecl SaveLastTwoKeyDownEvents(unsigned char, unsigned char);
extern "C" __declspec(dllexport) void __cdecl SendSavedKeyEvents();
extern "C" __declspec(dllexport) void __cdecl SetCPUMultiplayerFlag(unsigned char);
extern "C" __declspec(dllexport) void __cdecl SetTurboMode(unsigned char);
extern "C" __declspec(dllexport) void __cdecl SoftReset();

extern "C" __declspec(dllexport) HMODULE __cdecl LoadResources();
extern "C" __declspec(dllexport) void __cdecl CheckQuickLoad(char* qLoadFile);
extern "C" __declspec(dllexport) void __cdecl CreatePrimaryWindow();
extern "C" __declspec(dllexport) void CheckScreenModeChange();

extern "C" __declspec(dllexport) void __cdecl VccStartup(HINSTANCE hInstance, CmdLineArguments cmdArg);
extern "C" __declspec(dllexport) void __cdecl VccRun();
extern "C" __declspec(dllexport) INT __cdecl VccShutdown();

