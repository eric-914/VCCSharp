#include "DirectSoundState.h"
#include "Config.h"

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
  __declspec(dllexport) BOOL CALLBACK DirectSoundEnumerateCallback(LPGUID lpGuid, LPCSTR lpcstrDescription, LPCSTR lpcstrModule, LPVOID lpContext)
  {
    ConfigState* configState = GetConfigState();

    strncpy(configState->SoundCards[configState->NumberOfSoundCards].CardName, lpcstrDescription, 63);
    configState->SoundCards[configState->NumberOfSoundCards].Guid = lpGuid;
    configState->NumberOfSoundCards++;

    return (configState->NumberOfSoundCards < MAXCARDS);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DirectSoundEnumerateSoundCards()
  {
    DirectSoundEnumerate(DirectSoundEnumerateCallback, NULL);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DirectSoundStop() {
    return instance->lpdsbuffer1->Stop();
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

extern "C" {
  __declspec(dllexport) void __cdecl DirectSoundSetupFormatDataStructure(unsigned short bitRate) {
    memset(&(instance->pcmwf), 0, sizeof(WAVEFORMATEX));
    instance->pcmwf.wFormatTag = WAVE_FORMAT_PCM;
    instance->pcmwf.nChannels = 2;
    instance->pcmwf.nSamplesPerSec = bitRate;
    instance->pcmwf.wBitsPerSample = 16;
    instance->pcmwf.nBlockAlign = (instance->pcmwf.wBitsPerSample * instance->pcmwf.nChannels) >> 3;
    instance->pcmwf.nAvgBytesPerSec = instance->pcmwf.nSamplesPerSec * instance->pcmwf.nBlockAlign;
    instance->pcmwf.cbSize = 0;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DirectSoundSetupSecondaryBuffer(DWORD sndBuffLength) {
    memset(&(instance->dsbd), 0, sizeof(DSBUFFERDESC));
    instance->dsbd.dwSize = sizeof(DSBUFFERDESC);
    instance->dsbd.dwFlags = DSBCAPS_GETCURRENTPOSITION2 | DSBCAPS_LOCSOFTWARE | DSBCAPS_STATIC | DSBCAPS_GLOBALFOCUS;
    instance->dsbd.dwBufferBytes = sndBuffLength;
    instance->dsbd.lpwfxFormat = &(instance->pcmwf);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DirectSoundCreateSoundBuffer() {
    return instance->lpds->CreateSoundBuffer(&(instance->dsbd), &(instance->lpdsbuffer1), NULL);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DirectSoundPlay() {
    return instance->lpdsbuffer1->Play(0, 0, DSBPLAY_LOOPING);	// play the sound in looping mode
  }
}

extern "C" {
  __declspec(dllexport) long __cdecl DirectSoundGetCurrentPosition(unsigned long* playCursor, unsigned long* writeCursor)
  {
    return instance->lpdsbuffer1->GetCurrentPosition(playCursor, writeCursor);
  }
}
