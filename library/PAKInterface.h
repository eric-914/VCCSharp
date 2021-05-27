#pragma once

#include "PakInterfaceState.h"

extern "C" __declspec(dllexport) PakInterfaceState * __cdecl GetPakInterfaceState();

extern "C" __declspec(dllexport) int __cdecl InsertModule(unsigned char, char*);

extern "C" __declspec(dllexport) unsigned char __cdecl PakMem8Read(unsigned short);

extern "C" __declspec(dllexport) int __cdecl UnloadPack(unsigned char);

extern "C" __declspec(dllexport) void __cdecl SetCart(unsigned char);
