#pragma once

#include <windows.h>

typedef struct {
  HINSTANCE hInstLib;

  unsigned char CartInserted;

  unsigned char* ExternalRomBuffer;
  BOOL RomPackLoaded;

  unsigned int BankedCartOffset;
  BOOL DialogOpen;

  char Modname[MAX_PATH];
} PakInterfaceState;
