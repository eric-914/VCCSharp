#include <windows.h>

#include "Audio.h"

extern "C" {
  __declspec(dllexport) BOOL CALLBACK DirectSoundEnumerateCallback(LPGUID lpGuid, LPCSTR lpcstrDescription, LPCSTR lpcstrModule, LPVOID lpContext)
  {
    AudioState* audioState = GetAudioState();

    strncpy(audioState->Cards[audioState->CardCount].CardName, lpcstrDescription, 63);
    audioState->Cards[audioState->CardCount++].Guid = lpGuid;

    return (audioState->CardCount < MAXCARDS);
  }
}
