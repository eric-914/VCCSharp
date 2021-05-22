#pragma once

#include "ConfigState.h"

extern "C" __declspec(dllexport) ConfigState* __cdecl GetConfigState();

extern "C" __declspec(dllexport) POINT __cdecl GetIniWindowSize();

extern "C" __declspec(dllexport) int __cdecl GetRememberSize();

extern "C" __declspec(dllexport) void __cdecl SetIniFilePath(char*);
