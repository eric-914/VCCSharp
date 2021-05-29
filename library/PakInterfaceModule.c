#include <windows.h>

#include "PakInterfaceDelegates.h"

PakInterfaceDelegates* InitializeDelegates(PakInterfaceDelegates*);

static PakInterfaceDelegates* delegates = InitializeDelegates(new PakInterfaceDelegates());

extern "C" {
  __declspec(dllexport) PakInterfaceDelegates* __cdecl GetPakInterfaceDelegates() {
    return delegates;
  }
}

PakInterfaceDelegates* InitializeDelegates(PakInterfaceDelegates* p) {
  p->ConfigModule = NULL;
  p->DmaMemPointers = NULL;
  p->GetModuleName = NULL;
  p->HeartBeat = NULL;
  p->ModuleAudioSample = NULL;
  p->ModuleReset = NULL;
  p->ModuleStatus = NULL;
  p->PakMemRead8 = NULL;
  p->PakMemWrite8 = NULL;
  p->PakPortRead = NULL;
  p->PakPortWrite = NULL;
  p->PakSetCart = NULL;
  p->SetIniPath = NULL;
  p->SetInterruptCallPointer = NULL;

  return p;
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl ModulePortRead(unsigned char port) {
    return(delegates->PakPortRead(port));
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl ModuleMem8Read(unsigned short address) {
    return(delegates->PakMemRead8(address & 32767));
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl ModulePortWrite(unsigned char port, unsigned char data) {
    if (delegates->PakPortWrite != NULL)
    {
      delegates->PakPortWrite(port, data);
      return 0;
    }

    return 1;
  }
}

/*******************************************************
* Used by PAKInterface.PakMem8Read(...)
********************************************************/
extern "C" {
  __declspec(dllexport) BOOL __cdecl HasModuleMem8Read() {
    return delegates->PakMemRead8 != NULL;
  }
}

/*******************************************************
* Used by PAKInterface.PakPortRead(...)
********************************************************/
extern "C" {
  __declspec(dllexport) BOOL __cdecl HasModulePortRead() {
    return delegates->PakPortRead != NULL;
  }
}
