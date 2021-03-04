#pragma once

#include <windows.h>

#include "defines.h"

#include "EmuState.h"

typedef struct
{
  HANDLE hEventThread;
  HANDLE hEmuThread;  // Message handlers
  MSG  msg;

  char CpuName[20];
  char AppName[MAX_LOADSTRING];
  unsigned char FlagEmuStop;

  char QuickLoadFile[256];
  bool BinaryRunning;
  bool DialogOpen;
  unsigned char Throttle;
  unsigned char AutoStart;
  unsigned char Qflag;

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
} VccState;
