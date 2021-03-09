#pragma once

#include <windows.h>

#include "SoundCardList.h"

typedef struct {
  HRESULT hr;

  DWORD SndLength1;
  DWORD SndLength2;
  DWORD SndBuffLength;
  DWORD WritePointer;
  DWORD BuffOffset;

  void* SndPointer1;
  void* SndPointer2;

  char AuxBufferPointer;
  int CardCount;
  unsigned short CurrentRate;
  unsigned char AudioPause;
  unsigned short BitRate;
  unsigned char InitPassed;
  unsigned short BlockSize;

  SoundCardList* Cards;

  unsigned short AuxBuffer[6][44100 / 60]; //Biggest block size possible

  char RateList[4][7];
  unsigned short iRateList[4];
} AudioState;
