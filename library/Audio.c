#include "AudioState.h"
#include "DirectSound.h"

#include "Config.h"
#include "Coco.h"
#include "DirectSoundEnumerateCallback.h"

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
  __declspec(dllexport) void __cdecl FlushAudioBuffer(unsigned int* buffer, unsigned short length)
  {
    unsigned short index = 0;
    unsigned char flag = 0;
    unsigned char* byteBuffer = (unsigned char*)buffer;

    if (GetFreeBlockCount() <= 0)	//this should only kick in when frame skipping or unthrottled
    {
      HandleSlowAudio(byteBuffer, length);
      return;
    }

    instance->hr = DirectSoundLock(instance->BuffOffset, length, &(instance->SndPointer1), &(instance->SndLength1), &(instance->SndPointer2), &(instance->SndLength2));

    if (instance->hr != DS_OK) {
      return;
    }

    memcpy(instance->SndPointer1, byteBuffer, instance->SndLength1);	// copy first section of circular buffer

    if (instance->SndPointer2 != NULL) { // copy last section of circular buffer if wrapped
      memcpy(instance->SndPointer2, byteBuffer + instance->SndLength1, instance->SndLength2);
    }

    instance->hr = DirectSoundUnlock(instance->SndPointer1, instance->SndLength1, instance->SndPointer2, instance->SndLength2);// unlock the buffer

    instance->BuffOffset = (instance->BuffOffset + length) % instance->SndBuffLength;	//Where to write next
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl PurgeAuxBuffer(void)
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
  __declspec(dllexport) int __cdecl GetSoundCardList(SoundCardList* list)
  {
    instance->CardCount = 0;
    instance->Cards = list;

    DirectSoundEnumerate(DirectSoundEnumerateCallback, NULL);

    return(instance->CardCount);
  }
}

extern "C" {
  __declspec(dllexport) int __cdecl SoundInit(HWND hWnd, _GUID* guid, unsigned short rate)
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
      directSoundState->lpdsbuffer1->Stop();

      if (directSoundState->lpdsbuffer1 != NULL)
      {
        instance->hr = directSoundState->lpdsbuffer1->Release();
        directSoundState->lpdsbuffer1 = NULL;
      }

      if (directSoundState->lpds != NULL)
      {
        instance->hr = directSoundState->lpds->Release();
        directSoundState->lpds = NULL;
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
      instance->hr = DirectSoundCreate(guid, &(directSoundState->lpds), NULL);	// create a directsound object

      if (instance->hr != DS_OK) {
        return(1);
      }

      instance->hr = directSoundState->lpds->SetCooperativeLevel(hWnd, DSSCL_NORMAL); // set cooperation level normal DSSCL_EXCLUSIVE

      if (instance->hr != DS_OK) {
        return(1);
      }

      // set up the format data structure
      memset(&(directSoundState->pcmwf), 0, sizeof(WAVEFORMATEX));
      directSoundState->pcmwf.wFormatTag = WAVE_FORMAT_PCM;
      directSoundState->pcmwf.nChannels = 2;
      directSoundState->pcmwf.nSamplesPerSec = instance->BitRate;
      directSoundState->pcmwf.wBitsPerSample = 16;
      directSoundState->pcmwf.nBlockAlign = (directSoundState->pcmwf.wBitsPerSample * directSoundState->pcmwf.nChannels) >> 3;
      directSoundState->pcmwf.nAvgBytesPerSec = directSoundState->pcmwf.nSamplesPerSec * directSoundState->pcmwf.nBlockAlign;
      directSoundState->pcmwf.cbSize = 0;

      // create the secondary buffer 
      memset(&(directSoundState->dsbd), 0, sizeof(DSBUFFERDESC));
      directSoundState->dsbd.dwSize = sizeof(DSBUFFERDESC);
      directSoundState->dsbd.dwFlags = DSBCAPS_GETCURRENTPOSITION2 | DSBCAPS_LOCSOFTWARE | DSBCAPS_STATIC | DSBCAPS_GLOBALFOCUS;
      directSoundState->dsbd.dwBufferBytes = instance->SndBuffLength;
      directSoundState->dsbd.lpwfxFormat = &(directSoundState->pcmwf);

      instance->hr = directSoundState->lpds->CreateSoundBuffer(&(directSoundState->dsbd), &(directSoundState->lpdsbuffer1), NULL);

      if (instance->hr != DS_OK) {
        return(1);
      }

      // Clear out sound buffers
      instance->hr = directSoundState->lpdsbuffer1->Lock(0, instance->SndBuffLength, &(instance->SndPointer1), &(instance->SndLength1), &(instance->SndPointer2), &(instance->SndLength2), DSBLOCK_ENTIREBUFFER);

      if (instance->hr != DS_OK) {
        return(1);
      }

      memset(instance->SndPointer1, 0, instance->SndBuffLength);
      instance->hr = directSoundState->lpdsbuffer1->Unlock(instance->SndPointer1, instance->SndLength1, instance->SndPointer2, instance->SndLength2);

      if (instance->hr != DS_OK) {
        return(1);
      }

      directSoundState->lpdsbuffer1->SetCurrentPosition(0);
      instance->hr = directSoundState->lpdsbuffer1->Play(0, 0, DSBPLAY_LOOPING);	// play the sound in looping mode

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
