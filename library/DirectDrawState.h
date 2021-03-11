#pragma once

#include <windows.h>

#include "defines.h"

typedef struct {
  unsigned char InfoBand;
  unsigned char ForceAspect;

  unsigned int StatusBarHeight;
  unsigned int Color;

  char TitleBarText[MAX_LOADSTRING];	// The title bar text
  char AppNameText[MAX_LOADSTRING];	// The title bar text
  char StatusText[255];

  HWND hWndStatusBar;
  HINSTANCE hInstance;
} DirectDrawState;
