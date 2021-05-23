#pragma once

#include <windows.h>

#include "defines.h"

#include "EmuState.h"

typedef struct
{
  unsigned char AutoStart;
  unsigned char BinaryRunning;
  unsigned char DialogOpen;
  unsigned char RunState;  //An IRQ of sorts telling the emulator to pause during Full Screen toggle
  unsigned char Throttle;

  char CpuName[20];
  char AppName[MAX_LOADSTRING];

  MSG msg;
} VccState;

extern "C" __declspec(dllexport) VccState * __cdecl GetVccState();
