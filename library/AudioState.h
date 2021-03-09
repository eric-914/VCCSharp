#pragma once

#include <windows.h>

#include "SoundCardList.h"

typedef struct {
  HRESULT hr;

  unsigned char InitPassed;
  unsigned char AudioPause;
  char AuxBufferPointer;

  unsigned short CurrentRate;
  unsigned short BitRate;
  unsigned short BlockSize;

  int CardCount;

  DWORD SndLength1;
  DWORD SndLength2;
  DWORD SndBuffLength;
  DWORD WritePointer;
  DWORD BuffOffset;

  SoundCardList* Cards;

  unsigned short AuxBuffer[6][44100 / 60]; //Biggest block size possible

  char RateList[4][7];
  unsigned short iRateList[4];

  void* SndPointer1;
  void* SndPointer2;
} AudioState;
