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

  char CassPath[MAX_PATH];
  char TapeFileName[MAX_PATH];

  unsigned char One[21];
  unsigned char Zero[40];
  unsigned char TempBuffer[8192];

  unsigned char* CasBuffer;
} CassetteState;
