#pragma once

#include <windows.h>

#include "defines.h"

#include "SoundCardList.h"

#define MAXCARDS 12

typedef struct
{
  SoundCardList SoundCards[MAXCARDS];

  char AppDataPath[MAX_PATH];
  char IniFilePath[MAX_PATH];
  char ExecDirectory[MAX_PATH];
} ConfigState;
