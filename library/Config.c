#include <windows.h>

#include "Config.h"
#include "JoyStickModel.h"

ConfigState* InitializeInstance(ConfigState*);
JoystickModel* InitializeModel(JoystickModel*);

static ConfigState* instance = InitializeInstance(new ConfigState());
static ConfigModel* model = new ConfigModel();
static JoystickModel* left = InitializeModel(new JoystickModel());
static JoystickModel* right = InitializeModel(new JoystickModel());

extern "C" {
  __declspec(dllexport) ConfigState* __cdecl GetConfigState() {
    return instance;
  }
}

extern "C" {
  __declspec(dllexport) ConfigModel* __cdecl GetConfigModel() {
    return model;
  }
}

extern "C" {
  __declspec(dllexport) JoystickModel* __cdecl GetLeftJoystick() {
    return left;
  }
}

extern "C" {
  __declspec(dllexport) JoystickModel* __cdecl GetRightJoystick() {
    return right;
  }
}

JoystickModel* InitializeModel(JoystickModel* p) {
  p->DiDevice = 0;
  p->HiRes = 0;
  p->UseMouse = 0;

  return p;
}

ConfigState* InitializeInstance(ConfigState* p) {
  strcpy(p->IniFilePath, "");

  return p;
}

extern "C" {
  __declspec(dllexport) char* __cdecl GetSoundCardNameAtIndex(byte index) {
    return instance->SoundCards[index].CardName;
  }
}
