#include <string>

#include "CassetteState.h"

#include "defines.h"
#include "macros.h"

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
  p->MotorState = 0;
  p->Quiet = 30;
  p->TapeMode = STOP;
  p->TapeOffset = 0;
  p->TempIndex = 0;
  p->TotalSize = 0;
  p->WriteProtect = 0;

  p->CasBuffer = NULL;
  p->TapeHandle = NULL;

  strcpy(p->TapeFileName, "");

  ARRAYCOPY(Zero);
  ARRAYCOPY(One);

  return p;
}

extern "C" {
  __declspec(dllexport) void __cdecl SyncFileBuffer()
  {
    char buffer[64] = "";
    unsigned long bytesMoved = 0;
    unsigned int fileSize;
    unsigned short waveType = 1;		//WAVE type format
    unsigned int formatSize = 16;		//size of WAVE section chunck
    unsigned short channels = 1;		//mono/stereo
    unsigned int bitRate = TAPEAUDIORATE;		//sample rate
    unsigned short bitsperSample = 8;	//Bits/sample
    unsigned int bytesperSec = bitRate * channels * (bitsperSample / 8);		//bytes/sec
    unsigned short blockAlign = (bitsperSample * channels) / 8;		//Block alignment
    unsigned int chunkSize;

    fileSize = instance->TotalSize + 40 - 8;
    chunkSize = fileSize;

    SetFilePointer(instance->TapeHandle, 0, 0, FILE_BEGIN);

    switch (instance->FileType)
    {
    case CAS:
      instance->CasBuffer[instance->TapeOffset] = instance->Byte;	//capture the last byte
      instance->LastTrans = 0;	//reset all static inter-call variables
      instance->Mask = 0;
      instance->Byte = 0;
      instance->LastSample = 0;
      instance->TempIndex = 0;

      WriteFile(instance->TapeHandle, instance->CasBuffer, instance->TapeOffset, &bytesMoved, NULL);

      break;

    case WAV:
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

      break;
    }

    FlushFileBuffers(instance->TapeHandle);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl CloseTapeFile()
  {
    if (instance->TapeHandle == NULL) {
      return;
    }

    SyncFileBuffer();
    CloseHandle(instance->TapeHandle);

    instance->TapeHandle = NULL;
    instance->TotalSize = 0;
  }
}

extern "C" {
  __declspec(dllexport) int __cdecl MountTape(char* filename)	//Return 1 on sucess 0 on fail
  {
    char extension[4] = "";
    unsigned char index = 0;

    if (instance->TapeHandle != NULL)
    {
      instance->TapeMode = STOP;

      CloseTapeFile();
    }

    instance->WriteProtect = 0;
    instance->FileType = 0;	//0=wav 1=cas
    instance->TapeHandle = CreateFile(filename, GENERIC_READ | GENERIC_WRITE, 0, 0, OPEN_ALWAYS, FILE_ATTRIBUTE_NORMAL, 0);

    if (instance->TapeHandle == INVALID_HANDLE_VALUE)	//Can't open read/write. try read only
    {
      instance->TapeHandle = CreateFile(filename, GENERIC_READ, 0, 0, OPEN_ALWAYS, FILE_ATTRIBUTE_NORMAL, 0);
      instance->WriteProtect = 1;
    }

    if (instance->TapeHandle == INVALID_HANDLE_VALUE)
    {
      MessageBox(0, "Can't Mount", "Error", 0);

      return(0);	//Give up
    }

    instance->TotalSize = SetFilePointer(instance->TapeHandle, 0, 0, FILE_END);
    instance->TapeOffset = 0;

    strcpy(extension, &filename[strlen(filename) - 3]);

    for (index = 0; index < strlen(extension); index++) {
      extension[index] = toupper(extension[index]);
    }

    if (strcmp(extension, "WAV"))
    {
      instance->FileType = CAS;
      instance->LastTrans = 0;
      instance->Mask = 0;
      instance->Byte = 0;
      instance->LastSample = 0;
      instance->TempIndex = 0;

      if (instance->CasBuffer != NULL) {
        free(instance->CasBuffer);
      }

      instance->CasBuffer = (unsigned char*)malloc(WRITEBUFFERSIZE);

      SetFilePointer(instance->TapeHandle, 0, 0, FILE_BEGIN);

      ReadFile(instance->TapeHandle, instance->CasBuffer, instance->TotalSize, &(instance->BytesMoved), NULL);	//Read the whole file in for .CAS files

      if (instance->BytesMoved != instance->TotalSize) {
        return(0);
      }
    }

    return(1);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl FlushCassetteWAV(unsigned char* buffer, unsigned int length)
  {
      SetFilePointer(instance->TapeHandle, instance->TapeOffset + 44, 0, FILE_BEGIN);
      WriteFile(instance->TapeHandle, buffer, length, &(instance->BytesMoved), NULL);

      if (length != instance->BytesMoved) {
        return;
      }

      instance->TapeOffset += length;

      if (instance->TapeOffset > instance->TotalSize) {
        instance->TotalSize = instance->TapeOffset;
      }
  }
}
