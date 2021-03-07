#pragma once

#include <windows.h>

#include "defines.h"

#include "EmuState.h"

#define EMU_RUNSTATE_RUNNING	0
#define EMU_RUNSTATE_REQWAIT	1
#define EMU_RUNSTATE_WAITING	2

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
