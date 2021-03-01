#pragma once

#include <windows.h>

typedef struct
{
  HANDLE TapeHandle;

  char CassPath[MAX_PATH];
  char FileType;
  char TapeFileName[MAX_PATH];

  int LastTrans;

  unsigned char Byte;
  unsigned char LastSample;
  unsigned char Mask;
  unsigned char MotorState;
  unsigned char One[21];
  unsigned char Quiet;
  unsigned char TapeMode;
  unsigned char TempBuffer[8192];
  unsigned char WriteProtect;
  unsigned char Zero[40];

  unsigned char* CasBuffer;

  unsigned int TempIndex;

  unsigned long BytesMoved;
  unsigned long TapeOffset;
  unsigned long TotalSize;

} CassetteState;
