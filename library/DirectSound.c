#include "DirectSoundState.h"

//--Generally a wrapper around <dsound.h>

DirectSoundState* InitializeDirectSoundState(DirectSoundState*);

static DirectSoundState* instance = InitializeDirectSoundState(new DirectSoundState());

extern "C" {
  __declspec(dllexport) DirectSoundState* __cdecl GetDirectSoundState() {
    return instance;
  }
}

DirectSoundState* InitializeDirectSoundState(DirectSoundState* p) {
  p->lpdsbuffer1 = NULL;
  p->lpdsbuffer2 = NULL;

  return p;
}

extern "C" {
  __declspec(dllexport) void __cdecl StopAndRelease() {
    instance->lpdsbuffer1->Stop();
    instance->lpds->Release();
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetCurrentPosition(DWORD position) {
    instance->lpdsbuffer1->SetCurrentPosition(position);
  }
}
