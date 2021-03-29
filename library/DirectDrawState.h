#pragma once

#include <windows.h>

#include "defines.h"

typedef struct {
  HWND hWndStatusBar;
  HINSTANCE hInstance;
  POINT WindowSize;

  unsigned char InfoBand;
  unsigned char ForceAspect;

  unsigned int StatusBarHeight;
  unsigned int Color;

  char AppNameText[MAX_LOADSTRING];	// The title bar text
  char TitleBarText[MAX_LOADSTRING];	// The title bar text
  char StatusText[255];
} DirectDrawState;
