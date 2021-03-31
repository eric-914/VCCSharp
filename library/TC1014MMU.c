#include <windows.h>
#include <stdio.h>

#include "TC1014MMU.h"
#include "Graphics.h"
#include "Config.h"
#include "IOBus.h"
#include "PAKInterface.h"

#include "fileoperations.h"

#include "macros.h"

const unsigned int MemConfig[4] = { 0x20000, 0x80000, 0x200000, 0x800000 };
const unsigned short RamMask[4] = { 15, 63, 255, 1023 };
const unsigned char StateSwitch[4] = { 8, 56, 56, 56 };
const unsigned char VectorMask[4] = { 15, 63, 63, 63 };
const unsigned char VectorMaska[4] = { 12, 60, 60, 60 };
const unsigned long VidMask[4] = { 0x1FFFF, 0x7FFFF, 0x1FFFFF, 0x7FFFFF };

TC1014MmuState* InitializeInstance(TC1014MmuState*);

static TC1014MmuState* instance = InitializeInstance(new TC1014MmuState());

extern "C" {
  __declspec(dllexport) TC1014MmuState* __cdecl GetTC1014MmuState() {
    return instance;
  }
}

TC1014MmuState* InitializeInstance(TC1014MmuState* p) {
  p->MmuState = 0;
  p->MmuTask = 0;
  p->MmuEnabled = 0;
  p->RamVectors = 0;
  p->RomMap = 0;
  p->MapType = 0;
  p->CurrentRamConfig = 1;
  p->MmuPrefix = 0;

  p->Memory = NULL;
  p->InternalRomBuffer = NULL;

  ARRAYCOPY(MemConfig);
  ARRAYCOPY(RamMask);
  ARRAYCOPY(StateSwitch);
  ARRAYCOPY(VectorMask);
  ARRAYCOPY(VectorMaska);
  ARRAYCOPY(VidMask);

  return p;
}

