#include "CoCo.h"

#include "defines.h"

#include "Graphics.h"
#include "MC6821.h"

CoCoState* InitializeInstance(CoCoState*);

static CoCoState* instance = InitializeInstance(new CoCoState());

extern "C" {
  __declspec(dllexport) CoCoState* __cdecl GetCoCoState() {
    return instance;
  }
}

CoCoState* InitializeInstance(CoCoState* p) {
  p->AudioIndex = 0;
  p->BlinkPhase = 1;
  p->BottomBorder = 0;
  p->ClipCycle = 1;
  p->CycleDrift = 0;
  p->CyclesThisLine = 0;
  p->HorzInterruptEnabled = 0;
  p->IntEnable = 0;
  p->MasterTickCounter = 0;
  p->MasterTimer = 0;
  p->OldMaster = 0;
  p->OverClock = 1;
  p->PicosThisLine = 0;
  p->PicosToInterrupt = 0;
  p->PicosToSoundSample = 0;
  p->SndEnable = 1;
  p->SoundInterrupt = 0;
  p->SoundOutputMode = 0; //Default to Speaker 1=Cassette
  p->SoundRate = 0;
  p->StateSwitch = 0;
  p->Throttle = 0;
  p->TimerClockRate = 0;
  p->TimerCycleCount = 0;
  p->TimerInterruptEnabled = 0;
  p->TopBorder = 0;
  p->UnxlatedTickCounter = 0;
  p->VertInterruptEnabled = 0;
  p->WaitCycle = 2000;

  p->CyclesPerSecord = (COLORBURST / 4) * (TARGETFRAMERATE / FRAMESPERSECORD);
  p->LinesPerSecond = (double)TARGETFRAMERATE * (double)LINESPERFIELD;
  p->PicosPerLine = PICOSECOND / p->LinesPerSecond;
  p->CyclesPerLine = p->CyclesPerSecord / p->LinesPerSecond;

  return p;
}
