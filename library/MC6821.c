#include "MC6821.h"
#include "MC6821State.h"

#include "macros.h"

#include "PAKInterface.h"

const unsigned char rega[4] = { 0,0,0,0 };
const unsigned char regb[4] = { 0,0,0,0 };
const unsigned char rega_dd[4] = { 0,0,0,0 };
const unsigned char regb_dd[4] = { 0,0,0,0 };

MC6821State* InitializeInstance(MC6821State*);

static MC6821State* instance = InitializeInstance(new MC6821State());

extern "C" {
  __declspec(dllexport) MC6821State* __cdecl GetMC6821State() {
    return instance;
  }
}

MC6821State* InitializeInstance(MC6821State* p) {
  p->LeftChannel = 0;
  p->RightChannel = 0;
  p->Asample = 0;
  p->Ssample = 0;
  p->Csample = 0;
  p->CartInserted = 0;
  p->CartAutoStart = 1;
  p->AddLF = 0;

  p->hPrintFile = INVALID_HANDLE_VALUE;
  p->hOut = NULL;
  p->MonState = FALSE;

  ARRAYCOPY(rega);
  ARRAYCOPY(regb);
  ARRAYCOPY(rega_dd);
  ARRAYCOPY(regb_dd);

  return p;
}

extern "C" {
  __declspec(dllexport) unsigned int __cdecl MC6821_GetDACSample()
  {
    static unsigned int retVal = 0;
    static unsigned short sampleLeft = 0, sampleRight = 0, pakSample = 0;
    static unsigned short outLeft = 0, outRight = 0;
    static unsigned short lastLeft = 0, lastRight = 0;

    pakSample = PakAudioSample();
    sampleLeft = (pakSample >> 8) + instance->Asample + instance->Ssample;
    sampleRight = (pakSample & 0xFF) + instance->Asample + instance->Ssample; //9 Bits each
    sampleLeft = sampleLeft << 6;	//Conver to 16 bit values
    sampleRight = sampleRight << 6;	//For Max volume

    if (sampleLeft == lastLeft)	//Simulate a slow high pass filter
    {
      if (outLeft) {
        outLeft--;
      }
    }
    else
    {
      outLeft = sampleLeft;
      lastLeft = sampleLeft;
    }

    if (sampleRight == lastRight)
    {
      if (outRight) {
        outRight--;
      }
    }
    else
    {
      outRight = sampleRight;
      lastRight = sampleRight;
    }

    retVal = (outLeft << 16) + (outRight);

    return(retVal);
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl MC6821_GetCasSample()
  {
    return(instance->Csample);
  }
}

extern "C" {
  __declspec(dllexport) HANDLE __cdecl MC6821_OpenPrintFile(char* filename)
  {
    return CreateFile(filename, GENERIC_READ | GENERIC_WRITE, 0, 0, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, 0);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetCart(unsigned char cart)
  {
    instance->CartInserted = cart;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl MC6821_SetCassetteSample(unsigned char sample)
  {
    instance->regb[0] = instance->regb[0] & 0xFE;

    if (sample > 0x7F) {
      instance->regb[0] = instance->regb[0] | 1;
    }
  }
}
