#pragma once

#include <windows.h>

#include "defines.h"

#include "ConfigModel.h"
#include "SoundCardList.h"

#define TABS 7
#define MAXCARDS 12

typedef struct
{
  char TextMode;  //--Add LF to CR
  char PrintMonitorWindow;

  unsigned char NumberOfJoysticks;

  unsigned short TapeCounter;
  unsigned char TapeMode;

  short NumberOfSoundCards;

  ConfigModel* Model;
  SoundCardList SoundCards[MAXCARDS];

  char AppDataPath[MAX_PATH];
  char IniFilePath[MAX_PATH];
  char TapeFileName[MAX_PATH];
  char ExecDirectory[MAX_PATH];
  char SerialCaptureFile[MAX_PATH];
  char OutBuffer[MAX_PATH];

  HWND hDlgBar;
  HWND hDlgTape;

  HWND hWndConfig[TABS];
} ConfigState;
