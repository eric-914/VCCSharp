#pragma once

#include <windows.h>

#include "AudioState.h"

extern "C" __declspec(dllexport) AudioState * __cdecl GetAudioState();

extern "C" __declspec(dllexport) int __cdecl SoundInit(HWND, GUID*, unsigned short);
