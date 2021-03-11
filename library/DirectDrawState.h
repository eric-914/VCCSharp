#pragma once

#include <windows.h>

#include "defines.h"

typedef struct {
  HWND hWndStatusBar;
  TCHAR TitleBarText[MAX_LOADSTRING];	// The title bar text
  TCHAR AppNameText[MAX_LOADSTRING];	// The title bar text

  HINSTANCE hInstance;

  unsigned int StatusBarHeight;
  unsigned char InfoBand;
  unsigned char ForceAspect;
  unsigned int Color;

  char StatusText[255];
} DirectDrawState;

