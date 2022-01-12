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
  __declspec(dllexport) GUID* __cdecl GetRangeGuid()
  {
    return (GUID*)(&DIPROP_RANGE);
  }
}
    
extern "C" {
  __declspec(dllexport) DIJOYSTATE2* __cdecl GetJoystickState()
  {
    return _joyState;
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl JoystickPoll(DIJOYSTATE2* state, unsigned char stickNumber)
  {
    LPDIRECTINPUTDEVICE8 stick = _joysticks[stickNumber];

    if (stick == NULL) {
      return (S_OK);
    }

    HRESULT hr = stick->Poll();

    if (FAILED(hr))
    {
      hr = stick->Acquire();

      while (hr == DIERR_INPUTLOST) {
        hr = stick->Acquire();
      }

      if (hr == DIERR_INVALIDPARAM) {
        return(E_FAIL);
      }

      if (hr == DIERR_OTHERAPPHASPRIO) {
        return(S_OK);
      }
    }

    if (FAILED(hr = stick->GetDeviceState(sizeof(DIJOYSTATE2), state))) {
      return(hr);
    }

    return(S_OK);
  }
}

extern "C" {
  __declspec(dllexport) unsigned short __cdecl StickX(DIJOYSTATE2* state) {
    return (unsigned short)state->lX;
  }
}

extern "C" {
  __declspec(dllexport) unsigned short __cdecl StickY(DIJOYSTATE2* state) {
    return (unsigned short)state->lY;
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl Button(DIJOYSTATE2* state, int index) {
    return state->rgbButtons[index];
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
