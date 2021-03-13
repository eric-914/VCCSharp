#pragma once

using namespace std;

#include "ClipboardState.h"

extern "C" __declspec(dllexport) ClipboardState * __cdecl GetClipboardState();

extern "C" __declspec(dllexport) int __cdecl GetCurrentKeyMap();
