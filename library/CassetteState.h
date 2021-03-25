#pragma once

#include <windows.h>

typedef struct
{
  char FileType;

  unsigned char Byte;
  unsigned char LastSample;
  unsigned char Mask;
  unsigned char MotorState;
  unsigned char Quiet;
  unsigned char TapeMode;
  unsigned char WriteProtect;

  unsigned long BytesMoved;
  unsigned long TapeOffset;
  unsigned long TotalSize;

  int LastTrans;
  unsigned int TempIndex;

  HANDLE TapeHandle;

  char CassPath[MAX_PATH];
  char TapeFileName[MAX_PATH];

  unsigned char One[21];
  unsigned char Zero[40];
  unsigned char TempBuffer[8192];

  unsigned char* CasBuffer;
} CassetteState;
