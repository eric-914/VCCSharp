#pragma once

#include <windows.h>

#include "defines.h"

#include "ConfigModel.h"
#include "SoundCardList.h"

#define SCAN_TRANS_COUNT	84
#define TABS 8
#define MAXCARDS 12

typedef struct
{
  HWND hWndConfig[TABS];
  HWND hDlgBar;
  HWND hDlgTape;

  ConfigModel* Model;

  char TextMode;  //--Add LF to CR
  char PrintMonitorWindow;

  unsigned char NumberOfJoysticks;

  char IniFilePath[MAX_PATH];
  char TapeFileName[MAX_PATH];
  char ExecDirectory[MAX_PATH];
  char SerialCaptureFile[MAX_PATH];
  char OutBuffer[MAX_PATH];

  unsigned int TapeCounter;
  unsigned char TapeMode;

  int NumberOfSoundCards;
  SoundCardList SoundCards[MAXCARDS];
} ConfigState;
