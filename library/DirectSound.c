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
  __declspec(dllexport) void __cdecl DirectSoundStopAndRelease() {
    instance->lpdsbuffer1->Stop();
    instance->lpds->Release();
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DirectSoundSetCurrentPosition(DWORD position) {
    instance->lpdsbuffer1->SetCurrentPosition(position);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DirectSoundLock(DWORD buffOffset, unsigned short length, void** sndPointer1, DWORD* sndLength1, void** sndPointer2, DWORD* sndLength2) {
    return instance->lpdsbuffer1->Lock(buffOffset, length, sndPointer1, sndLength1, sndPointer2, sndLength2, 0);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DirectSoundUnlock(void* sndPointer1, DWORD sndLength1, void* sndPointer2, DWORD sndLength2) {
    return instance->lpdsbuffer1->Unlock(sndPointer1, sndLength1, sndPointer2, sndLength2);
  }
}
