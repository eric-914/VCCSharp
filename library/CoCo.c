#include "CoCo.h"

#include "defines.h"

#include "Graphics.h"
#include "MC6821.h"
#include "Cassette.h"
#include "Audio.h"

static void (*AudioEvent)(void) = AudioOut;

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

extern "C" {
  __declspec(dllexport) void __cdecl ExecuteAudioEvent() {
    AudioEvent();
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl AudioOut()
  {
    instance->AudioBuffer[instance->AudioIndex++] = MC6821_GetDACSample();
  }
}

extern "C" {
  __declspec(dllexport) unsigned short __cdecl SetAudioRate(unsigned short rate)
  {
    instance->CycleDrift = 0;
    instance->AudioIndex = 0;

    if (rate != 0) {	//Force Mute or 44100Hz
      rate = 44100;
    }

    if (rate == 0) {
      instance->SndEnable = 0;
      instance->SoundInterrupt = 0;
    }
    else
    {
      instance->SndEnable = 1;
      instance->SoundInterrupt = PICOSECOND / rate;
      instance->PicosToSoundSample = instance->SoundInterrupt;
    }

    instance->SoundRate = rate;

    return(0);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetLinesperScreen(unsigned char lines)
  {
    GraphicsState* graphicsState = GetGraphicsState();

    lines = (lines & 3);

    instance->LinesperScreen = graphicsState->Lpf[lines];
    instance->TopBorder = graphicsState->VcenterTable[lines];
    instance->BottomBorder = 243 - (instance->TopBorder + instance->LinesperScreen); //4 lines of top border are unrendered 244-4=240 rendered scanlines
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl CassIn()
  {
    instance->AudioBuffer[instance->AudioIndex] = MC6821_GetDACSample();

    MC6821_SetCassetteSample(instance->CassBuffer[instance->AudioIndex++]);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl CassOut()
  {
    instance->CassBuffer[instance->AudioIndex++] = MC6821_GetCasSample();
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetAudioEventAudioOut()
  {
    AudioEvent = AudioOut;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetAudioEventCassOut()
  {
    AudioEvent = CassOut;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetAudioEventCassIn()
  {
    AudioEvent = CassIn;
  }
}