extern "C" {
  __declspec(dllexport) unsigned char* __cdecl GetInternalRomPointer(void)
  {
    return instance->InternalRomBuffer;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetMmuEnabled(unsigned char usingmmu)
  {
    instance->MmuEnabled = usingmmu;
    instance->MmuState = (!instance->MmuEnabled) << 1 | instance->MmuTask;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetMmuPrefix(unsigned char data)
  {
    instance->MmuPrefix = (data & 3) << 8;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl MemWrite16(unsigned short data, unsigned short addr)
  {
    MemWrite8(data >> 8, addr);
    MemWrite8(data & 0xFF, addr + 1);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl MemWrite32(unsigned int data, unsigned short addr)
  {
    MemWrite16(data >> 16, addr);
    MemWrite16(data & 0xFFFF, addr + 2);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl UpdateMmuArray(void)
  {
    if (instance->MapType) {
      instance->MemPages[instance->VectorMask[instance->CurrentRamConfig] - 3] = instance->Memory + (0x2000 * (instance->VectorMask[instance->CurrentRamConfig] - 3));
      instance->MemPages[instance->VectorMask[instance->CurrentRamConfig] - 2] = instance->Memory + (0x2000 * (instance->VectorMask[instance->CurrentRamConfig] - 2));
      instance->MemPages[instance->VectorMask[instance->CurrentRamConfig] - 1] = instance->Memory + (0x2000 * (instance->VectorMask[instance->CurrentRamConfig] - 1));
      instance->MemPages[instance->VectorMask[instance->CurrentRamConfig]] = instance->Memory + (0x2000 * instance->VectorMask[instance->CurrentRamConfig]);

      instance->MemPageOffsets[instance->VectorMask[instance->CurrentRamConfig] - 3] = 1;
      instance->MemPageOffsets[instance->VectorMask[instance->CurrentRamConfig] - 2] = 1;
      instance->MemPageOffsets[instance->VectorMask[instance->CurrentRamConfig] - 1] = 1;
      instance->MemPageOffsets[instance->VectorMask[instance->CurrentRamConfig]] = 1;

      return;
    }

    switch (instance->RomMap)
    {
    case 0:
    case 1:	//16K Internal 16K External
      instance->MemPages[instance->VectorMask[instance->CurrentRamConfig] - 3] = instance->InternalRomBuffer;
      instance->MemPages[instance->VectorMask[instance->CurrentRamConfig] - 2] = instance->InternalRomBuffer + 0x2000;
      instance->MemPages[instance->VectorMask[instance->CurrentRamConfig] - 1] = NULL;
      instance->MemPages[instance->VectorMask[instance->CurrentRamConfig]] = NULL;

      instance->MemPageOffsets[instance->VectorMask[instance->CurrentRamConfig] - 3] = 1;
      instance->MemPageOffsets[instance->VectorMask[instance->CurrentRamConfig] - 2] = 1;
      instance->MemPageOffsets[instance->VectorMask[instance->CurrentRamConfig] - 1] = 0;
      instance->MemPageOffsets[instance->VectorMask[instance->CurrentRamConfig]] = 0x2000;

      return;

    case 2:	// 32K Internal
      instance->MemPages[instance->VectorMask[instance->CurrentRamConfig] - 3] = instance->InternalRomBuffer;
      instance->MemPages[instance->VectorMask[instance->CurrentRamConfig] - 2] = instance->InternalRomBuffer + 0x2000;
      instance->MemPages[instance->VectorMask[instance->CurrentRamConfig] - 1] = instance->InternalRomBuffer + 0x4000;
      instance->MemPages[instance->VectorMask[instance->CurrentRamConfig]] = instance->InternalRomBuffer + 0x6000;

      instance->MemPageOffsets[instance->VectorMask[instance->CurrentRamConfig] - 3] = 1;
      instance->MemPageOffsets[instance->VectorMask[instance->CurrentRamConfig] - 2] = 1;
      instance->MemPageOffsets[instance->VectorMask[instance->CurrentRamConfig] - 1] = 1;
      instance->MemPageOffsets[instance->VectorMask[instance->CurrentRamConfig]] = 1;

      return;

    case 3:	//32K External
      instance->MemPages[instance->VectorMask[instance->CurrentRamConfig] - 1] = NULL;
      instance->MemPages[instance->VectorMask[instance->CurrentRamConfig]] = NULL;
      instance->MemPages[instance->VectorMask[instance->CurrentRamConfig] - 3] = NULL;
      instance->MemPages[instance->VectorMask[instance->CurrentRamConfig] - 2] = NULL;

      instance->MemPageOffsets[instance->VectorMask[instance->CurrentRamConfig] - 1] = 0;
      instance->MemPageOffsets[instance->VectorMask[instance->CurrentRamConfig]] = 0x2000;
      instance->MemPageOffsets[instance->VectorMask[instance->CurrentRamConfig] - 3] = 0x4000;
      instance->MemPageOffsets[instance->VectorMask[instance->CurrentRamConfig] - 2] = 0x6000;

      return;
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetVectors(unsigned char data)
  {
    instance->RamVectors = !!data; //Bit 3 of $FF90 MC3
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetMmuRegister(unsigned char Register, unsigned char data)
  {
    unsigned char bankRegister, task;

    bankRegister = Register & 7;
    task = !!(Register & 8);

    instance->MmuRegisters[task][bankRegister] = instance->MmuPrefix | (data & instance->RamMask[instance->CurrentRamConfig]); //gime.c returns what was written so I can get away with this
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetMmuTask(unsigned char task)
  {
    instance->MmuTask = task;
    instance->MmuState = (!instance->MmuEnabled) << 1 | instance->MmuTask;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetRomMap(unsigned char data)
  {
    instance->RomMap = (data & 3);

    UpdateMmuArray();
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetMapType(unsigned char type)
  {
    instance->MapType = type;

    UpdateMmuArray();
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetDistoRamBank(unsigned char data)
  {
    switch (instance->CurrentRamConfig)
    {
    case 0:	// 128K
      return;
      break;

    case 1:	//512K
      return;
      break;

    case 2:	//2048K
      SetVideoBank(data & 3);
      SetMmuPrefix(0);

      return;

    case 3:	//8192K	//No Can 3 
      SetVideoBank(data & 0x0F);
      SetMmuPrefix((data & 0x30) >> 4);

      return;
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl FreeMemory(unsigned char* target) {
    if (target != NULL) {
      free(target);
    }
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl MemRead8(unsigned short address)
  {
    if (address < 0xFE00)
    {
      if (instance->MemPageOffsets[instance->MmuRegisters[instance->MmuState][address >> 13]] == 1) {
        return(instance->MemPages[instance->MmuRegisters[instance->MmuState][address >> 13]][address & 0x1FFF]);
      }

      return(PakMem8Read(instance->MemPageOffsets[instance->MmuRegisters[instance->MmuState][address >> 13]] + (address & 0x1FFF)));
    }

    if (address > 0xFEFF) {
      return (port_read(address));
    }

    if (instance->RamVectors) { //Address must be $FE00 - $FEFF
      return(instance->Memory[(0x2000 * instance->VectorMask[instance->CurrentRamConfig]) | (address & 0x1FFF)]);
    }

    if (instance->MemPageOffsets[instance->MmuRegisters[instance->MmuState][address >> 13]] == 1) {
      return(instance->MemPages[instance->MmuRegisters[instance->MmuState][address >> 13]][address & 0x1FFF]);
    }

    return(PakMem8Read(instance->MemPageOffsets[instance->MmuRegisters[instance->MmuState][address >> 13]] + (address & 0x1FFF)));
  }
}

extern "C" {
  __declspec(dllexport) unsigned short __cdecl MemRead16(unsigned short addr)
  {
    return (MemRead8(addr) << 8 | MemRead8(addr + 1));
  }
}

extern "C" {
  __declspec(dllexport) unsigned int __cdecl MemRead32(unsigned short addr)
  {
    return MemRead16(addr) << 16 | MemRead16(addr + 2);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl MemWrite8(unsigned char data, unsigned short address)
  {
    if (address < 0xFE00)
    {
      if (instance->MapType || (instance->MmuRegisters[instance->MmuState][address >> 13] < instance->VectorMaska[instance->CurrentRamConfig]) || (instance->MmuRegisters[instance->MmuState][address >> 13] > instance->VectorMask[instance->CurrentRamConfig])) {
        instance->MemPages[instance->MmuRegisters[instance->MmuState][address >> 13]][address & 0x1FFF] = data;
      }

      return;
    }

    if (address > 0xFEFF)
    {
      port_write(data, address);

      return;
    }

    if (instance->RamVectors) { //Address must be $FE00 - $FEFF
      instance->Memory[(0x2000 * instance->VectorMask[instance->CurrentRamConfig]) | (address & 0x1FFF)] = data;
    }
    else if (instance->MapType || (instance->MmuRegisters[instance->MmuState][address >> 13] < instance->VectorMaska[instance->CurrentRamConfig]) || (instance->MmuRegisters[instance->MmuState][address >> 13] > instance->VectorMask[instance->CurrentRamConfig])) {
      instance->MemPages[instance->MmuRegisters[instance->MmuState][address >> 13]][address & 0x1FFF] = data;
    }
  }
}
