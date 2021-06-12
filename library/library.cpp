/*
Has following depedency libraries:
    > ddraw.lib
    > dinput8.lib
    > dsound.lib
    > dxguid.lib;
*/
#define DIRECTINPUT_VERSION 0x0800
#include <ddraw.h>
#include <dinput.h>
#include <dsound.h>

#include <windows.h>

#define MAXSTICKS 10
#define STRLEN 64

static LPDIRECTINPUT8 di;
static LPDIRECTINPUTDEVICE8 Joysticks[MAXSTICKS];

static unsigned char CurrentStick;

static char StickName[MAXSTICKS][STRLEN];

static DIJOYSTATE2* PollStick = new DIJOYSTATE2();

typedef struct {
  //Global Variables for Direct Draw functions
  LPDIRECTDRAW        DD;             // The DirectDraw object
  LPDIRECTDRAWCLIPPER DDClipper;      // Clipper for primary surface
  LPDIRECTDRAWSURFACE DDSurface;      // Primary surface
  LPDIRECTDRAWSURFACE DDBackSurface;  // Back surface
} DirectDrawInternalState;

typedef struct {
  //PlayBack
  LPDIRECTSOUND	lpds;           // directsound interface pointer
  DSBUFFERDESC	dsbd;           // directsound description
  DSCAPS			  dscaps;         // directsound caps
  DSBCAPS			  dsbcaps;        // directsound buffer caps

  //Record
  LPDIRECTSOUNDCAPTURE8	lpdsin;
  DSCBUFFERDESC			    dsbdin; // directsound description

  LPDIRECTSOUNDBUFFER	lpdsbuffer1;			    //the sound buffers
  LPDIRECTSOUNDCAPTUREBUFFER	lpdsbuffer2;	//the sound buffers for capture

  WAVEFORMATEX pcmwf; //generic waveformat structure
} DirectSoundState;

DirectDrawInternalState* InitializeInternal(DirectDrawInternalState*);

static DirectDrawInternalState* _internal = InitializeInternal(new DirectDrawInternalState());

static WNDCLASSEX _wcex;

static DDSURFACEDESC ddsd;

extern "C" {
  __declspec(dllexport) DirectDrawInternalState* __cdecl GetDirectDrawInternalState() {
    return _internal;
  }
}

DirectDrawInternalState* InitializeInternal(DirectDrawInternalState* p) {
  p->DD = NULL;
  p->DDClipper = NULL;
  p->DDSurface = NULL;
  p->DDBackSurface = NULL;

  return p;
}

DirectSoundState* InitializeDirectSoundState(DirectSoundState*);

static DirectSoundState* _sound = InitializeDirectSoundState(new DirectSoundState());

/***********************************************************************************************/

extern "C" {
  __declspec(dllexport) DirectSoundState* __cdecl GetDirectSoundState() {
    return _sound;
  }
}

