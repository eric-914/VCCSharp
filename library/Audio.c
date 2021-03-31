#include "AudioState.h"
#include "DirectSound.h"

#include "Config.h"
#include "Coco.h"

#include "defines.h"
#include "macros.h"

const char RateList[4][7] = { "Mute", "11025", "22050", "44100" };
const unsigned short iRateList[4] = { 0, 11025, 22050, 44100 };

AudioState* InitializeInstance(AudioState*);

static AudioState* instance = InitializeInstance(new AudioState());

extern "C" {
  __declspec(dllexport) AudioState* __cdecl GetAudioState() {
    return instance;
  }
}

AudioState* InitializeInstance(AudioState* p) {
  p->AudioPause = 0;
  p->AuxBufferPointer = 0;
  p->BitRate = 0;
  p->BlockSize = 0;
  p->BuffOffset = 0;
  p->CurrentRate = 0;
  p->InitPassed = 0;
  p->SndBuffLength = 0;
  p->SndLength1 = 0;
  p->SndLength2 = 0;
  p->WritePointer = 0;

  p->SndPointer1 = NULL;
  p->SndPointer2 = NULL;

  STRARRAYCOPY(RateList);
  ARRAYCOPY(iRateList);

  return p;
}

/*****************************************************************
* TODO: This has been ported, but is still being used by ConfigDialogCallbacks.CheckAudioChange(...)
******************************************************************/
extern "C" {
  __declspec(dllexport) int __cdecl SoundInit(HWND hWnd, GUID* guid, unsigned short rate)
  {
    rate = (rate & 3);

    if (rate != 0) {	//Force 44100 or Mute
      rate = 3;
    }

    instance->CurrentRate = rate;

    if (instance->InitPassed)
    {
      instance->InitPassed = 0;
      DirectSoundStop();

      if (DirectSoundHasBuffer() == TRUE)
      {
        instance->hr = DirectSoundBufferRelease();
      }

      if (DirectSoundHasInterface())
      {
        instance->hr = DirectSoundInterfaceRelease();
      }
    }

    instance->SndLength1 = 0;
    instance->SndLength2 = 0;
    instance->BuffOffset = 0;
    instance->AuxBufferPointer = 0;
    instance->BitRate = instance->iRateList[rate];
    instance->BlockSize = instance->BitRate * 4 / TARGETFRAMERATE;
    instance->SndBuffLength = (instance->BlockSize * AUDIOBUFFERS);

    if (rate)
    {
      instance->hr = DirectSoundInitialize(guid);	// create a directsound object

      if (instance->hr != DS_OK) {
        return(1);
      }

      instance->hr = DirectSoundSetCooperativeLevel(hWnd);

      if (instance->hr != DS_OK) {
        return(1);
      }

      DirectSoundSetupFormatDataStructure(instance->BitRate);

      DirectSoundSetupSecondaryBuffer(instance->SndBuffLength);

      instance->hr = DirectSoundCreateSoundBuffer();

      if (instance->hr != DS_OK) {
        return(1);
      }

      // Clear out sound buffers
      instance->hr = DirectSoundLock(0, (unsigned short)instance->SndBuffLength, &(instance->SndPointer1), &(instance->SndLength1), &(instance->SndPointer2), &(instance->SndLength2));

      if (instance->hr != DS_OK) {
        return(1);
      }

      memset(instance->SndPointer1, 0, instance->SndBuffLength);
      instance->hr = DirectSoundUnlock(instance->SndPointer1, instance->SndLength1, instance->SndPointer2, instance->SndLength2);

      if (instance->hr != DS_OK) {
        return(1);
      }

      DirectSoundSetCurrentPosition(0);
      instance->hr = DirectSoundPlay();

      if (instance->hr != DS_OK) {
        return(1);
      }

      instance->InitPassed = 1;
      instance->AudioPause = 0;
    }

    SetAudioRate(instance->iRateList[rate]);

    return(0);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl HandleSlowAudio(unsigned char* buffer, unsigned short length) {
    memcpy(instance->AuxBuffer[instance->AuxBufferPointer], buffer, length);	//Saving buffer to aux stack
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl PurgeAuxBuffer()
  {
    memcpy(instance->SndPointer1, instance->AuxBuffer[instance->AuxBufferPointer], instance->SndLength1);

    if (instance->SndPointer2 != NULL) {
      memcpy(instance->SndPointer2, (instance->AuxBuffer[instance->AuxBufferPointer] + (instance->SndLength1 >> 2)), instance->SndLength2);
    }
  }
}
