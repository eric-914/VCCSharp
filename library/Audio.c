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
  p->CardCount = 0;
  p->CurrentRate = 0;
  p->InitPassed = 0;
  p->SndBuffLength = 0;
  p->SndLength1 = 0;
  p->SndLength2 = 0;
  p->WritePointer = 0;

  p->Cards = NULL;
  p->SndPointer1 = NULL;
  p->SndPointer2 = NULL;

  STRARRAYCOPY(RateList);
  ARRAYCOPY(iRateList);

  return p;
}

extern "C" {
  __declspec(dllexport) unsigned short __cdecl GetSoundStatus(void)
  {
    return(instance->CurrentRate);
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl PauseAudio(unsigned char pause)
  {
    DirectSoundState* directSoundState = GetDirectSoundState();

    instance->AudioPause = pause;

    if (instance->InitPassed)
    {
      if (instance->AudioPause == 1) {
        instance->hr = directSoundState->lpdsbuffer1->Stop();
      }
      else {
        instance->hr = directSoundState->lpdsbuffer1->Play(0, 0, DSBPLAY_LOOPING);
      }
    }

    return(instance->AudioPause);
  }
}

extern "C" {
  __declspec(dllexport) const char* __cdecl GetRateList(unsigned char index) {
    return instance->RateList[index];
  }
}

extern "C" {
  __declspec(dllexport) int __cdecl GetFreeBlockCount() //return 0 on full buffer
  {
    unsigned long writeCursor = 0, playCursor = 0;
    long retVal = 0, maxSize = 0;

    DirectSoundState* directSoundState = GetDirectSoundState();

    if ((!instance->InitPassed) || (instance->AudioPause)) {
      return(AUDIOBUFFERS);
    }

    retVal = directSoundState->lpdsbuffer1->GetCurrentPosition(&playCursor, &writeCursor);

    if (instance->BuffOffset <= playCursor) {
      maxSize = playCursor - instance->BuffOffset;
    }
    else {
      maxSize = instance->SndBuffLength - instance->BuffOffset + playCursor;
    }

    return(maxSize / instance->BlockSize);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl HandleSlowAudio(unsigned char* Abuffer2, unsigned short length) {
    memcpy(instance->AuxBuffer[instance->AuxBufferPointer], Abuffer2, length);	//Saving buffer to aux stack

    instance->AuxBufferPointer++;		  //and chase your own tail
    instance->AuxBufferPointer %= 5;	//At this point we are so far behind we may as well drop the buffer
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl PurgeAuxBuffer()
  {
    DirectSoundState* directSoundState = GetDirectSoundState();

    if ((!instance->InitPassed) || (instance->AudioPause)) {
      return;
    }

    return; //TODO: Why?

    instance->AuxBufferPointer--;			//Normally points to next free block Point to last used block

    if (instance->AuxBufferPointer >= 0)	//zero is a valid data block
    {
      while ((GetFreeBlockCount() <= 0)) {};

      instance->hr = directSoundState->lpdsbuffer1->Lock(instance->BuffOffset, instance->BlockSize, &(instance->SndPointer1), &(instance->SndLength1), &(instance->SndPointer2), &(instance->SndLength2), 0);

      if (instance->hr != DS_OK) {
        return;
      }

      memcpy(instance->SndPointer1, instance->AuxBuffer[instance->AuxBufferPointer], instance->SndLength1);

      if (instance->SndPointer2 != NULL) {
        memcpy(instance->SndPointer2, (instance->AuxBuffer[instance->AuxBufferPointer] + (instance->SndLength1 >> 2)), instance->SndLength2);
      }

      instance->BuffOffset = (instance->BuffOffset + instance->BlockSize) % instance->SndBuffLength;

      instance->hr = directSoundState->lpdsbuffer1->Unlock(instance->SndPointer1, instance->SndLength1, instance->SndPointer2, instance->SndLength2);

      instance->AuxBufferPointer--;
    }

    instance->AuxBufferPointer = 0;
  }
}

extern "C" {
  __declspec(dllexport) int __cdecl SoundInit(HWND hWnd, GUID* guid, unsigned short rate)
  {
    DirectSoundState* directSoundState = GetDirectSoundState();

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
