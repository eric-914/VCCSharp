#pragma once

#include "VccState.h"
#include "EmuState.h"

extern "C" __declspec(dllexport) VccState * __cdecl GetVccState();

extern "C" __declspec(dllexport) void __cdecl SaveLastTwoKeyDownEvents(unsigned char, unsigned char);
extern "C" __declspec(dllexport) void __cdecl SendSavedKeyEvents();
extern "C" __declspec(dllexport) void __cdecl SetCpuType(unsigned char);
