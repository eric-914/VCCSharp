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

