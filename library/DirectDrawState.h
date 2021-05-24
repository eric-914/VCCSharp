#pragma once

#include <windows.h>

#include "defines.h"

typedef struct {
  char AppNameText[MAX_LOADSTRING];
  char TitleBarText[MAX_LOADSTRING];
  char StatusText[255];
} DirectDrawState;
