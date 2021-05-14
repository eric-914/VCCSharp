#pragma once

#include "CoCoState.h"

extern "C" __declspec(dllexport) CoCoState * __cdecl GetCoCoState();

extern "C" __declspec(dllexport) unsigned char __cdecl SetSndOutMode(unsigned char);
extern "C" __declspec(dllexport) unsigned short __cdecl SetAudioRate(unsigned short);

extern "C" __declspec(dllexport) void __cdecl AudioOut();
extern "C" __declspec(dllexport) void __cdecl SetLinesperScreen(unsigned char);
