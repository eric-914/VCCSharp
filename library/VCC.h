#pragma once

#include <windows.h>

#include "VccState.h"
#include "EmuState.h"
#include "CmdLineArguments.h"

#define TH_RUNNING	0
#define TH_REQWAIT	1
#define TH_WAITING	2

//Menu "Actions"
#define MENU_FLUSH   0
#define MENU_DONE    1
#define MENU_REFRESH 2

//Type 0= HEAD TAG 1= Slave Tag 2=StandAlone
#define	HEAD 0
#define SLAVE 1
#define STANDALONE 2

extern "C" __declspec(dllexport) VccState * __cdecl GetVccState();
extern "C" __declspec(dllexport) EmuState* __cdecl GetEmuState();

extern "C" __declspec(dllexport) unsigned __stdcall CartLoad(void*);
extern "C" __declspec(dllexport) unsigned __stdcall EmuLoopRun(void*);

extern "C" __declspec(dllexport) unsigned char __cdecl SetAutoStart(unsigned char);
extern "C" __declspec(dllexport) unsigned char __cdecl SetCPUMultiplier(unsigned char);
extern "C" __declspec(dllexport) unsigned char __cdecl SetCpuType(unsigned char);
extern "C" __declspec(dllexport) unsigned char __cdecl SetFrameSkip(unsigned char);
extern "C" __declspec(dllexport) unsigned char __cdecl SetRamSize(unsigned char);
extern "C" __declspec(dllexport) unsigned char __cdecl SetSpeedThrottle(unsigned char);

extern "C" __declspec(dllexport) void __cdecl EmuLoop();
extern "C" __declspec(dllexport) void __cdecl HardReset(EmuState*);
extern "C" __declspec(dllexport) void __cdecl LoadIniFile();
extern "C" __declspec(dllexport) void __cdecl LoadPack();
extern "C" __declspec(dllexport) void __cdecl SaveConfig();
extern "C" __declspec(dllexport) void __cdecl SaveLastTwoKeyDownEvents(unsigned char, unsigned char);
extern "C" __declspec(dllexport) void __cdecl SendSavedKeyEvents();
extern "C" __declspec(dllexport) void __cdecl SetCPUMultiplierFlag(unsigned char);
extern "C" __declspec(dllexport) void __cdecl SetTurboMode(unsigned char);
extern "C" __declspec(dllexport) void __cdecl SoftReset();

extern "C" __declspec(dllexport) void __cdecl CheckQuickLoad(char* qLoadFile);
extern "C" __declspec(dllexport) void __cdecl CreatePrimaryWindow();
extern "C" __declspec(dllexport) void CheckScreenModeChange();

extern "C" __declspec(dllexport) void __cdecl VccStartupThreading();
extern "C" __declspec(dllexport) void __cdecl VccStartup(HINSTANCE hInstance, CmdLineArguments* cmdArg, EmuState* emu);
extern "C" __declspec(dllexport) void __cdecl VccRun();
extern "C" __declspec(dllexport) INT __cdecl VccShutdown();

