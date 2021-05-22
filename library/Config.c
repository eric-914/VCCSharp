#include <ShlObj.h>

#include "../resources/resource.h"

#include "Config.h"
#include "Cassette.h"
#include "Audio.h"

#include "fileoperations.h"

#include "VccState.h"

#include "ConfigConstants.h"

using namespace std;

ConfigState* InitializeInstance(ConfigState*, ConfigModel*, JoystickModel*, JoystickModel*);
JoystickModel* InitializeModel(JoystickModel* p);

static ConfigState* instance = InitializeInstance(new ConfigState(), new ConfigModel(), InitializeModel(new JoystickModel()), InitializeModel(new JoystickModel()));
static ConfigModel* model;
static JoystickModel* left;
static JoystickModel* right;

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

ConfigState* InitializeInstance(ConfigState* p, ConfigModel* m, JoystickModel* l, JoystickModel* r) {
  p->Model = model = m;
  model->Left = left = l;
  model->Right = right = r;

  OutputDebugString("Here!");

  p->NumberOfSoundCards = 0;
  p->NumberOfJoysticks = 0;
  p->PrintMonitorWindow = 0;
  p->TapeCounter = 0;
  p->TextMode = 1;
  p->TapeMode = STOP;

  p->hDlgBar = NULL;
  p->hDlgTape = NULL;

  strcpy(p->IniFilePath, "");
  strcpy(p->OutBuffer, "");
  strcpy(p->SerialCaptureFile, "");
  strcpy(p->TapeFileName, "");

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
  __declspec(dllexport) unsigned char __cdecl GetSoundCardIndex(char* soundCardName) {
    for (unsigned char index = 0; index < instance->NumberOfSoundCards; index++) {
      if (!strcmp(instance->SoundCards[index].CardName, soundCardName)) {
        return index;
      }
    }

    return 0;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetIniFilePath(char* path)
  {
    //  Path must be to an existing ini file
    strcpy(instance->IniFilePath, path);
  }
}

extern "C" {
  __declspec(dllexport) int __cdecl GetRememberSize() {
    return((int)(instance->Model->RememberSize));
  }
}

extern "C" {
  __declspec(dllexport) POINT __cdecl GetIniWindowSize() {
    POINT out = POINT();

    out.x = instance->Model->WindowSizeX;
    out.y = instance->Model->WindowSizeY;

    return(out);
  }
}

void CheckAudioChange(EmuState* emuState, ConfigModel* current, SoundCardList* soundCards) {
  //unsigned char currentSoundCardIndex = GetSoundCardIndex(current->SoundCardName);
  //unsigned char tempSoundCardIndex = GetSoundCardIndex(temp->SoundCardName);
  unsigned char tempSoundCardIndex = GetSoundCardIndex(current->SoundCardName);

  //TODO: current and temp are pointing to the same object.
  //if ((currentSoundCardIndex != tempSoundCardIndex) || (current->AudioRate != temp->AudioRate)) {
  //SoundInit(emuState->WindowHandle, soundCards[tempSoundCardIndex].Guid, temp->AudioRate);
  SoundInit(emuState->WindowHandle, soundCards[tempSoundCardIndex].Guid, current->AudioRate);
  //}
}
