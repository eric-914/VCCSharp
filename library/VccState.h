#pragma once

#include <windows.h>

#include "defines.h"

#include "EmuState.h"

typedef struct
{
  HANDLE hEventThread;
  HANDLE hEmuThread;  // Message handlers

  unsigned char AutoStart;
  unsigned char BinaryRunning;
  unsigned char DialogOpen;
  unsigned char FlagEmuStop;
  unsigned char Throttle;

  //--------------------------------------------------------------------------
  // When the main window is about to lose keyboard focus there are one
  // or two keys down in the emulation that must be raised.  These routines
  // track the last two key down events so they can be raised when needed.
  //--------------------------------------------------------------------------
  unsigned char SC_save1;
  unsigned char SC_save2;
  unsigned char KB_save1;
  unsigned char KB_save2;

  int KeySaveToggle;

  char CpuName[20];
  char AppName[MAX_LOADSTRING];

  MSG msg;
} VccState;
