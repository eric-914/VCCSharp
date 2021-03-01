#pragma once

#include "CoCoState.h"

extern "C" __declspec(dllexport) CoCoState * __cdecl GetCoCoState();

extern "C" __declspec(dllexport) /* _inline */ int __cdecl CPUCycle();

extern "C" __declspec(dllexport) float __cdecl RenderFrame(SystemState*);

extern "C" __declspec(dllexport) unsigned char __cdecl SetSndOutMode(unsigned char);
extern "C" __declspec(dllexport) unsigned short __cdecl SetAudioRate(unsigned short);

extern "C" __declspec(dllexport) void __cdecl AudioOut();
extern "C" __declspec(dllexport) void __cdecl CassIn();
extern "C" __declspec(dllexport) void __cdecl CassOut();
extern "C" __declspec(dllexport) void __cdecl CocoReset();
extern "C" __declspec(dllexport) void __cdecl SetClockSpeed(unsigned short);
extern "C" __declspec(dllexport) void __cdecl SetHorzInterruptState(unsigned char);
extern "C" __declspec(dllexport) void __cdecl SetInterruptTimer(unsigned short);
extern "C" __declspec(dllexport) void __cdecl SetLinesperScreen(unsigned char);
extern "C" __declspec(dllexport) void __cdecl SetMasterTickCounter();
extern "C" __declspec(dllexport) void __cdecl SetTimerClockRate(unsigned char);
extern "C" __declspec(dllexport) void __cdecl SetTimerInterruptState(unsigned char);
extern "C" __declspec(dllexport) void __cdecl SetVertInterruptState(unsigned char);
