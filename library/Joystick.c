#include "di.version.h"
#include <dinput.h>

#include "JoystickState.h"

#define MAXSTICKS 10
#define STRLEN 64

static LPDIRECTINPUT8 di;
static LPDIRECTINPUTDEVICE8 Joysticks[MAXSTICKS];

static unsigned char CurrentStick;

static char StickName[MAXSTICKS][STRLEN];

JoystickState* InitializeInstance(JoystickState*);

static JoystickState* instance = InitializeInstance(new JoystickState());

extern "C" {
  __declspec(dllexport) JoystickState* __cdecl GetJoystickState() {
    return instance;
  }
}

JoystickState* InitializeInstance(JoystickState* p) {
  p->StickValue = 0;

  p->LeftStickNumber = 0;
  p->LeftStickX = 32;
  p->LeftStickY = 32;
  p->LeftButton1Status = 0;
  p->LeftButton2Status = 0;

  p->RightStickNumber = 0;
  p->RightStickX = 32;
  p->RightStickY = 32;
  p->RightButton1Status = 0;
  p->RightButton2Status = 0;

  return p;
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

/*
  Joystick related code
*/

extern "C"
{
  __declspec(dllexport) void __cdecl get_pot_value()
  {
    DIJOYSTATE2 stick;

    if (instance->Left->UseMouse == 3)
    {
      JoyStickPoll(&stick, instance->LeftStickNumber);

      instance->LeftStickX = (unsigned short)stick.lX >> 10;
      instance->LeftStickY = (unsigned short)stick.lY >> 10;
      instance->LeftButton1Status = stick.rgbButtons[0] >> 7;
      instance->LeftButton2Status = stick.rgbButtons[1] >> 7;
    }

    if (instance->Right->UseMouse == 3)
    {
      JoyStickPoll(&stick, instance->RightStickNumber);

      instance->RightStickX = (unsigned short)stick.lX >> 10;
      instance->RightStickY = (unsigned short)stick.lY >> 10;
      instance->RightButton1Status = stick.rgbButtons[0] >> 7;
      instance->RightButton2Status = stick.rgbButtons[1] >> 7;
    }
  }
}
