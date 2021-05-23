#pragma once

extern "C" __declspec(dllexport) unsigned char __cdecl MC6821_GetCasSample();
extern "C" __declspec(dllexport) unsigned int __cdecl MC6821_GetDACSample();

extern "C" __declspec(dllexport) void __cdecl MC6821_SetCassetteSample(unsigned char);
extern "C" __declspec(dllexport) void __cdecl SetCart(unsigned char);
