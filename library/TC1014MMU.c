#include "TC1014MMU.h"
#include "PAKInterface.h"

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

/******************************************************************************************
* Following still used by PakInterfaceModule.InvokeDmaMemPointer(...) as function pointers
*******************************************************************************************/

extern "C" {
  __declspec(dllexport) unsigned char __cdecl MemRead8(unsigned short address)
  {
    if (instance->MemPageOffsets[instance->MmuRegisters[instance->MmuState][address >> 13]] == 1) {
      return(instance->MemPages[instance->MmuRegisters[instance->MmuState][address >> 13]][address & 0x1FFF]);
    }

    return(PakMem8Read(instance->MemPageOffsets[instance->MmuRegisters[instance->MmuState][address >> 13]] + (address & 0x1FFF)));
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl MemWrite8(unsigned char data, unsigned short address)
  {
    if (instance->MapType || (instance->MmuRegisters[instance->MmuState][address >> 13] < instance->VectorMaska[instance->CurrentRamConfig]) || (instance->MmuRegisters[instance->MmuState][address >> 13] > instance->VectorMask[instance->CurrentRamConfig])) {
      instance->MemPages[instance->MmuRegisters[instance->MmuState][address >> 13]][address & 0x1FFF] = data;
    }
  }
}
