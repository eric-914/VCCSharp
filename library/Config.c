#include <windows.h>

typedef struct
{
  char IniFilePath[MAX_PATH];
} ConfigState;

ConfigState* InitializeInstance(ConfigState*);

static ConfigState* instance = InitializeInstance(new ConfigState());

extern "C" {
  __declspec(dllexport) ConfigState* __cdecl GetConfigState() {
    return instance;
  }
}

ConfigState* InitializeInstance(ConfigState* p) {
  strcpy(p->IniFilePath, "");

  return p;
}
