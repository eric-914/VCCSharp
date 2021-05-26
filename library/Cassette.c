#include <string>

#include "CassetteState.h"

#include "defines.h"
#include "macros.h"

#include "fileoperations.h"

using namespace std;

const unsigned char One[21] = { 0x80, 0xA8, 0xC8, 0xE8, 0xE8, 0xF8, 0xF8, 0xE8, 0xC8, 0xA8, 0x78, 0x50, 0x50, 0x30, 0x10, 0x00, 0x00, 0x10, 0x30, 0x30, 0x50 };
const unsigned char Zero[40] = { 0x80, 0x90, 0xA8, 0xB8, 0xC8, 0xD8, 0xE8, 0xE8, 0xF0, 0xF8, 0xF8, 0xF8, 0xF0, 0xE8, 0xD8, 0xC8, 0xB8, 0xA8, 0x90, 0x78, 0x78, 0x68, 0x50, 0x40, 0x30, 0x20, 0x10, 0x08, 0x00, 0x00, 0x00, 0x08, 0x10, 0x10, 0x20, 0x30, 0x40, 0x50, 0x68, 0x68 };

#define WRITEBUFFERSIZE	0x1FFFF
#define CAS	1
#define WAV 0

#define STOP	0

CassetteState* InitializeInstance(CassetteState*);

static CassetteState* instance = InitializeInstance(new CassetteState());

extern "C" {
  __declspec(dllexport) CassetteState* __cdecl GetCassetteState() {
    return instance;
  }
}

CassetteState* InitializeInstance(CassetteState* p) {
  p->Byte = 0;
  p->BytesMoved = 0;
  p->FileType = 0;
  p->LastSample = 0;
  p->LastTrans = 0;
  p->Mask = 0;
  p->TapeOffset = 0;
  p->TempIndex = 0;
  p->TotalSize = 0;

  p->CasBuffer = NULL;
  p->TapeHandle = NULL;

  strcpy(p->TapeFileName, "");

  ARRAYCOPY(Zero);
  ARRAYCOPY(One);

  return p;
}

extern "C" {
  __declspec(dllexport) void __cdecl ResetCassetteBuffer()
  {
    if (instance->CasBuffer != NULL) {
      free(instance->CasBuffer);
    }

    instance->CasBuffer = (unsigned char*)malloc(WRITEBUFFERSIZE);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl FlushCassetteWAV(unsigned char* buffer, unsigned int length)
  {
    SetFilePointer(instance->TapeHandle, instance->TapeOffset + 44, 0, FILE_BEGIN);
    WriteFile(instance->TapeHandle, buffer, length, &(instance->BytesMoved), NULL);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SyncFileBufferCAS()
  {
    unsigned long bytesMoved = 0;

    instance->CasBuffer[instance->TapeOffset] = instance->Byte;	//capture the last byte
    instance->LastTrans = 0;	//reset all static inter-call variables
    instance->Mask = 0;
    instance->Byte = 0;
    instance->LastSample = 0;
    instance->TempIndex = 0;

    WriteFile(instance->TapeHandle, instance->CasBuffer, instance->TapeOffset, &bytesMoved, NULL);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SyncFileBufferWAV()
  {
    char buffer[64] = "";
    unsigned long bytesMoved = 0;
    
    unsigned int formatSize = 16;		//size of WAVE section chunk
    unsigned short waveType = 1;		//WAVE type format
    unsigned short channels = 1;		//mono/stereo
    unsigned int bitRate = TAPEAUDIORATE;		//sample rate
    unsigned short bitsperSample = 8;	//Bits/sample
    unsigned int bytesperSec = bitRate * channels * (bitsperSample / 8);		//bytes/sec
    unsigned short blockAlign = (bitsperSample * channels) / 8;		//Block alignment
    unsigned int fileSize = instance->TotalSize + 40 - 8;
    unsigned int chunkSize = fileSize;

    sprintf(buffer, "RIFF");

    WriteFile(instance->TapeHandle, buffer, 4, &bytesMoved, NULL);
    WriteFile(instance->TapeHandle, &fileSize, 4, &bytesMoved, NULL);

    sprintf(buffer, "WAVE");

    WriteFile(instance->TapeHandle, buffer, 4, &bytesMoved, NULL);

    sprintf(buffer, "fmt ");

    WriteFile(instance->TapeHandle, buffer, 4, &bytesMoved, NULL);
    WriteFile(instance->TapeHandle, &formatSize, 4, &bytesMoved, NULL);
    WriteFile(instance->TapeHandle, &waveType, 2, &bytesMoved, NULL);
    WriteFile(instance->TapeHandle, &channels, 2, &bytesMoved, NULL);
    WriteFile(instance->TapeHandle, &bitRate, 4, &bytesMoved, NULL);
    WriteFile(instance->TapeHandle, &bytesperSec, 4, &bytesMoved, NULL);
    WriteFile(instance->TapeHandle, &blockAlign, 2, &bytesMoved, NULL);
    WriteFile(instance->TapeHandle, &bitsperSample, 2, &bytesMoved, NULL);

    sprintf(buffer, "data");

    WriteFile(instance->TapeHandle, buffer, 4, &bytesMoved, NULL);
    WriteFile(instance->TapeHandle, &chunkSize, 4, &bytesMoved, NULL);
  }
}
