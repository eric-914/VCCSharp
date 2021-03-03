#pragma once

#include <windows.h>
#include <richedit.h>

#include "defines.h"

#include "ConfigModel.h"
#include "JoystickModel.h"
#include "SoundCardList.h"

#define SCAN_TRANS_COUNT	84
#define TABS 8
#define MAXCARDS 12

typedef struct
{
  CHARFORMAT CounterText;
  CHARFORMAT ModeText;

  HWND hDlgBar;
  HWND hDlgTape;

  ConfigModel Model;

  char TextMode;
  char PrtMon;
  unsigned char NumberOfJoysticks;

  char IniFilePath[MAX_PATH];
  char TapeFileName[MAX_PATH];
  char ExecDirectory[MAX_PATH];
  char SerialCaptureFile[MAX_PATH];
  char OutBuffer[MAX_PATH];
  char AppName[MAX_LOADSTRING];

  unsigned int TapeCounter;
  unsigned char TapeMode;
  int NumberOfSoundCards;

  SoundCardList SoundCards[MAXCARDS];
  HWND hWndConfig[TABS];
} ConfigState;
