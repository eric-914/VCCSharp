#pragma once

#include "DirectSoundState.h"

extern "C" __declspec(dllexport) DirectSoundState* __cdecl GetDirectSoundState();

extern "C" __declspec(dllexport) BOOL __cdecl DirectSoundHasBuffer();
extern "C" __declspec(dllexport) BOOL __cdecl DirectSoundHasInterface();

extern "C" __declspec(dllexport) HRESULT __cdecl DirectSoundBufferRelease();
extern "C" __declspec(dllexport) HRESULT __cdecl DirectSoundCreateSoundBuffer();
extern "C" __declspec(dllexport) HRESULT __cdecl DirectSoundInitialize(GUID* guid);
extern "C" __declspec(dllexport) HRESULT __cdecl DirectSoundInterfaceRelease();
extern "C" __declspec(dllexport) HRESULT __cdecl DirectSoundLock(DWORD buffOffset, unsigned short length, void** sndPointer1, DWORD* sndLength1, void** sndPointer2, DWORD* sndLength2);
extern "C" __declspec(dllexport) HRESULT __cdecl DirectSoundSetCooperativeLevel(HWND hWnd);
extern "C" __declspec(dllexport) HRESULT __cdecl DirectSoundUnlock(void* sndPointer1, DWORD sndLength1, void* sndPointer2, DWORD sndLength2);

extern "C" __declspec(dllexport) void __cdecl DirectSoundEnumerateSoundCards();
extern "C" __declspec(dllexport) void __cdecl DirectSoundSetCurrentPosition(DWORD position);
extern "C" __declspec(dllexport) void __cdecl DirectSoundSetupFormatDataStructure(unsigned short bitRate);
extern "C" __declspec(dllexport) void __cdecl DirectSoundSetupSecondaryBuffer(DWORD sndBuffLength);
extern "C" __declspec(dllexport) void __cdecl DirectSoundStopAndRelease();

extern "C" __declspec(dllexport) HRESULT __cdecl DirectSoundPlay();
extern "C" __declspec(dllexport) HRESULT __cdecl DirectSoundStop();
extern "C" __declspec(dllexport) long __cdecl DirectSoundGetCurrentPosition(unsigned long* playCursor, unsigned long* writeCursor);