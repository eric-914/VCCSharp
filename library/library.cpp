#define DIRECTINPUT_VERSION 0x0800
#include <dinput.h>

/***********************************************************************************************/

#define MAXSTICKS 8

static DIJOYSTATE2* _joyState = new DIJOYSTATE2();
static LPDIRECTINPUTDEVICE8 _joysticks[MAXSTICKS];

static unsigned char _joystickIndex = 0;
static char* _buffer;
static unsigned char _bufferSize;
static LPDIRECTINPUT8 _di;

extern "C" {
  __declspec(dllexport) int __cdecl EnumerateCallback(LPCDIDEVICEINSTANCEA p, void* v)
  {
    HRESULT hr = _di->CreateDevice(p->guidInstance, &_joysticks[_joystickIndex], NULL);

    unsigned int offset = _joystickIndex * _bufferSize;

    strncpy(_buffer + offset, p->tszProductName, _bufferSize);

    _joystickIndex++;

    return (int)(_joystickIndex < MAXSTICKS);
  }
}

extern "C" {
  __declspec(dllexport) int __cdecl EnumerateJoysticks(LPDIRECTINPUT8 di, char* buffer, unsigned char bufferSize, LPDIENUMDEVICESCALLBACKA callback)
  {
    _di = di;
    _buffer = buffer;
    _bufferSize = bufferSize;

    HRESULT hr = _di->EnumDevices(DI8DEVCLASS_GAMECTRL, callback, NULL, DIEDFL_ATTACHEDONLY);

    return FAILED(hr) ? 0 : _joystickIndex;
  }
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

extern "C" {
  __declspec(dllexport) BOOL __cdecl InitJoyStick(unsigned char stickNumber)
  {
    static LPDIRECTINPUTDEVICE8 stick = _joysticks[stickNumber];

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

      if (FAILED(stick->SetProperty(DIPROP_RANGE, &d.diph))) {
        return(DIENUM_STOP);
      }

      return (BOOL)(DIENUM_CONTINUE);
    };

    if (stick == NULL) {
      return(0);
    }

    if (FAILED(hr = stick->SetDataFormat(&c_dfDIJoystick2))) {
      return(0);
    }

    if (FAILED(hr = stick->EnumObjects(callback, NULL, DIDFT_AXIS))) {
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
