#include "DirectSoundState.h"
#include "DirectSoundEnumerateCallback.h"

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

extern "C" {
  __declspec(dllexport) void __cdecl EnumerateSoundCards()
  {
    DirectSoundEnumerate(DirectSoundEnumerateCallback, NULL);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DirectSoundStop() {
    instance->lpdsbuffer1->Stop();
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl DirectSoundHasBuffer() {
    return instance->lpdsbuffer1 != NULL;
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DirectSoundBufferRelease() {
    HRESULT hResult = instance->lpdsbuffer1->Release();
    instance->lpdsbuffer1 = NULL;
    return hResult;
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl DirectSoundHasInterface() {
    return instance->lpds != NULL;
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DirectSoundInterfaceRelease() {
    HRESULT hResult = instance->lpds->Release();
    instance->lpds = NULL;
    return hResult;
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DirectSoundInitialize(GUID* guid) {
    return DirectSoundCreate(guid, &(instance->lpds), NULL);	// create a directsound object
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DirectSoundSetCooperativeLevel(HWND hWnd) {
    return instance->lpds->SetCooperativeLevel(hWnd, DSSCL_NORMAL); // set cooperation level normal DSSCL_EXCLUSIVE
  }
}
