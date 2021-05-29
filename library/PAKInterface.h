#pragma once

#include "PakInterfaceState.h"

extern "C" __declspec(dllexport) PakInterfaceState * __cdecl GetPakInterfaceState();

extern "C" __declspec(dllexport) unsigned char __cdecl PakMem8Read(unsigned short);
