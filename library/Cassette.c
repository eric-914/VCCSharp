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
