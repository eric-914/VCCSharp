#pragma once

#include <windows.h>

#include "AudioState.h"

extern "C" __declspec(dllexport) AudioState * __cdecl GetAudioState();

extern "C" __declspec(dllexport) const char* __cdecl GetRateList(unsigned char);

extern "C" __declspec(dllexport) int __cdecl GetFreeBlockCount();
extern "C" __declspec(dllexport) int __cdecl SoundInit(HWND, GUID*, unsigned short);

extern "C" __declspec(dllexport) unsigned char __cdecl PauseAudio(unsigned char);
extern "C" __declspec(dllexport) unsigned short __cdecl GetSoundStatus();

extern "C" __declspec(dllexport) void __cdecl PurgeAuxBuffer();
