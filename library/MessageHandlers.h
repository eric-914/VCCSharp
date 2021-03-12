#pragma once

#include <windows.h>

extern "C" __declspec(dllexport) void __cdecl CreateMainMenu(HWND);
extern "C" __declspec(dllexport) void __cdecl KeyDown(WPARAM, LPARAM);
extern "C" __declspec(dllexport) void __cdecl KeyUp(WPARAM, LPARAM);
extern "C" __declspec(dllexport) void __cdecl MouseMove(LPARAM);

extern "C" __declspec(dllexport) HWND __cdecl CreateConfigurationDialog(HINSTANCE resources, HWND windowHandle);
