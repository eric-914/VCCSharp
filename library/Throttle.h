#pragma once

#include <windows.h>

#include "ThrottleState.h"

extern "C" __declspec(dllexport) ThrottleState* __cdecl GetThrottleState();

extern "C" __declspec(dllexport) void __cdecl StartRender();
extern "C" __declspec(dllexport) void __cdecl EndRender(unsigned char);
