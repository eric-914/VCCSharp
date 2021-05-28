#include "TC1014.h"

#include "PAKInterface.h"

#include "macros.h"

const unsigned char VectorMask[4] = { 15, 63, 63, 63 };
const unsigned char VectorMaska[4] = { 12, 60, 60, 60 };

TC1014State* InitializeInstance(TC1014State*);

static TC1014State* instance = InitializeInstance(new TC1014State());

extern "C" {
  __declspec(dllexport) TC1014State* __cdecl GetTC1014State() {
    return instance;
  }
}

TC1014State* InitializeInstance(TC1014State* p) {
  p->MmuState = 0;
  p->MapType = 0;
  p->CurrentRamConfig = 1;

  ARRAYCOPY(VectorMask);
  ARRAYCOPY(VectorMaska);

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
