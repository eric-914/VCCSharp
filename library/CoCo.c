#include "CoCo.h"

#include "defines.h"

#include "Graphics.h"
#include "tcc1014graphics-8.h"
#include "tcc1014graphics-16.h"
#include "tcc1014graphics-24.h"
#include "tcc1014graphics-32.h"

#include "MC6821.h"
#include "Cassette.h"
#include "Keyboard.h"
#include "Clipboard.h"
#include "Throttle.h"
#include "DirectDraw.h"
#include "Audio.h"

#include "cpudef.h"

#include "PAKInterface.h"
#include "TC1014Registers.h"

#include "VccState.h"

static void (*AudioEvent)(void) = AudioOut;
static void (*DrawTopBorder[4]) (EmuState*) = { DrawTopBorder8, DrawTopBorder16, DrawTopBorder24, DrawTopBorder32 };
static void (*DrawBottomBorder[4]) (EmuState*) = { DrawBottomBorder8, DrawBottomBorder16, DrawBottomBorder24, DrawBottomBorder32 };
static void (*UpdateScreen[4]) (EmuState*) = { UpdateScreen8, UpdateScreen16, UpdateScreen24, UpdateScreen32 };

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
  __declspec(dllexport) void __cdecl CoCoDrawTopBorder(EmuState* emuState) {
    DrawTopBorder[emuState->BitDepth](emuState);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl CoCoUpdateScreen(EmuState* emuState) {
    UpdateScreen[emuState->BitDepth](emuState);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl CoCoDrawBottomBorder(EmuState* emuState) {
    DrawBottomBorder[emuState->BitDepth](emuState);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl AudioOut(void)
  {
    instance->AudioBuffer[instance->AudioIndex++] = MC6821_GetDACSample();
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetVertInterruptState(unsigned char state)
  {
    instance->VertInterruptEnabled = !!state;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetHorzInterruptState(unsigned char state)
  {
    instance->HorzInterruptEnabled = !!state;
  }
}

extern "C" {
  __declspec(dllexport) unsigned short __cdecl SetAudioRate(unsigned short rate)
  {
    instance->SndEnable = 1;
    instance->SoundInterrupt = 0;
    instance->CycleDrift = 0;
    instance->AudioIndex = 0;

    if (rate != 0) {	//Force Mute or 44100Hz
      rate = 44100;
    }

    if (rate == 0) {
      instance->SndEnable = 0;
    }
    else
    {
      instance->SoundInterrupt = PICOSECOND / rate;
      instance->PicosToSoundSample = instance->SoundInterrupt;
    }

    instance->SoundRate = rate;

    return(0);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetMasterTickCounter(void)
  {
    double Rate[2] = { PICOSECOND / (TARGETFRAMERATE * LINESPERFIELD), PICOSECOND / COLORBURST };

    if (instance->UnxlatedTickCounter == 0) {
      instance->MasterTickCounter = 0;
    }
    else {
      instance->MasterTickCounter = (instance->UnxlatedTickCounter + 2) * Rate[instance->TimerClockRate];
    }

    if (instance->MasterTickCounter != instance->OldMaster)
    {
      instance->OldMaster = instance->MasterTickCounter;
      instance->PicosToInterrupt = instance->MasterTickCounter;
    }

    instance->IntEnable = instance->MasterTickCounter == 0 ? 0 : 1;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetTimerInterruptState(unsigned char state)
  {
    instance->TimerInterruptEnabled = state;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetInterruptTimer(unsigned short timer)
  {
    instance->UnxlatedTickCounter = (timer & 0xFFF);

    SetMasterTickCounter();
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetTimerClockRate(unsigned char clockRate)
  {
    //1= 279.265nS (1/ColorBurst)
    //0= 63.695uS  (1/60*262)  1 scanline time

    instance->TimerClockRate = !!clockRate;

    SetMasterTickCounter();
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
  __declspec(dllexport) void __cdecl CassIn(void)
  {
    instance->AudioBuffer[instance->AudioIndex] = MC6821_GetDACSample();

    MC6821_SetCassetteSample(instance->CassBuffer[instance->AudioIndex++]);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl CassOut(void)
  {
    instance->CassBuffer[instance->AudioIndex++] = MC6821_GetCasSample();
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl SetSndOutMode(unsigned char mode)  //0 = Speaker 1= Cassette Out 2=Cassette In
  {
    static unsigned char lastMode = 0;
    static unsigned short primarySoundRate = instance->SoundRate;

    switch (mode)
    {
    case 0:
      if (lastMode == 1) {	//Send the last bits to be encoded
        FlushCassetteBuffer(instance->CassBuffer, instance->AudioIndex); /* Cassette.cpp */
      }

      AudioEvent = AudioOut;

      SetAudioRate(primarySoundRate);

      break;

    case 1:
      AudioEvent = CassOut;

      primarySoundRate = instance->SoundRate;

      SetAudioRate(TAPEAUDIORATE);

      break;

    case 2:
      AudioEvent = CassIn;

      primarySoundRate = instance->SoundRate;

      SetAudioRate(TAPEAUDIORATE);

      break;

    default:	//QUERY
      return(instance->SoundOutputMode);
      break;
    }

    if (mode != lastMode)
    {
      instance->AudioIndex = 0;	//Reset Buffer on true mode switch
      lastMode = mode;
    }

    instance->SoundOutputMode = mode;

    return(instance->SoundOutputMode);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl CPUCyclePicos(VccState* vccState) {
    CPU* cpu = GetCPU();

    instance->StateSwitch = 0;

    if ((instance->PicosToInterrupt <= instance->PicosThisLine) && instance->IntEnable) {	//Does this iteration need to Timer Interrupt
      instance->StateSwitch = 1;
    }

    if ((instance->PicosToSoundSample <= instance->PicosThisLine) && instance->SndEnable) { //Does it need to collect an Audio sample
      instance->StateSwitch += 2;
    }

    switch (instance->StateSwitch)
    {
    case 0:		//No interrupts this line
      instance->CyclesThisLine = instance->CycleDrift + (instance->PicosThisLine * instance->CyclesPerLine * instance->OverClock / instance->PicosPerLine);

      if (instance->CyclesThisLine >= 1) {	//Avoid un-needed CPU engine calls
        instance->CycleDrift = cpu->CPUExec((int)floor(instance->CyclesThisLine)) + (instance->CyclesThisLine - floor(instance->CyclesThisLine));
      }
      else {
        instance->CycleDrift = instance->CyclesThisLine;
      }

      instance->PicosToInterrupt -= instance->PicosThisLine;
      instance->PicosToSoundSample -= instance->PicosThisLine;
      instance->PicosThisLine = 0;

      break;

    case 1:		//Only Interrupting
      instance->PicosThisLine -= instance->PicosToInterrupt;
      instance->CyclesThisLine = instance->CycleDrift + (instance->PicosToInterrupt * instance->CyclesPerLine * instance->OverClock / instance->PicosPerLine);

      if (instance->CyclesThisLine >= 1) {
        instance->CycleDrift = cpu->CPUExec((int)floor(instance->CyclesThisLine)) + (instance->CyclesThisLine - floor(instance->CyclesThisLine));
      }
      else {
        instance->CycleDrift = instance->CyclesThisLine;
      }

      GimeAssertTimerInterrupt();

      instance->PicosToSoundSample -= instance->PicosToInterrupt;
      instance->PicosToInterrupt = instance->MasterTickCounter;

      break;

    case 2:		//Only Sampling
      instance->PicosThisLine -= instance->PicosToSoundSample;
      instance->CyclesThisLine = instance->CycleDrift + (instance->PicosToSoundSample * instance->CyclesPerLine * instance->OverClock / instance->PicosPerLine);

      if (instance->CyclesThisLine >= 1) {
        instance->CycleDrift = cpu->CPUExec((int)floor(instance->CyclesThisLine)) + (instance->CyclesThisLine - floor(instance->CyclesThisLine));
      }
      else {
        instance->CycleDrift = instance->CyclesThisLine;
      }

      AudioEvent();

      instance->PicosToInterrupt -= instance->PicosToSoundSample;
      instance->PicosToSoundSample = instance->SoundInterrupt;

      break;

    case 3:		//Interrupting and Sampling
      if (instance->PicosToSoundSample < instance->PicosToInterrupt)
      {
        instance->PicosThisLine -= instance->PicosToSoundSample;
        instance->CyclesThisLine = instance->CycleDrift + (instance->PicosToSoundSample * instance->CyclesPerLine * instance->OverClock / instance->PicosPerLine);

        if (instance->CyclesThisLine >= 1) {
          instance->CycleDrift = cpu->CPUExec((int)floor(instance->CyclesThisLine)) + (instance->CyclesThisLine - floor(instance->CyclesThisLine));
        }
        else {
          instance->CycleDrift = instance->CyclesThisLine;
        }

        AudioEvent();

        instance->PicosToInterrupt -= instance->PicosToSoundSample;
        instance->PicosToSoundSample = instance->SoundInterrupt;
        instance->PicosThisLine -= instance->PicosToInterrupt;

        instance->CyclesThisLine = instance->CycleDrift + (instance->PicosToInterrupt * instance->CyclesPerLine * instance->OverClock / instance->PicosPerLine);

        if (instance->CyclesThisLine >= 1) {
          instance->CycleDrift = cpu->CPUExec((int)floor(instance->CyclesThisLine)) + (instance->CyclesThisLine - floor(instance->CyclesThisLine));
        }
        else {
          instance->CycleDrift = instance->CyclesThisLine;
        }

        GimeAssertTimerInterrupt();

        instance->PicosToSoundSample -= instance->PicosToInterrupt;
        instance->PicosToInterrupt = instance->MasterTickCounter;

        break;
      }

      if (instance->PicosToSoundSample > instance->PicosToInterrupt)
      {
        instance->PicosThisLine -= instance->PicosToInterrupt;
        instance->CyclesThisLine = instance->CycleDrift + (instance->PicosToInterrupt * instance->CyclesPerLine * instance->OverClock / instance->PicosPerLine);

        if (instance->CyclesThisLine >= 1) {
          instance->CycleDrift = cpu->CPUExec((int)floor(instance->CyclesThisLine)) + (instance->CyclesThisLine - floor(instance->CyclesThisLine));
        }
        else {
          instance->CycleDrift = instance->CyclesThisLine;
        }

        GimeAssertTimerInterrupt();

        instance->PicosToSoundSample -= instance->PicosToInterrupt;
        instance->PicosToInterrupt = instance->MasterTickCounter;
        instance->PicosThisLine -= instance->PicosToSoundSample;
        instance->CyclesThisLine = instance->CycleDrift + (instance->PicosToSoundSample * instance->CyclesPerLine * instance->OverClock / instance->PicosPerLine);

        if (instance->CyclesThisLine >= 1) {
          instance->CycleDrift = cpu->CPUExec((int)floor(instance->CyclesThisLine)) + (instance->CyclesThisLine - floor(instance->CyclesThisLine));
        }
        else {
          instance->CycleDrift = instance->CyclesThisLine;
        }

        AudioEvent();

        instance->PicosToInterrupt -= instance->PicosToSoundSample;
        instance->PicosToSoundSample = instance->SoundInterrupt;

        break;
      }

      //They are the same (rare)
      instance->PicosThisLine -= instance->PicosToInterrupt;
      instance->CyclesThisLine = instance->CycleDrift + (instance->PicosToSoundSample * instance->CyclesPerLine * instance->OverClock / instance->PicosPerLine);

      if (instance->CyclesThisLine > 1) {
        instance->CycleDrift = cpu->CPUExec((int)floor(instance->CyclesThisLine)) + (instance->CyclesThisLine - floor(instance->CyclesThisLine));
      }
      else {
        instance->CycleDrift = instance->CyclesThisLine;
      }

      GimeAssertTimerInterrupt();

      AudioEvent();

      instance->PicosToInterrupt = instance->MasterTickCounter;
      instance->PicosToSoundSample = instance->SoundInterrupt;
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl ResetKeyMap() {
    vccKeyboardBuildRuntimeTable((keyboardlayout_e)GetCurrentKeyMap());
  }
}
