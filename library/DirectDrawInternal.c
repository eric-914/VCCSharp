#include "DirectDrawInternalState.h"

DirectDrawInternalState* InitializeInternal(DirectDrawInternalState*);

static DirectDrawInternalState* internal = InitializeInternal(new DirectDrawInternalState());

extern "C" {
  __declspec(dllexport) DirectDrawInternalState* __cdecl GetDirectDrawInternalState() {
    return internal;
  }
}

DirectDrawInternalState* InitializeInternal(DirectDrawInternalState* p) {
  p->DD = NULL;
  p->DDClipper = NULL;
  p->DDSurface = NULL;
  p->DDBackSurface = NULL;

  return p;
}
