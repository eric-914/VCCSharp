#pragma once

#include <windows.h>

#include "di.version.h"
#include <ddraw.h>

#include "DirectDrawState.h"
#include "EmuState.h"

#define NO_WARN_MBCS_MFC_DEPRECATION

extern "C" __declspec(dllexport) DirectDrawState * __cdecl GetDirectDrawState();

extern "C" __declspec(dllexport) BOOL __cdecl InitDirectDraw(HINSTANCE, HINSTANCE);

extern "C" __declspec(dllexport) BOOL __cdecl CreateDirectDrawWindow(EmuState*);

extern "C" __declspec(dllexport) void __cdecl CheckSurfaces();
extern "C" __declspec(dllexport) void __cdecl SetStatusBarText(char*);
