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

static LPDIRECTINPUT8 _di;
static DDSURFACEDESC _ddsd;

/***********************************************************************************************/

//--IDirectDraw

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDCreate(IDirectDraw** dd)
  {
    return DirectDrawCreate(NULL, dd, NULL);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDGetDisplayMode(IDirectDraw* dd, DDSURFACEDESC* ddsd)
  {
    return dd->GetDisplayMode(ddsd);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDSetCooperativeLevel(IDirectDraw* dd, HWND hWnd, DWORD value)
  {
    return dd->SetCooperativeLevel(hWnd, value);
  }
}

//--IDirectDraw->Create[...]

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDCreateSurface(IDirectDraw* dd, IDirectDrawSurface** surface, DDSURFACEDESC* ddsd)
  {
    return dd->CreateSurface(ddsd, surface, NULL);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDCreateBackSurface(IDirectDraw* dd, IDirectDrawSurface** back, DDSURFACEDESC* ddsd)
  {
    return dd->CreateSurface(ddsd, back, NULL);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDCreateClipper(IDirectDraw* dd, IDirectDrawClipper** clipper)
  {
    return dd->CreateClipper(0, clipper, NULL);
  }
}

//--IDirectDrawClipper

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDClipperSetHWnd(IDirectDrawClipper* clipper, HWND hWnd)
  {
    return clipper->SetHWnd(0, hWnd);
  }
}

//--IDirectDrawSurface

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDSurfaceFlip(IDirectDrawSurface* surface)
  {
    return surface->Flip(NULL, DDFLIP_NOVSYNC | DDFLIP_DONOTWAIT); //DDFLIP_WAIT
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl DDSurfaceIsLost(IDirectDrawSurface* surface)
  {
    return surface->IsLost() == DDERR_SURFACELOST;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DDSurfaceRestore(IDirectDrawSurface* surface)
  {
    surface->Restore();
  }
}

//--IDirectDrawSurface/Back

extern "C" {
  __declspec(dllexport) HRESULT __cdecl LockDDBackSurface(IDirectDrawSurface* back, DDSURFACEDESC* ddsd, DWORD flags) {
    return back->Lock(NULL, ddsd, flags, NULL);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl UnlockDDBackSurface(IDirectDrawSurface* back) {
    return back->Unlock(NULL);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl GetDDBackSurfaceDC(IDirectDrawSurface* back, HDC* hdc) {
    back->GetDC(hdc);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl ReleaseDDBackSurfaceDC(IDirectDrawSurface* back, HDC hdc) {
    back->ReleaseDC(hdc);
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl DDBackSurfaceIsLost(IDirectDrawSurface* back)
  {
    return back->IsLost() == DDERR_SURFACELOST;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DDBackSurfaceRestore(IDirectDrawSurface* back)
  {
    back->Restore();
  }
}

//--IDirectDrawSurface/IDirectDraw[Back]Surface

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDSurfaceBlt(IDirectDrawSurface* surface, IDirectDrawSurface* back, RECT* rcDest, RECT* rcSrc)
  {
    return surface->Blt(rcDest, back, rcSrc, DDBLT_WAIT, NULL);
  }
}

//--IDirectDrawSurface/IDirectDrawClipper

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDSurfaceSetClipper(IDirectDrawSurface* surface, IDirectDrawClipper* clipper)
  {
    return surface->SetClipper(clipper);
  }
}

/***********************************************************************************************/

extern "C" {
  __declspec(dllexport) DDSURFACEDESC* __cdecl DDSDCreate()
  {
    _ddsd = DDSURFACEDESC();

    memset(&_ddsd, 0, sizeof(_ddsd));	// Clear all members of the structure to 0
    _ddsd.dwSize = sizeof(_ddsd);		  // The first parameter of the structure must contain the size of the structure

    return &_ddsd;
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

#define MAXSTICKS 10
#define STRLEN 64

static DIJOYSTATE2* _joyState = new DIJOYSTATE2();
static LPDIRECTINPUTDEVICE8 _joysticks[MAXSTICKS];

static char _stickName[MAXSTICKS][STRLEN];
static unsigned char _currentStick;

BOOL HasJoystick(unsigned char stickNumber) {
  return _joysticks[stickNumber] != NULL;
}

extern "C" {
  __declspec(dllexport) DIJOYSTATE2* __cdecl GetPollStick()
  {
    return _joyState;
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl JoystickPoll(DIJOYSTATE2* js, unsigned char stickNumber)
  {
    HRESULT hr;

    if (_joysticks[stickNumber] == NULL) {
      return (S_OK);
    }

    hr = _joysticks[stickNumber]->Poll();

    if (FAILED(hr))
    {
      hr = _joysticks[stickNumber]->Acquire();

      while (hr == DIERR_INPUTLOST) {
        hr = _joysticks[stickNumber]->Acquire();
      }

      if (hr == DIERR_INVALIDPARAM) {
        return(E_FAIL);
      }

      if (hr == DIERR_OTHERAPPHASPRIO) {
        return(S_OK);
      }
    }

    if (FAILED(hr = _joysticks[stickNumber]->GetDeviceState(sizeof(DIJOYSTATE2), js))) {
      return(hr);
    }

    return(S_OK);
  }
}

void SetStickName(unsigned char joystickIndex, const char* joystickName) {
  strncpy(_stickName[joystickIndex], joystickName, STRLEN);
}

extern "C" {
  __declspec(dllexport) int __cdecl EnumerateJoysticks()
  {
    HRESULT hr;
    static unsigned char joystickIndex = 0;

    LPDIENUMDEVICESCALLBACKA callback = [](const DIDEVICEINSTANCE* p, VOID* v) {
      HRESULT hr = _di->CreateDevice(p->guidInstance, &_joysticks[joystickIndex], NULL);

      SetStickName(joystickIndex, p->tszProductName);
      joystickIndex++;

      return (BOOL)(joystickIndex < MAXSTICKS);
    };

    if (FAILED(hr = DirectInput8Create(GetModuleHandle(NULL), DIRECTINPUT_VERSION, IID_IDirectInput8, (VOID**)&_di, NULL))) {
      return(0);
    }

    if (FAILED(hr = _di->EnumDevices(DI8DEVCLASS_GAMECTRL, callback, NULL, DIEDFL_ATTACHEDONLY))) {
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

      if (FAILED(_joysticks[_currentStick]->SetProperty(DIPROP_RANGE, &d.diph))) {
        return(DIENUM_STOP);
      }

      return (BOOL)(DIENUM_CONTINUE);
    };

    _currentStick = stickNumber;

    if (_joysticks[stickNumber] == NULL) {
      return(0);
    }

    if (FAILED(hr = _joysticks[stickNumber]->SetDataFormat(&c_dfDIJoystick2))) {
      return(0);
    }

    if (FAILED(hr = _joysticks[stickNumber]->EnumObjects(callback, NULL, DIDFT_AXIS))) {
      return(0);
    }

    return(1); //return true on success
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl JoyStickPoll(DIJOYSTATE2* js, unsigned char stickNumber)
  {
    HRESULT hr;

    if (_joysticks[stickNumber] == NULL) {
      return (S_OK);
    }

    hr = _joysticks[stickNumber]->Poll();

    if (FAILED(hr))
    {
      hr = _joysticks[stickNumber]->Acquire();

      while (hr == DIERR_INPUTLOST) {
        hr = _joysticks[stickNumber]->Acquire();
      }

      if (hr == DIERR_INVALIDPARAM) {
        return(E_FAIL);
      }

      if (hr == DIERR_OTHERAPPHASPRIO) {
        return(S_OK);
      }
    }

    if (FAILED(hr = _joysticks[stickNumber]->GetDeviceState(sizeof(DIJOYSTATE2), js))) {
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

DirectSoundState* InitializeDirectSoundState(DirectSoundState*);

static DirectSoundState* _sound = InitializeDirectSoundState(new DirectSoundState());

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

