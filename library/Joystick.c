#include "Joystick.h"

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

extern "C" {
  __declspec(dllexport) char* __cdecl GetStickName(int index) {
    return StickName[index];
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

/*
  Joystick related code
*/

extern "C"
{
  __declspec(dllexport) void __cdecl SetJoystick(unsigned short x, unsigned short y)
  {
    if (x > 63) {
      x = 63;
    }

    if (y > 63) {
      y = 63;
    }

    if (instance->Left->UseMouse == 1)
    {
      instance->LeftStickX = x;
      instance->LeftStickY = y;
    }

    if (instance->Right->UseMouse == 1)
    {
      instance->RightStickX = x;
      instance->RightStickY = y;
    }

    return;
  }
}

extern "C"
{
  __declspec(dllexport) void __cdecl SetStickNumbers(unsigned char leftStickNumber, unsigned char rightStickNumber)
  {
    instance->LeftStickNumber = leftStickNumber;
    instance->RightStickNumber = rightStickNumber;
  }
}

extern "C"
{
  __declspec(dllexport) unsigned short __cdecl get_pot_value(unsigned char pot)
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

    switch (pot)
    {
    case 0:
      return(instance->RightStickX);
      break;

    case 1:
      return(instance->RightStickY);
      break;

    case 2:
      return(instance->LeftStickX);
      break;

    case 3:
      return(instance->LeftStickY);
      break;
    }

    return (0);
  }
}

//0 = Left 1=right
extern "C"
{
  __declspec(dllexport) void __cdecl SetButtonStatus(unsigned char side, unsigned char state) //Side=0 Left Button Side=1 Right Button State 1=Down
  {
    unsigned char buttonStatus = (side << 1) | state;

    if (instance->Left->UseMouse == 1)
      switch (buttonStatus)
      {
      case 0:
        instance->LeftButton1Status = 0;
        break;

      case 1:
        instance->LeftButton1Status = 1;
        break;

      case 2:
        instance->LeftButton2Status = 0;
        break;

      case 3:
        instance->LeftButton2Status = 1;
        break;
      }

    if (instance->Right->UseMouse == 1)
      switch (buttonStatus)
      {
      case 0:
        instance->RightButton1Status = 0;
        break;

      case 1:
        instance->RightButton1Status = 1;
        break;

      case 2:
        instance->RightButton2Status = 0;
        break;

      case 3:
        instance->RightButton2Status = 1;
        break;
      }
  }
}

extern "C"
{
  __declspec(dllexport) char __cdecl SetMouseStatus(char scanCode, unsigned char phase)
  {
    char retValue = scanCode;

    switch (phase)
    {
    case 0:
      if (instance->Left->UseMouse == 0)
      {
        if (scanCode == instance->Left->Left)
        {
          instance->LeftStickX = 32;
          retValue = 0;
        }

        if (scanCode == instance->Left->Right)
        {
          instance->LeftStickX = 32;
          retValue = 0;
        }

        if (scanCode == instance->Left->Up)
        {
          instance->LeftStickY = 32;
          retValue = 0;
        }

        if (scanCode == instance->Left->Down)
        {
          instance->LeftStickY = 32;
          retValue = 0;
        }

        if (scanCode == instance->Left->Fire1)
        {
          instance->LeftButton1Status = 0;
          retValue = 0;
        }

        if (scanCode == instance->Left->Fire2)
        {
          instance->LeftButton2Status = 0;
          retValue = 0;
        }
      }

      if (instance->Right->UseMouse == 0)
      {
        if (scanCode == instance->Right->Left)
        {
          instance->RightStickX = 32;
          retValue = 0;
        }

        if (scanCode == instance->Right->Right)
        {
          instance->RightStickX = 32;
          retValue = 0;
        }

        if (scanCode == instance->Right->Up)
        {
          instance->RightStickY = 32;
          retValue = 0;
        }

        if (scanCode == instance->Right->Down)
        {
          instance->RightStickY = 32;
          retValue = 0;
        }

        if (scanCode == instance->Right->Fire1)
        {
          instance->RightButton1Status = 0;
          retValue = 0;
        }

        if (scanCode == instance->Right->Fire2)
        {
          instance->RightButton2Status = 0;
          retValue = 0;
        }
      }
      break;

    case 1:
      if (instance->Left->UseMouse == 0)
      {
        if (scanCode == instance->Left->Left)
        {
          instance->LeftStickX = 0;
          retValue = 0;
        }

        if (scanCode == instance->Left->Right)
        {
          instance->LeftStickX = 63;
          retValue = 0;
        }

        if (scanCode == instance->Left->Up)
        {
          instance->LeftStickY = 0;
          retValue = 0;
        }

        if (scanCode == instance->Left->Down)
        {
          instance->LeftStickY = 63;
          retValue = 0;
        }

        if (scanCode == instance->Left->Fire1)
        {
          instance->LeftButton1Status = 1;
          retValue = 0;
        }

        if (scanCode == instance->Left->Fire2)
        {
          instance->LeftButton2Status = 1;
          retValue = 0;
        }
      }

      if (instance->Right->UseMouse == 0)
      {
        if (scanCode == instance->Right->Left)
        {
          retValue = 0;
          instance->RightStickX = 0;
        }

        if (scanCode == instance->Right->Right)
        {
          instance->RightStickX = 63;
          retValue = 0;
        }

        if (scanCode == instance->Right->Up)
        {
          instance->RightStickY = 0;
          retValue = 0;
        }

        if (scanCode == instance->Right->Down)
        {
          instance->RightStickY = 63;
          retValue = 0;
        }

        if (scanCode == instance->Right->Fire1)
        {
          instance->RightButton1Status = 1;
          retValue = 0;
        }

        if (scanCode == instance->Right->Fire2)
        {
          instance->RightButton2Status = 1;
          retValue = 0;
        }
      }
      break;
    }

    return(retValue);
  }
}
