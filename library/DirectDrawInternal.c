#include "DirectDrawInternalState.h"

DirectDrawInternalState* InitializeInternal(DirectDrawInternalState*);

static DirectDrawInternalState* instance = InitializeInternal(new DirectDrawInternalState());

extern "C" {
  __declspec(dllexport) DirectDrawInternalState* __cdecl GetDirectDrawInternalState() {
    return instance;
  }
}

DirectDrawInternalState* InitializeInternal(DirectDrawInternalState* p) {
  p->DD = NULL;
  p->DDClipper = NULL;
  p->DDSurface = NULL;
  p->DDBackSurface = NULL;

  return p;
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl UnlockSurface() {
    return instance->DDBackSurface->Unlock(NULL);
  }
}
