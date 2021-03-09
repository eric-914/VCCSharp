#pragma once

#include <windows.h>

#include "SoundCardList.h"

typedef struct {
  unsigned char InitPassed;
  unsigned char AudioPause;
  char AuxBufferPointer;

  unsigned short CurrentRate;
  unsigned short BitRate;
  unsigned short BlockSize;

  unsigned short iRateList[4];

  int CardCount;

  DWORD SndLength1;
  DWORD SndLength2;
  DWORD SndBuffLength;
  DWORD WritePointer;
  DWORD BuffOffset;

  HRESULT hr;

  SoundCardList* Cards;

  unsigned short AuxBuffer[6][44100 / 60]; //Biggest block size possible

  char RateList[4][7];

  void* SndPointer1;
  void* SndPointer2;
} AudioState;
