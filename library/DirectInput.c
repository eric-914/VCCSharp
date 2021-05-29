#include "di.version.h"
#include <dinput.h>

#define MAXSTICKS 10
#define STRLEN 64

static LPDIRECTINPUT8 di;
static LPDIRECTINPUTDEVICE8 Joysticks[MAXSTICKS];

static unsigned char CurrentStick;

static char StickName[MAXSTICKS][STRLEN];

BOOL HasJoystick(unsigned char stickNumber) {
  return Joysticks[stickNumber] != NULL;
}

static DIJOYSTATE2* PollStick = new DIJOYSTATE2();

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
