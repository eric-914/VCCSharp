#pragma once

#include "systemstate.h"

typedef struct
{
  double SoundInterrupt;
  double PicosToSoundSample;
  double CycleDrift;
  double CyclesThisLine;
  double PicosToInterrupt;
  double OldMaster;
  double MasterTickCounter;
  double UnxlatedTickCounter;
  double PicosThisLine;
  double CyclesPerSecord;
  double LinesPerSecond;
  double PicosPerLine;
  double CyclesPerLine;

  unsigned char SoundOutputMode;
  unsigned char HorzInterruptEnabled;
  unsigned char VertInterruptEnabled;
  unsigned char TopBorder;
  unsigned char BottomBorder;
  unsigned char LinesperScreen;
  unsigned char TimerInterruptEnabled;
  unsigned char BlinkPhase;

  unsigned short TimerClockRate;
  unsigned short SoundRate;
  unsigned short AudioIndex;

  unsigned int StateSwitch;

  int MasterTimer;
  int TimerCycleCount;
  int ClipCycle;
  int WaitCycle;
  int IntEnable;
  int SndEnable;
  int OverClock;

  char Throttle;

  unsigned int AudioBuffer[16384];
  unsigned char CassBuffer[8192];

  void (*AudioEvent)(void);
  void (*DrawTopBorder[4]) (SystemState*);
  void (*DrawBottomBorder[4]) (SystemState*);
  void (*UpdateScreen[4]) (SystemState*);
} CoCoState;