DirectSoundState* InitializeDirectSoundState(DirectSoundState* p) {
  p->lpdsbuffer1 = NULL;
  p->lpdsbuffer2 = NULL;

  return p;
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl LockDDBackSurface(DDSURFACEDESC* ddsd, DWORD flags) {
    return _internal->DDBackSurface->Lock(NULL, ddsd, flags, NULL);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl UnlockDDBackSurface() {
    return _internal->DDBackSurface->Unlock(NULL);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl GetDDBackSurfaceDC(HDC* hdc) {
    _internal->DDBackSurface->GetDC(hdc);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl ReleaseDDBackSurfaceDC(HDC hdc) {
    _internal->DDBackSurface->ReleaseDC(hdc);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDSurfaceFlip()
  {
    return _internal->DDSurface->Flip(NULL, DDFLIP_NOVSYNC | DDFLIP_DONOTWAIT); //DDFLIP_WAIT
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl HasDDBackSurface()
  {
    return _internal->DDBackSurface != NULL;
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl HasDDSurface()
  {
    return _internal->DDSurface != NULL;
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDSurfaceBlt(RECT* rcDest, RECT* rcSrc)
  {
    return _internal->DDSurface->Blt(rcDest, _internal->DDBackSurface, rcSrc, DDBLT_WAIT, NULL); // DDBLT_WAIT
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDSurfaceSetClipper()
  {
    return _internal->DDSurface->SetClipper(_internal->DDClipper);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDClipperSetHWnd(HWND hWnd)
  {
    return _internal->DDClipper->SetHWnd(0, hWnd);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDCreateClipper()
  {
    return _internal->DD->CreateClipper(0, &(_internal->DDClipper), NULL);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDGetDisplayMode(DDSURFACEDESC* ddsd)
  {
    return _internal->DD->GetDisplayMode(ddsd);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDCreateBackSurface(DDSURFACEDESC* ddsd)
  {
    return _internal->DD->CreateSurface(ddsd, &(_internal->DDBackSurface), NULL);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDCreateSurface(DDSURFACEDESC* ddsd)
  {
    return _internal->DD->CreateSurface(ddsd, &(_internal->DDSurface), NULL);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDSetCooperativeLevel(HWND hWnd, DWORD value)
  {
    return _internal->DD->SetCooperativeLevel(hWnd, value);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDCreate()
  {
    return DirectDrawCreate(NULL, &(_internal->DD), NULL);
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl DDSurfaceIsLost()
  {
    return _internal->DDSurface->IsLost() == DDERR_SURFACELOST;
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl DDBackSurfaceIsLost()
  {
    return _internal->DDBackSurface->IsLost() == DDERR_SURFACELOST;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DDSurfaceRestore()
  {
    _internal->DDSurface->Restore();
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DDBackSurfaceRestore()
  {
    _internal->DDBackSurface->Restore();
  }
}

extern "C" {
  __declspec(dllexport) DDSURFACEDESC* __cdecl DDSDCreate()
  {
    ddsd = DDSURFACEDESC();

    memset(&ddsd, 0, sizeof(ddsd));	// Clear all members of the structure to 0
    ddsd.dwSize = sizeof(ddsd);		  // The first parameter of the structure must contain the size of the structure

    return &ddsd;
  }
}

extern "C" {
  __declspec(dllexport) unsigned long __cdecl DDSDGetPitch(DDSURFACEDESC* ddsd)
  {
    return ddsd->lPitch;
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl DDSDHasSurface(DDSURFACEDESC* ddsd)
  {
    return ddsd->lpSurface != NULL;
  }
}

extern "C" {
  __declspec(dllexport) void* __cdecl DDSDGetSurface(DDSURFACEDESC* ddsd)
  {
    return ddsd->lpSurface;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DDSDSetdwCaps(DDSURFACEDESC* ddsd, DWORD value)
  {
    ddsd->ddsCaps.dwCaps = value;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DDSDSetdwWidth(DDSURFACEDESC* ddsd, DWORD value)
  {
    ddsd->dwWidth = value;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DDSDSetdwHeight(DDSURFACEDESC* ddsd, DWORD value)
  {
    ddsd->dwHeight = value;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DDSDSetdwFlags(DDSURFACEDESC* ddsd, DWORD value)
  {
    ddsd->dwFlags = value;
  }
}

/***********************************************************************************************/

BOOL HasJoystick(unsigned char stickNumber) {
  return Joysticks[stickNumber] != NULL;
}


extern "C" {
  __declspec(dllexport) DIJOYSTATE2* __cdecl GetPollStick()
  {
    return PollStick;
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl JoystickPoll(DIJOYSTATE2* js, unsigned char stickNumber)
  {
    HRESULT hr;

    if (Joysticks[stickNumber] == NULL) {
      return (S_OK);
    }

    hr = Joysticks[stickNumber]->Poll();

    if (FAILED(hr))
    {
      hr = Joysticks[stickNumber]->Acquire();

      while (hr == DIERR_INPUTLOST) {
        hr = Joysticks[stickNumber]->Acquire();
      }

      if (hr == DIERR_INVALIDPARAM) {
        return(E_FAIL);
      }

      if (hr == DIERR_OTHERAPPHASPRIO) {
        return(S_OK);
      }
    }

    if (FAILED(hr = Joysticks[stickNumber]->GetDeviceState(sizeof(DIJOYSTATE2), js))) {
      return(hr);
    }

    return(S_OK);
  }
}

void SetStickName(unsigned char joystickIndex, const char* joystickName) {
  strncpy(StickName[joystickIndex], joystickName, STRLEN);
}

extern "C" {
  __declspec(dllexport) int __cdecl EnumerateJoysticks()
  {
    HRESULT hr;
    static unsigned char joystickIndex = 0;

    LPDIENUMDEVICESCALLBACKA callback = [](const DIDEVICEINSTANCE* p, VOID* v) {
      HRESULT hr = di->CreateDevice(p->guidInstance, &Joysticks[joystickIndex], NULL);

      SetStickName(joystickIndex, p->tszProductName);
      joystickIndex++;

      return (BOOL)(joystickIndex < MAXSTICKS);
    };

    if (FAILED(hr = DirectInput8Create(GetModuleHandle(NULL), DIRECTINPUT_VERSION, IID_IDirectInput8, (VOID**)&di, NULL))) {
      return(0);
    }

    if (FAILED(hr = di->EnumDevices(DI8DEVCLASS_GAMECTRL, callback, NULL, DIEDFL_ATTACHEDONLY))) {
      return(0);
    }

    return(joystickIndex);
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl InitJoyStick(unsigned char stickNumber)
  {
    //	DIDEVCAPS capabilities;
    HRESULT hr;

    LPDIENUMDEVICEOBJECTSCALLBACKA callback = [](const DIDEVICEOBJECTINSTANCE* p, VOID* v) {
      DIPROPRANGE d;
      d.diph.dwSize = sizeof(DIPROPRANGE);
      d.diph.dwHeaderSize = sizeof(DIPROPHEADER);
      d.diph.dwHow = DIPH_BYID;
      d.diph.dwObj = p->dwType;
      d.lMin = 0;
      d.lMax = 0xFFFF;

      if (FAILED(Joysticks[CurrentStick]->SetProperty(DIPROP_RANGE, &d.diph))) {
        return(DIENUM_STOP);
      }

      return (BOOL)(DIENUM_CONTINUE);
    };

    CurrentStick = stickNumber;

    if (Joysticks[stickNumber] == NULL) {
      return(0);
    }

    if (FAILED(hr = Joysticks[stickNumber]->SetDataFormat(&c_dfDIJoystick2))) {
      return(0);
    }

    if (FAILED(hr = Joysticks[stickNumber]->EnumObjects(callback, NULL, DIDFT_AXIS))) {
      return(0);
    }

    return(1); //return true on success
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl JoyStickPoll(DIJOYSTATE2* js, unsigned char stickNumber)
  {
    HRESULT hr;

    if (Joysticks[stickNumber] == NULL) {
      return (S_OK);
    }

    hr = Joysticks[stickNumber]->Poll();

    if (FAILED(hr))
    {
      hr = Joysticks[stickNumber]->Acquire();

      while (hr == DIERR_INPUTLOST) {
        hr = Joysticks[stickNumber]->Acquire();
      }

      if (hr == DIERR_INVALIDPARAM) {
        return(E_FAIL);
      }

      if (hr == DIERR_OTHERAPPHASPRIO) {
        return(S_OK);
      }
    }

    if (FAILED(hr = Joysticks[stickNumber]->GetDeviceState(sizeof(DIJOYSTATE2), js))) {
      return(hr);
    }

    return(S_OK);
  }
}

extern "C" {
  __declspec(dllexport) unsigned short __cdecl StickX(DIJOYSTATE2* stick) {
    return (unsigned short)stick->lX;
  }
}

extern "C" {
  __declspec(dllexport) unsigned short __cdecl StickY(DIJOYSTATE2* stick) {
    return (unsigned short)stick->lY;
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl Button(DIJOYSTATE2* stick, int index) {
    return stick->rgbButtons[index];
  }
}

/***********************************************************************************************/

extern "C" {
  __declspec(dllexport) void __cdecl DirectSoundStopAndRelease() {
    _sound->lpdsbuffer1->Stop();
    _sound->lpds->Release();
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DirectSoundSetCurrentPosition(DWORD position) {
    _sound->lpdsbuffer1->SetCurrentPosition(position);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DirectSoundLock(DWORD buffOffset, unsigned short length, void** sndPointer1, DWORD* sndLength1, void** sndPointer2, DWORD* sndLength2) {
    return _sound->lpdsbuffer1->Lock(buffOffset, length, sndPointer1, sndLength1, sndPointer2, sndLength2, 0);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DirectSoundUnlock(void* sndPointer1, DWORD sndLength1, void* sndPointer2, DWORD sndLength2) {
    return _sound->lpdsbuffer1->Unlock(sndPointer1, sndLength1, sndPointer2, sndLength2);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DirectSoundEnumerateSoundCards(LPDSENUMCALLBACKA pDSEnumCallback)
  {
    DirectSoundEnumerate(pDSEnumCallback, NULL);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DirectSoundStop() {
    return _sound->lpdsbuffer1->Stop();
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl DirectSoundHasBuffer() {
    return _sound->lpdsbuffer1 != NULL;
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DirectSoundBufferRelease() {
    HRESULT hResult = _sound->lpdsbuffer1->Release();
    _sound->lpdsbuffer1 = NULL;
    return hResult;
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl DirectSoundHasInterface() {
    return _sound->lpds != NULL;
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DirectSoundInterfaceRelease() {
    HRESULT hResult = _sound->lpds->Release();
    _sound->lpds = NULL;
    return hResult;
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DirectSoundInitialize(GUID* guid) {
    return DirectSoundCreate(guid, &(_sound->lpds), NULL);	// create a directsound object
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DirectSoundSetCooperativeLevel(HWND hWnd, DWORD flag) {
    return _sound->lpds->SetCooperativeLevel(hWnd, flag); 
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DirectSoundSetupFormatDataStructure(unsigned short bitRate) {
    memset(&(_sound->pcmwf), 0, sizeof(WAVEFORMATEX));
    _sound->pcmwf.wFormatTag = WAVE_FORMAT_PCM;
    _sound->pcmwf.nChannels = 2;
    _sound->pcmwf.nSamplesPerSec = bitRate;
    _sound->pcmwf.wBitsPerSample = 16;
    _sound->pcmwf.nBlockAlign = (_sound->pcmwf.wBitsPerSample * _sound->pcmwf.nChannels) >> 3;
    _sound->pcmwf.nAvgBytesPerSec = _sound->pcmwf.nSamplesPerSec * _sound->pcmwf.nBlockAlign;
    _sound->pcmwf.cbSize = 0;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DirectSoundSetupSecondaryBuffer(DWORD sndBuffLength, DWORD flags) {
    memset(&(_sound->dsbd), 0, sizeof(DSBUFFERDESC));
    _sound->dsbd.dwSize = sizeof(DSBUFFERDESC);
    _sound->dsbd.dwFlags = flags;
    _sound->dsbd.dwBufferBytes = sndBuffLength;
    _sound->dsbd.lpwfxFormat = &(_sound->pcmwf);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DirectSoundCreateSoundBuffer() {
    return _sound->lpds->CreateSoundBuffer(&(_sound->dsbd), &(_sound->lpdsbuffer1), NULL);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DirectSoundPlay() {
    return _sound->lpdsbuffer1->Play(0, 0, DSBPLAY_LOOPING);	// play the sound in looping mode
  }
}

extern "C" {
  __declspec(dllexport) long __cdecl DirectSoundGetCurrentPosition(unsigned long* playCursor, unsigned long* writeCursor)
  {
    return _sound->lpdsbuffer1->GetCurrentPosition(playCursor, writeCursor);
  }
}

/***********************************************************************************************/

static HINSTANCE g_hinstDLL = NULL;

BOOL WINAPI DllMain(
  HINSTANCE hinstDLL,  // handle to DLL module
  DWORD fdwReason,     // reason for calling function
  LPVOID lpReserved)   // reserved
{
  switch (fdwReason)
  {
  case DLL_PROCESS_ATTACH:
  case DLL_THREAD_ATTACH:
  case DLL_THREAD_DETACH:
    g_hinstDLL = hinstDLL;
    break;

  case DLL_PROCESS_DETACH:
    break;
  }

  return TRUE;
}

