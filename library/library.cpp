/*
Has following depedency libraries:
    > ddraw.lib
    > dinput8.lib
    > dsound.lib
    > dxguid.lib;
*/
#define DIRECTINPUT_VERSION 0x0800
#include <dinput.h>
#include <dsound.h>

/***********************************************************************************************/

//--DirectInput / Joysticks

#define MAXSTICKS 10
#define STRLEN 64

static DIJOYSTATE2* _joyState = new DIJOYSTATE2();
static LPDIRECTINPUTDEVICE8 _joysticks[MAXSTICKS];
static LPDIRECTINPUT8 _di;

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

//--DirectSound / Audio

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

extern "C" {
  __declspec(dllexport) DirectSoundState* __cdecl GetDirectSoundState() {
    return new DirectSoundState();
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DirectSoundInitialize(DirectSoundState* ds, GUID* guid) {
    return DirectSoundCreate(guid, &(ds->lpds), NULL);	// create a directsound object
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DirectSoundSetCooperativeLevel(DirectSoundState* ds, HWND hWnd, DWORD flag) {
    return ds->lpds->SetCooperativeLevel(hWnd, flag); 
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DirectSoundSetupFormatDataStructure(DirectSoundState* ds, unsigned short bitRate) {
    memset(&(ds->pcmwf), 0, sizeof(WAVEFORMATEX));
    ds->pcmwf.wFormatTag = WAVE_FORMAT_PCM;
    ds->pcmwf.nChannels = 2;
    ds->pcmwf.nSamplesPerSec = bitRate;
    ds->pcmwf.wBitsPerSample = 16;
    ds->pcmwf.nBlockAlign = (ds->pcmwf.wBitsPerSample * ds->pcmwf.nChannels) >> 3;
    ds->pcmwf.nAvgBytesPerSec = ds->pcmwf.nSamplesPerSec * ds->pcmwf.nBlockAlign;
    ds->pcmwf.cbSize = 0;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DirectSoundSetupSecondaryBuffer(DirectSoundState* ds, DWORD sndBuffLength, DWORD flags) {
    memset(&(ds->dsbd), 0, sizeof(DSBUFFERDESC));
    ds->dsbd.dwSize = sizeof(DSBUFFERDESC);
    ds->dsbd.dwFlags = flags;
    ds->dsbd.dwBufferBytes = sndBuffLength;
    ds->dsbd.lpwfxFormat = &(ds->pcmwf);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DirectSoundCreateSoundBuffer(DirectSoundState* ds) {
    return ds->lpds->CreateSoundBuffer(&(ds->dsbd), &(ds->lpdsbuffer1), NULL);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DirectSoundLock(DirectSoundState* ds, DWORD buffOffset, unsigned short length, void** sndPointer1, DWORD* sndLength1, void** sndPointer2, DWORD* sndLength2) {
    return ds->lpdsbuffer1->Lock(buffOffset, length, sndPointer1, sndLength1, sndPointer2, sndLength2, 0);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DirectSoundUnlock(DirectSoundState* ds, void* sndPointer1, DWORD sndLength1, void* sndPointer2, DWORD sndLength2) {
    return ds->lpdsbuffer1->Unlock(sndPointer1, sndLength1, sndPointer2, sndLength2);
  }
}

extern "C" {
  __declspec(dllexport) long __cdecl DirectSoundGetCurrentPosition(DirectSoundState* ds, unsigned long* playCursor, unsigned long* writeCursor) {
    return ds->lpdsbuffer1->GetCurrentPosition(playCursor, writeCursor);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DirectSoundSetCurrentPosition(DirectSoundState* ds, DWORD position) {
    ds->lpdsbuffer1->SetCurrentPosition(position);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DirectSoundPlay(DirectSoundState* ds) {
    return ds->lpdsbuffer1->Play(0, 0, DSBPLAY_LOOPING);	// play the sound in looping mode
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DirectSoundStop(DirectSoundState* ds) {
    return ds->lpdsbuffer1->Stop();
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DirectSoundStopAndRelease(DirectSoundState* ds) {
    ds->lpdsbuffer1->Stop();
    ds->lpds->Release();
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl DirectSoundHasBuffer(DirectSoundState* ds) {
    return ds->lpdsbuffer1 != NULL;
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DirectSoundBufferRelease(DirectSoundState* ds) {
    HRESULT hResult = ds->lpdsbuffer1->Release();
    ds->lpdsbuffer1 = NULL;
    return hResult;
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl DirectSoundHasInterface(DirectSoundState* ds) {
    return ds->lpds != NULL;
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DirectSoundInterfaceRelease(DirectSoundState* ds) {
    HRESULT hResult = ds->lpds->Release();
    ds->lpds = NULL;
    return hResult;
  }
}

//....................................................................//

extern "C" {
  __declspec(dllexport) void __cdecl DirectSoundEnumerateSoundCards(LPDSENUMCALLBACKA pDSEnumCallback)
  {
    DirectSoundEnumerate(pDSEnumCallback, NULL);
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

