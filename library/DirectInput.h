#pragma once

#include "di.version.h"
#include <dinput.h>

#include <windows.h>

extern "C" __declspec(dllexport) HRESULT __cdecl JoyStickPoll(DIJOYSTATE2* js, unsigned char stickNumber);