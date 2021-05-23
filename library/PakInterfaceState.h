#pragma once

#include <windows.h>
#include <stdint.h>

typedef struct {
  HINSTANCE hInstLib;

  unsigned char* ExternalRomBuffer;
  BOOL RomPackLoaded;

  unsigned int BankedCartOffset;
  char DllPath[256];
  unsigned short ModualParms;
  BOOL DialogOpen;

  char Modname[MAX_PATH];
} PakInterfaceState;
