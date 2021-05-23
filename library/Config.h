#pragma once

#include "ConfigState.h"

extern "C" __declspec(dllexport) ConfigState* __cdecl GetConfigState();
extern "C" __declspec(dllexport) ConfigModel* __cdecl GetConfigModel();