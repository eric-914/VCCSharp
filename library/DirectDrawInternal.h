#pragma once

#include "DirectDrawInternalState.h"

extern "C" __declspec(dllexport) DirectDrawInternalState* __cdecl GetDirectDrawInternalState();

extern "C" __declspec(dllexport) HRESULT __cdecl UnlockSurface();
extern "C" __declspec(dllexport) void __cdecl GetSurfaceDC(HDC* hdc);
extern "C" __declspec(dllexport) void __cdecl ReleaseSurfaceDC(HDC hdc);
