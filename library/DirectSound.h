#pragma once

#include "DirectSoundState.h"

extern "C" __declspec(dllexport) DirectSoundState* __cdecl GetDirectSoundState();

extern "C" __declspec(dllexport) void __cdecl DirectSoundStopAndRelease();
extern "C" __declspec(dllexport) void __cdecl DirectSoundSetCurrentPosition(DWORD position);

extern "C" __declspec(dllexport) HRESULT __cdecl DirectSoundLock(DWORD buffOffset, unsigned short length, void** sndPointer1, DWORD* sndLength1, void** sndPointer2, DWORD* sndLength2);
extern "C" __declspec(dllexport) HRESULT __cdecl DirectSoundUnlock(void* sndPointer1, DWORD sndLength1, void* sndPointer2, DWORD sndLength2);

extern "C" __declspec(dllexport) void __cdecl EnumerateSoundCards();

extern "C" __declspec(dllexport) void __cdecl DirectSoundStop();
extern "C" __declspec(dllexport) BOOL __cdecl DirectSoundHasBuffer();
extern "C" __declspec(dllexport) HRESULT __cdecl DirectSoundBufferRelease();
extern "C" __declspec(dllexport) BOOL __cdecl DirectSoundHasInterface();
extern "C" __declspec(dllexport) HRESULT __cdecl DirectSoundInterfaceRelease();
extern "C" __declspec(dllexport) HRESULT __cdecl DirectSoundInitialize(GUID* guid);
extern "C" __declspec(dllexport) HRESULT __cdecl DirectSoundSetCooperativeLevel(HWND hWnd);
