#pragma once

#include <windows.h>

#include "DirectDrawState.h"
#include "EmuState.h"

#define NO_WARN_MBCS_MFC_DEPRECATION


extern "C" __declspec(dllexport) DirectDrawState * __cdecl GetDirectDrawState();

extern "C" __declspec(dllexport) BOOL __cdecl InitDirectDraw(HINSTANCE, HINSTANCE);

extern "C" __declspec(dllexport) bool __cdecl CreateDirectDrawWindow(EmuState*, WNDPROC);

extern "C" __declspec(dllexport) float __cdecl Static(EmuState*);

extern "C" __declspec(dllexport) unsigned char __cdecl LockScreen(EmuState*);
extern "C" __declspec(dllexport) unsigned char __cdecl SetAspect(unsigned char);
extern "C" __declspec(dllexport) unsigned char __cdecl SetInfoBand(unsigned char);

extern "C" __declspec(dllexport) void __cdecl CheckSurfaces();
extern "C" __declspec(dllexport) void __cdecl ClearScreen();
extern "C" __declspec(dllexport) void __cdecl DisplayFlip(EmuState*);
extern "C" __declspec(dllexport) void __cdecl DoCls(EmuState*);
extern "C" __declspec(dllexport) void __cdecl FullScreenToggle(WNDPROC);
extern "C" __declspec(dllexport) void __cdecl SetStatusBarText(char*, EmuState*);
extern "C" __declspec(dllexport) void __cdecl UnlockScreen(EmuState*);
