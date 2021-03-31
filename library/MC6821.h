#pragma once

#include "MC6821State.h"

#define FALLING 0
#define RISING	1
#define ANY		  2

extern "C" __declspec(dllexport) MC6821State * __cdecl GetMC6821State();

extern "C" __declspec(dllexport) int __cdecl MC6821_OpenPrintFile(char*);

extern "C" __declspec(dllexport) unsigned char __cdecl MC6821_DACState();
extern "C" __declspec(dllexport) unsigned char __cdecl MC6821_GetCasSample();
extern "C" __declspec(dllexport) unsigned char __cdecl MC6821_GetMuxState();
extern "C" __declspec(dllexport) unsigned char __cdecl MC6821_pia0_read(unsigned char);
extern "C" __declspec(dllexport) unsigned char __cdecl MC6821_pia1_read(unsigned char);

extern "C" __declspec(dllexport) unsigned int __cdecl MC6821_GetDACSample();

extern "C" __declspec(dllexport) void __cdecl MC6821_CaptureBit(unsigned char);
extern "C" __declspec(dllexport) void __cdecl MC6821_SetCassetteSample(unsigned char);
extern "C" __declspec(dllexport) void __cdecl MC6821_pia0_write(unsigned char, unsigned char);
extern "C" __declspec(dllexport) void __cdecl MC6821_pia1_write(unsigned char, unsigned char);
extern "C" __declspec(dllexport) void __cdecl SetCart(unsigned char);
