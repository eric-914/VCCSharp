#pragma once

#include "di.version.h"
#include <dinput.h>

#include "JoystickState.h"

#define MAXSTICKS 10
#define STRLEN 64

extern "C" __declspec(dllexport) JoystickState * __cdecl GetJoystickState();

extern "C" __declspec(dllexport) HRESULT __cdecl JoyStickPoll(DIJOYSTATE2*, unsigned char);

extern "C" __declspec(dllexport) bool __cdecl InitJoyStick(unsigned char);

extern "C" __declspec(dllexport) char __cdecl SetMouseStatus(char, unsigned char);

extern "C" __declspec(dllexport) char* __cdecl GetStickName(int);

extern "C" __declspec(dllexport) int __cdecl EnumerateJoysticks();

extern "C" __declspec(dllexport) unsigned short __cdecl get_pot_value(unsigned char);

extern "C" __declspec(dllexport) void __cdecl SetButtonStatus(unsigned char, unsigned char);
extern "C" __declspec(dllexport) void __cdecl SetJoystick(unsigned short, unsigned short);
extern "C" __declspec(dllexport) void __cdecl SetStickNumbers(unsigned char, unsigned char);
