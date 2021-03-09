#pragma once

#include <windows.h>
#include <stdint.h>

#include "Dmenu.h"
#include "PakInterfaceDelegates.h"

typedef struct {
  HINSTANCE hInstLib;

  // Storage for Pak ROMs
  uint8_t* ExternalRomBuffer;
  bool RomPackLoaded;

  unsigned int BankedCartOffset;
  char DllPath[256];
  unsigned short ModualParms;
  bool DialogOpen;

  char Modname[MAX_PATH];
} PakInterfaceState;

typedef struct {
  void (*GetModuleName)(char*, char*, DYNAMICMENUCALLBACK);
  void (*ConfigModule)(unsigned char);
  void (*SetInterruptCallPointer) (ASSERTINTERRUPT);
  void (*DmaMemPointer) (MEMREAD8, MEMWRITE8);
  void (*HeartBeat)(void);
  void (*PakPortWrite)(unsigned char, unsigned char);
  unsigned char (*PakPortRead)(unsigned char);
  void (*PakMemWrite8)(unsigned char, unsigned short);
  unsigned char (*PakMemRead8)(unsigned short);
  void (*ModuleStatus)(char*);
  unsigned short (*ModuleAudioSample)(void);
  void (*ModuleReset) (void);
  void (*SetIniPath) (char*);
  void (*PakSetCart)(SETCART);
} PakInterfaceDelegates;