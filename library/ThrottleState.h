#pragma once

#include <windows.h>

typedef struct
{
  LARGE_INTEGER StartTime;
  LARGE_INTEGER EndTime;
  LARGE_INTEGER OneFrame;
  LARGE_INTEGER CurrentTime;
  LARGE_INTEGER SleepRes;
  LARGE_INTEGER TargetTime, OneMs;
  LARGE_INTEGER MasterClock;
  LARGE_INTEGER Now;

  unsigned char FrameSkip;
  float fMasterClock;
} ThrottleState;
