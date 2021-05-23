#pragma once

#include <windows.h>

#define MAXCARDS	12

typedef struct {
  char AuxBufferPointer;

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
} AudioState;
