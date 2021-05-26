#pragma once

#include <windows.h>

#include "defines.h"

#include "EmuState.h"

typedef struct
{
  unsigned char BinaryRunning;
  unsigned char DialogOpen;

  MSG msg;
} VccState;

extern "C" __declspec(dllexport) VccState * __cdecl GetVccState();
