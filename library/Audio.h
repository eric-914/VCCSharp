#pragma once

#include <windows.h>

#include "AudioState.h"

extern "C" __declspec(dllexport) AudioState * __cdecl GetAudioState();
