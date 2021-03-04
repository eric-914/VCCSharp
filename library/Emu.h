#pragma once

#include "EmuState.h"

extern "C" __declspec(dllexport) EmuState* __cdecl GetEmuState();
extern "C" __declspec(dllexport) void __cdecl SetEmuState(EmuState*);
