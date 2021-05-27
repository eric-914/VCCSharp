#include <ShlObj.h>

#include "Config.h"
#include "JoyStickModel.h"

#include "fileoperations.h"

using namespace std;

#define STOP	0

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

  GetModuleFileName(NULL, p->ExecDirectory, MAX_PATH);
  FilePathRemoveFileSpec(p->ExecDirectory);

  static TCHAR AppDataPath[MAX_PATH];

  if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_APPDATA, NULL, 0, AppDataPath))) {
    OutputDebugString(AppDataPath);
  }

  strcpy(p->AppDataPath, AppDataPath);

  return p;
}

extern "C" {
  __declspec(dllexport) char* __cdecl GetSoundCardNameAtIndex(byte index) {
    return instance->SoundCards[index].CardName;
  }
}
