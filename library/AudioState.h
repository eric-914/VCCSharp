#pragma once

#include <windows.h>

#include "SoundCardList.h"

#define MAXCARDS	12

typedef struct {
  unsigned char InitPassed;
  unsigned char AudioPause;
  char AuxBufferPointer;

  unsigned short CurrentRate;
  unsigned short BitRate;
  unsigned short BlockSize;

  DWORD SndLength1;
  DWORD SndLength2;
  DWORD SndBuffLength;
  DWORD WritePointer;
  DWORD BuffOffset;

  unsigned short iRateList[4];

  HRESULT hr;

  void* SndPointer1;
  void* SndPointer2;

  unsigned short AuxBuffer[6][44100 / 60]; //Biggest block size possible

  char RateList[4][7];

} AudioState;
