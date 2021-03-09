#pragma once

#include <windows.h>
#include <stdint.h>

#include "Dmenu.h"

typedef struct {
  HINSTANCE hInstLib;

  // Storage for Pak ROMs
  unsigned char* ExternalRomBuffer;
  BOOL RomPackLoaded;

  unsigned int BankedCartOffset;
  char DllPath[256];
  unsigned short ModualParms;
  BOOL DialogOpen;

  char Modname[MAX_PATH];
} PakInterfaceState;
