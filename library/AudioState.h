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

  void* SndPointer1;
  void* SndPointer2;

  unsigned short AuxBuffer[6][44100 / 60]; //Biggest block size possible
} AudioState;
