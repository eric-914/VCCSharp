#include <string>

#include "CassetteState.h"

#include "defines.h"

#include "fileoperations.h"

using namespace std;

#define WRITEBUFFERSIZE	0x1FFFF

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
