#include <windows.h>

#define WRITEBUFFERSIZE	0x1FFFF

typedef struct
{
  unsigned char* CasBuffer;
} CassetteState;

CassetteState* InitializeInstance(CassetteState*);

static CassetteState* instance = InitializeInstance(new CassetteState());

extern "C" {
  __declspec(dllexport) CassetteState* __cdecl GetCassetteState() {
    return instance;
  }
}

CassetteState* InitializeInstance(CassetteState* p) {
  p->CasBuffer = NULL;

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
