#pragma once

#include <windows.h>

extern "C" __declspec(dllexport) void __cdecl ProcessCommandMessage(HWND, WPARAM);

extern "C" __declspec(dllexport) void __cdecl SaveLastTwoKeyDownEvents(unsigned char, unsigned char);
extern "C" __declspec(dllexport) void __cdecl SendSavedKeyEvents();
