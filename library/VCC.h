#pragma once

#include <windows.h>

#include "VccState.h"
#include "EmuState.h"
#include "CmdLineArguments.h"

extern "C" __declspec(dllexport) VccState * __cdecl GetVccState();
extern "C" __declspec(dllexport) EmuState* __cdecl GetEmuState();

extern "C" __declspec(dllexport) unsigned __stdcall CartLoad(void*);
extern "C"  __declspec(dllexport) void __stdcall EmuLoopRun(HANDLE hEvent);

extern "C" __declspec(dllexport) unsigned char __cdecl SetAutoStart(unsigned char);
extern "C" __declspec(dllexport) unsigned char __cdecl SetCpuType(unsigned char);
extern "C" __declspec(dllexport) unsigned char __cdecl SetSpeedThrottle(unsigned char);

extern "C" __declspec(dllexport) void __cdecl EmuLoop();
extern "C" __declspec(dllexport) void __cdecl LoadPack();
extern "C" __declspec(dllexport) void __cdecl SaveLastTwoKeyDownEvents(unsigned char, unsigned char);
extern "C" __declspec(dllexport) void __cdecl SendSavedKeyEvents();

extern "C" __declspec(dllexport) HANDLE __cdecl CreateThreadHandle(HANDLE hEvent);
