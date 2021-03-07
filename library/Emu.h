#pragma once

#include "EmuState.h"

extern "C" __declspec(dllexport) EmuState* __cdecl GetEmuState();
extern "C" __declspec(dllexport) void __cdecl SetEmuState(EmuState*);

extern "C" __declspec(dllexport) void __cdecl SetTurboMode(unsigned char);
extern "C" __declspec(dllexport) void __cdecl SetCPUMultiplierFlag(unsigned char);

extern "C" __declspec(dllexport) unsigned char __cdecl SetFrameSkip(unsigned char);
extern "C" __declspec(dllexport) unsigned char __cdecl SetRamSize(unsigned char);
extern "C" __declspec(dllexport) unsigned char __cdecl SetCPUMultiplier(unsigned char);

extern "C" __declspec(dllexport) void __cdecl SetCPUToHD6309();
extern "C" __declspec(dllexport) void __cdecl SetCPUToMC6809();