#pragma once

#include "TC1014MmuState.h"

extern "C" __declspec(dllexport) TC1014MmuState * __cdecl GetTC1014MmuState();

extern "C" __declspec(dllexport) unsigned char __cdecl MemRead8(unsigned short);
extern "C" __declspec(dllexport) void __cdecl MemWrite8(unsigned char, unsigned short);
