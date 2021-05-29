#pragma once

#include <windows.h>

extern "C" __declspec(dllexport) BOOL __cdecl HasModuleMem8Read();

extern "C" __declspec(dllexport) unsigned char __cdecl ModuleMem8Read(unsigned short address);
