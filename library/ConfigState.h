#pragma once

#include "SoundCardList.h"

#define MAXCARDS 12

typedef struct
{
  SoundCardList SoundCards[MAXCARDS];

  char IniFilePath[MAX_PATH];
} ConfigState;
