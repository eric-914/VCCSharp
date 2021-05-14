#include <stdint.h>

#include "MC6821.h"

#include "macros.h"
#include "cpudef.h"
#include "defines.h"

#include "PAKInterface.h"
#include "Keyboard.h"
#include "Cassette.h"
#include "Graphics.h"

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
  __declspec(dllexport) void __cdecl MC6821_ClosePrintFile()
  {
    CloseHandle(instance->hPrintFile);

    instance->hPrintFile = INVALID_HANDLE_VALUE;

    FreeConsole();

    instance->hOut = NULL;
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl MC6821_DACState()
  {
    return (instance->regb[0] >> 2);
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl MC6821_GetCasSample()
  {
    return(instance->Csample);
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl MC6821_GetMuxState()
  {
    return (((instance->rega[1] & 8) >> 3) + ((instance->rega[3] & 8) >> 2));
  }
}

extern "C" {
  __declspec(dllexport) int __cdecl MC6821_OpenPrintFile(char* filename)
  {
    instance->hPrintFile = CreateFile(filename, GENERIC_READ | GENERIC_WRITE, 0, 0, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, 0);

    if (instance->hPrintFile == INVALID_HANDLE_VALUE) {
      return(0);
    }

    return(1);
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

extern "C" {
  __declspec(dllexport) void __cdecl MC6821_SetSerialParams(unsigned char textMode)
  {
    instance->AddLF = textMode;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl MC6821_WritePrintMon(char* data)
  {
    unsigned long dummy;

    if (instance->hOut == NULL)
    {
      AllocConsole();

      instance->hOut = GetStdHandle(STD_OUTPUT_HANDLE);

      SetConsoleTitle("Printer Monitor");
    }

    WriteConsole(instance->hOut, data, 1, &dummy, 0);

    if (data[0] == 0x0D)
    {
      data[0] = 0x0A;

      WriteConsole(instance->hOut, data, 1, &dummy, 0);
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl MC6821_CaptureBit(unsigned char sample)
  {
    unsigned long bytesMoved = 0;
    static unsigned char bitMask = 1, startWait = 1;
    static char Byte = 0;

    if (instance->hPrintFile == INVALID_HANDLE_VALUE) {
      return;
    }

    if (startWait & sample) { //Waiting for start bit
      return;
    }

    if (startWait)
    {
      startWait = 0;

      return;
    }

    if (sample) {
      Byte |= bitMask;
    }

    bitMask = bitMask << 1;

    if (!bitMask)
    {
      bitMask = 1;
      startWait = 1;

      WriteFile(instance->hPrintFile, &Byte, 1, &bytesMoved, NULL);

      if (instance->MonState) {
        MC6821_WritePrintMon(&Byte);
      }

      if ((Byte == 0x0D) & instance->AddLF)
      {
        Byte = 0x0A;

        WriteFile(instance->hPrintFile, &Byte, 1, &bytesMoved, NULL);
      }

      Byte = 0;
    }
  }
}
