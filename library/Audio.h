#pragma once

#define MAXCARDS	12

#include <windows.h>

#include "AudioState.h"

extern "C" __declspec(dllexport) AudioState * __cdecl GetAudioState();

extern "C" __declspec(dllexport) const char* __cdecl GetRateList(unsigned char);

extern "C" __declspec(dllexport) int __cdecl GetFreeBlockCount();
extern "C" __declspec(dllexport) int __cdecl GetSoundCardList(SoundCardList*);
extern "C" __declspec(dllexport) int __cdecl SoundInit(HWND, _GUID*, unsigned short);

extern "C" __declspec(dllexport) unsigned char __cdecl PauseAudio(unsigned char);
extern "C" __declspec(dllexport) unsigned short __cdecl GetSoundStatus();

extern "C" __declspec(dllexport) void __cdecl FlushAudioBuffer(unsigned int*, unsigned short);
extern "C" __declspec(dllexport) void __cdecl PurgeAuxBuffer();
