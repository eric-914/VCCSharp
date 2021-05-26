#pragma once

#include "CoCoState.h"

extern "C" __declspec(dllexport) CoCoState * __cdecl GetCoCoState();

extern "C" __declspec(dllexport) void __cdecl AudioOut();
