#pragma once

#include "ConfigModel.h"

extern "C" __declspec(dllexport) void __cdecl SaveConfiguration(ConfigModel* model, char* iniFilePath);
extern "C" __declspec(dllexport) void __cdecl LoadConfiguration(ConfigModel* model, char* iniFilePath);
