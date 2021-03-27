#pragma once

#include "DirectDrawInternalState.h"

extern "C" __declspec(dllexport) DirectDrawInternalState* __cdecl GetDirectDrawInternalState();

extern "C" __declspec(dllexport) HRESULT __cdecl UnlockSurface();