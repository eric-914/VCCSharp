#pragma once

#include "PakInterfaceState.h"
#include "PakInterfaceDelegates.h"
#include "EmuState.h"

#define NOMODULE	1
#define NOTVCC	2

#define HASCONFIG		     1
#define HASIOWRITE		   2
#define HASIOREAD		     4
#define NEEDSCPUIRQ		   8
#define DOESDMA			    16
#define NEEDHEARTBEAT	  32
#define ANALOGAUDIO		  64
#define CSWRITE			   128
#define CSREAD			   256
#define RETURNSSTATUS	 512
#define CARTRESET		  1024
#define SAVESINI		  2048
#define ASSERTCART		4096

extern "C" __declspec(dllexport) PakInterfaceState * __cdecl GetPakInterfaceState();

extern "C" __declspec(dllexport) int __cdecl InsertModule(EmuState*, char*);

extern "C" __declspec(dllexport) unsigned char __cdecl PakMem8Read(unsigned short);

extern "C" __declspec(dllexport) unsigned short __cdecl PakAudioSample(void);

extern "C" __declspec(dllexport) void __cdecl UnloadPack(EmuState*);
