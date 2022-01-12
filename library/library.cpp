#define DIRECTINPUT_VERSION 0x0800
#include <dinput.h>

/***********************************************************************************************/

#define MAXSTICKS 8

static DIJOYSTATE2* _joyState = new DIJOYSTATE2();
static LPDIRECTINPUTDEVICE8 _joysticks[MAXSTICKS];

static unsigned char _joystickIndex = 0;
static LPDIRECTINPUTDEVICE8 _stick;

extern "C" {
  __declspec(dllexport) DIDATAFORMAT GetDataFormat() {
    return c_dfDIJoystick2;
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl SetJoystickPropertiesCallback(const DIDEVICEOBJECTINSTANCE* p, VOID* v)
  {
    DIPROPRANGE d;
    d.diph.dwSize = sizeof(DIPROPRANGE);
    d.diph.dwHeaderSize = sizeof(DIPROPHEADER);
    d.diph.dwHow = DIPH_BYID;
    d.diph.dwObj = p->dwType;
    d.lMin = 0;
    d.lMax = 0xFFFF;

    if (FAILED(_stick->SetProperty(DIPROP_RANGE, &d.diph))) {
      return(DIENUM_STOP);
    }

    return (BOOL)(DIENUM_CONTINUE);
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl SetJoystickDataFormat(LPDIRECTINPUTDEVICE8 stick, DIDATAFORMAT df) {
    HRESULT hr = stick->SetDataFormat(&df);

    return FAILED(hr) ? 0 : 1;
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl SetJoystickProperties(LPDIRECTINPUTDEVICE8 stick, LPDIENUMDEVICEOBJECTSCALLBACKA callback)
  {
    _stick = stick;

    HRESULT hr = stick->EnumObjects(callback, NULL, DIDFT_AXIS);

    return FAILED(hr) ? 0 : 1;
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
