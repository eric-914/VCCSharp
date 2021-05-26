#pragma once

#include <windows.h>

typedef struct
{
  char FileType;

  unsigned char Byte;
  unsigned char LastSample;
  unsigned char Mask;

  unsigned long BytesMoved;
  unsigned long TapeOffset;
  unsigned long TotalSize;

  HANDLE TapeHandle;

  int LastTrans;
  unsigned int TempIndex;

  unsigned char TempBuffer[8192];

  unsigned char* CasBuffer;
} CassetteState;
