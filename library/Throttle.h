#pragma once

#include <windows.h>

#include "ThrottleState.h"

extern "C" __declspec(dllexport) ThrottleState* __cdecl GetThrottleState();

extern "C" __declspec(dllexport) float __cdecl CalculateFPS();

extern "C" __declspec(dllexport) void __cdecl CalibrateThrottle();
extern "C" __declspec(dllexport) void __cdecl EndRender(unsigned char);
extern "C" __declspec(dllexport) void __cdecl FrameWait();
extern "C" __declspec(dllexport) void __cdecl StartRender();
