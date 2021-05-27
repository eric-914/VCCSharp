#include <windows.h>

#include "Config.h"

ConfigState* InitializeInstance(ConfigState*);

static ConfigState* instance = InitializeInstance(new ConfigState());
static ConfigModel* model = new ConfigModel();

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

ConfigState* InitializeInstance(ConfigState* p) {
  strcpy(p->IniFilePath, "");

  return p;
}
