#pragma once

#include "ConfigState.h"
#include "ConfigModel.h"

extern "C" __declspec(dllexport) ConfigState* __cdecl GetConfigState();
extern "C" __declspec(dllexport) ConfigModel* __cdecl GetConfigModel();