#include "AudioState.h"
#include "DirectSound.h"

#include "Config.h"
#include "Coco.h"

#include "defines.h"

AudioState* InitializeInstance(AudioState*);

static AudioState* instance = InitializeInstance(new AudioState());

extern "C" {
  __declspec(dllexport) AudioState* __cdecl GetAudioState() {
    return instance;
  }
}

AudioState* InitializeInstance(AudioState* p) {
  p->AuxBufferPointer = 0;
  p->BuffOffset = 0;
  p->SndBuffLength = 0;
  p->SndLength1 = 0;
  p->SndLength2 = 0;
  p->WritePointer = 0;

  p->SndPointer1 = NULL;
  p->SndPointer2 = NULL;

  return p;
}

extern "C" {
  __declspec(dllexport) void __cdecl HandleSlowAudio(unsigned char* buffer, unsigned short length) {
    memcpy(instance->AuxBuffer[instance->AuxBufferPointer], buffer, length);	//Saving buffer to aux stack
  }
}
