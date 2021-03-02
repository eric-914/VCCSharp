#pragma once

#include <windows.h>

#include "DirectDrawState.h"
#include "SystemState.h"

#define NO_WARN_MBCS_MFC_DEPRECATION


extern "C" __declspec(dllexport) DirectDrawState * __cdecl GetDirectDrawState();

extern "C" __declspec(dllexport) BOOL __cdecl InitInstance(HINSTANCE, HINSTANCE);

extern "C" __declspec(dllexport) bool __cdecl CreateDirectDrawWindow(SystemState*, WNDPROC);

extern "C" __declspec(dllexport) float __cdecl Static(SystemState*);

extern "C" __declspec(dllexport) unsigned char __cdecl LockScreen(SystemState*);
extern "C" __declspec(dllexport) unsigned char __cdecl SetAspect(unsigned char);
extern "C" __declspec(dllexport) unsigned char __cdecl SetInfoBand(unsigned char);

extern "C" __declspec(dllexport) void __cdecl CheckSurfaces();
extern "C" __declspec(dllexport) void __cdecl Cls(unsigned int, SystemState*);
extern "C" __declspec(dllexport) void __cdecl DisplayFlip(SystemState*);
extern "C" __declspec(dllexport) void __cdecl DoCls(SystemState*);
extern "C" __declspec(dllexport) void __cdecl FullScreenToggle(WNDPROC);
extern "C" __declspec(dllexport) void __cdecl SetStatusBarText(char*, SystemState*);
extern "C" __declspec(dllexport) void __cdecl UnlockScreen(SystemState*);
