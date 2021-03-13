#include "Throttle.h"
#include "Audio.h"

#include "defines.h"

ThrottleState* InitializeInstance(ThrottleState*);

static ThrottleState* instance = InitializeInstance(new ThrottleState());

extern "C" {
  __declspec(dllexport) ThrottleState* __cdecl GetThrottleState() {
    return instance;
  }
}

ThrottleState* InitializeInstance(ThrottleState* p) {
  p->FrameSkip = 0;
  p->fMasterClock = 0;

  return p;
}

extern "C" {
  __declspec(dllexport) void __cdecl CalibrateThrottle(void)
  {
    timeBeginPeriod(1);	//Needed to get max resolution from the timer normally its 10Ms
    QueryPerformanceFrequency(&(instance->MasterClock));
    timeEndPeriod(1);

    instance->OneFrame.QuadPart = instance->MasterClock.QuadPart / (TARGETFRAMERATE);
    instance->OneMs.QuadPart = instance->MasterClock.QuadPart / 1000;
    instance->fMasterClock = (float)(instance->MasterClock.QuadPart);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl FrameWait(void)
  {
    QueryPerformanceCounter(&(instance->CurrentTime));

    while ((instance->TargetTime.QuadPart - instance->CurrentTime.QuadPart) > (instance->OneMs.QuadPart * 2))	//If we have more that 2Ms till the end of the frame
    {
      Sleep(1);	//Give about 1Ms back to the system
      QueryPerformanceCounter(&(instance->CurrentTime));	//And check again
    }

    if (GetSoundStatus())	//Lean on the sound card a bit for timing
    {
      PurgeAuxBuffer();

      if (instance->FrameSkip == 1)
      {
        if (GetFreeBlockCount() > AUDIOBUFFERS / 2) {	//Dont let the buffer get lest that half full
          return;
        }

        while (GetFreeBlockCount() < 1);	// Dont let it fill up either
      }
    }

    while (instance->CurrentTime.QuadPart < instance->TargetTime.QuadPart) {	//Poll Untill frame end.
      QueryPerformanceCounter(&(instance->CurrentTime));
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl StartRender()
  {
    QueryPerformanceCounter(&(instance->StartTime));
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl EndRender(unsigned char skip)
  {
    instance->FrameSkip = skip;
    instance->TargetTime.QuadPart = (instance->StartTime.QuadPart + (instance->OneFrame.QuadPart * instance->FrameSkip));
  }
}
