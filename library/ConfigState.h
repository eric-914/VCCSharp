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

  HICON CpuIcons[2];
  HICON MonIcons[2];
  HICON JoystickIcons[4];

  HWND hDlgBar;
  HWND hDlgTape;

  JoyStickModel Left;
  JoyStickModel Right;

  short int Cpuchoice[2];
  short int Monchoice[2];
  short int PaletteChoice[2];

  unsigned short int Ramchoice[4];
  unsigned int LeftJoystickEmulation[3];
  unsigned int RightJoystickEmulation[3];

  ConfigModel CurrentConfig;
  ConfigModel TempConfig;

  TCHAR AppDataPath[MAX_PATH];

  char TextMode;
  char PrtMon;
  unsigned char NumberofJoysticks;

  char IniFilePath[MAX_PATH];
  char TapeFileName[MAX_PATH];
  char ExecDirectory[MAX_PATH];
  char SerialCaptureFile[MAX_PATH];
  char OutBuffer[MAX_PATH];
  char AppName[MAX_LOADSTRING];

  unsigned int TapeCounter;
  unsigned char Tmode;
  char Tmodes[4][10];
  int NumberOfSoundCards;

  SoundCardList SoundCards[MAXCARDS];
  HWND hWndConfig[TABS];

  unsigned char TranslateDisp2Scan[SCAN_TRANS_COUNT];
  unsigned char TranslateScan2Disp[SCAN_TRANS_COUNT];
} ConfigState;
