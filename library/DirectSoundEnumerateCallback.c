#include <windows.h>

#include "Config.h"

extern "C" {
  __declspec(dllexport) BOOL CALLBACK DirectSoundEnumerateCallback(LPGUID lpGuid, LPCSTR lpcstrDescription, LPCSTR lpcstrModule, LPVOID lpContext)
  {
    ConfigState* configState = GetConfigState();

    strncpy(configState->SoundCards[configState->NumberOfSoundCards].CardName, lpcstrDescription, 63);
    configState->SoundCards[configState->NumberOfSoundCards].Guid = lpGuid;
    configState->NumberOfSoundCards++;

    return (configState->NumberOfSoundCards < MAXCARDS);
  }
}
