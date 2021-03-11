#pragma once

#include <windows.h>

extern "C" __declspec(dllexport) void __cdecl CreateMainMenu(HWND);
extern "C" __declspec(dllexport) void __cdecl EmuReset(unsigned char);
extern "C" __declspec(dllexport) void __cdecl KeyDown(WPARAM, LPARAM);
extern "C" __declspec(dllexport) void __cdecl KeyUp(WPARAM, LPARAM);
extern "C" __declspec(dllexport) void __cdecl MouseMove(LPARAM);
extern "C" __declspec(dllexport) void __cdecl ShowConfiguration();

extern "C" __declspec(dllexport) void __cdecl ToggleMonitorType();
extern "C" __declspec(dllexport) void __cdecl ToggleInfoBand();
