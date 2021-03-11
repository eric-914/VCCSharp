#pragma once

#include "PakInterfaceDelegates.h"

extern "C" __declspec(dllexport) PakInterfaceDelegates* __cdecl GetPakInterfaceDelegates();

extern "C" __declspec(dllexport) BOOL __cdecl HasConfigModule();
extern "C" __declspec(dllexport) BOOL __cdecl HasDmaMemPointer();
extern "C" __declspec(dllexport) BOOL __cdecl HasHeartBeat();
extern "C" __declspec(dllexport) BOOL __cdecl HasModuleAudioSample();
extern "C" __declspec(dllexport) BOOL __cdecl HasModuleMem8Read();
extern "C" __declspec(dllexport) BOOL __cdecl HasModulePortRead();
extern "C" __declspec(dllexport) BOOL __cdecl HasModuleReset();
extern "C" __declspec(dllexport) BOOL __cdecl HasModuleStatus();
extern "C" __declspec(dllexport) BOOL __cdecl HasPakMemRead8();
extern "C" __declspec(dllexport) BOOL __cdecl HasPakMemWrite8();
extern "C" __declspec(dllexport) BOOL __cdecl HasPakPortRead();
extern "C" __declspec(dllexport) BOOL __cdecl HasPakPortWrite();
extern "C" __declspec(dllexport) BOOL __cdecl HasPakSetCart();
extern "C" __declspec(dllexport) BOOL __cdecl HasSetIniPath();
extern "C" __declspec(dllexport) BOOL __cdecl HasSetInterruptCallPointer();

extern "C" __declspec(dllexport) void __cdecl InvokeConfigModule(unsigned char menuItem);
extern "C" __declspec(dllexport) void __cdecl InvokeDmaMemPointer();
extern "C" __declspec(dllexport) void __cdecl InvokeGetModuleName(char* modName, char* catNumber);
extern "C" __declspec(dllexport) void __cdecl InvokeHeartBeat();
extern "C" __declspec(dllexport) void __cdecl InvokeModuleReset();
extern "C" __declspec(dllexport) void __cdecl InvokeModuleStatus(char* statusLine);
extern "C" __declspec(dllexport) void __cdecl InvokePakSetCart();
extern "C" __declspec(dllexport) void __cdecl InvokeSetIniPath(char* ini);
extern "C" __declspec(dllexport) void __cdecl InvokeSetInterruptCallPointer();

extern "C" __declspec(dllexport) BOOL __cdecl ModulePortWrite(unsigned char port, unsigned char data);
extern "C" __declspec(dllexport) BOOL __cdecl SetDelegates(HINSTANCE hInstLib);

extern "C" __declspec(dllexport) unsigned char __cdecl ModuleMem8Read(unsigned short address);
extern "C" __declspec(dllexport) unsigned char __cdecl ModulePortRead(unsigned char port);
extern "C" __declspec(dllexport) unsigned short __cdecl ReadModuleAudioSample();

extern "C" __declspec(dllexport) void __cdecl UnloadModule();
