#pragma once

#include "TC1014MmuState.h"

extern "C" __declspec(dllexport) TC1014MmuState * __cdecl GetTC1014MmuState();

extern "C" __declspec(dllexport) unsigned short __cdecl GetMem(long);
extern "C" __declspec(dllexport) unsigned char* __cdecl GetInternalRomPointer(void);

extern "C" __declspec(dllexport) void __cdecl SetMmuEnabled(unsigned char);
extern "C" __declspec(dllexport) void __cdecl SetMmuPrefix(unsigned char);
extern "C" __declspec(dllexport) void __cdecl UpdateMmuArray(void);
extern "C" __declspec(dllexport) void __cdecl SetVectors(unsigned char);
extern "C" __declspec(dllexport) void __cdecl SetMmuRegister(unsigned char, unsigned char);
extern "C" __declspec(dllexport) void __cdecl SetMmuTask(unsigned char);
extern "C" __declspec(dllexport) void __cdecl SetRomMap(unsigned char);
extern "C" __declspec(dllexport) void __cdecl SetMapType(unsigned char);

extern "C" __declspec(dllexport) void __cdecl MmuReset(void);
extern "C" __declspec(dllexport) void __cdecl SetDistoRamBank(unsigned char);

extern "C" __declspec(dllexport) unsigned char __cdecl MemRead8(unsigned short);
extern "C" __declspec(dllexport) unsigned short __cdecl MemRead16(unsigned short);
extern "C" __declspec(dllexport) unsigned int __cdecl MemRead32(unsigned short);
extern "C" __declspec(dllexport) void __cdecl MemWrite8(unsigned char, unsigned short);
extern "C" __declspec(dllexport) void __cdecl MemWrite16(unsigned short, unsigned short);
extern "C" __declspec(dllexport) void __cdecl MemWrite32(unsigned int, unsigned short);

extern "C" __declspec(dllexport) void __cdecl GetExecPath(char* buffer);

extern "C" __declspec(dllexport) void __cdecl FreeMemory(unsigned char* target);
extern "C" __declspec(dllexport) unsigned char* __cdecl AllocateMemory(unsigned int size);
