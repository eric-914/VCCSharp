#pragma once

#include <windows.h>

typedef struct
{
  unsigned char DialogOpen;

  MSG msg;
} VccState;

extern "C" __declspec(dllexport) VccState * __cdecl GetVccState();
