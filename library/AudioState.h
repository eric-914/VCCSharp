#pragma once

#include <windows.h>

#define MAXCARDS	12

typedef struct {
  char AuxBufferPointer;

  DWORD SndLength1;
  DWORD SndLength2;
  DWORD SndBuffLength;
  DWORD BuffOffset;

  void* SndPointer1;
  void* SndPointer2;
} AudioState;
