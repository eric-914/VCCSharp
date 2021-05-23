#pragma once

#include "KeyboardState.h"

extern "C" __declspec(dllexport) void __cdecl vccKeyboardHandleKey(unsigned char, unsigned char, KeyStates);
