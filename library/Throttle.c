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
