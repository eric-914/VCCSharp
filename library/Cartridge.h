#pragma once

extern "C" __declspec(dllexport) int __cdecl OpenLoadCartFileDialog(EmuState*);
extern "C" __declspec(dllexport) unsigned __stdcall CartLoad(void*);
extern "C" __declspec(dllexport) void __cdecl LoadPack();
