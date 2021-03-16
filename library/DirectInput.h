#pragma once

#include "di.version.h"
#include <dinput.h>

BOOL HasJoystick(unsigned char stickNumber);
extern "C" __declspec(dllexport) HRESULT __cdecl JoystickPoll(DIJOYSTATE2* js, unsigned char stickNumber);