#include <windows.h>

typedef struct {
  char AuxBufferPointer;

  DWORD SndLength1;
  DWORD SndLength2;
  DWORD SndBuffLength;
  DWORD BuffOffset;

  void* SndPointer1;
  void* SndPointer2;
} AudioState;

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

  p->SndPointer1 = NULL;
  p->SndPointer2 = NULL;

  return p;
}
