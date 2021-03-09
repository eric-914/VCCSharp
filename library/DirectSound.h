#pragma once

#include "DirectSoundState.h"

extern "C" __declspec(dllexport) DirectSoundState* __cdecl GetDirectSoundState();

extern "C" __declspec(dllexport) void __cdecl StopAndRelease();
extern "C" __declspec(dllexport) void __cdecl SetCurrentPosition(DWORD position);
