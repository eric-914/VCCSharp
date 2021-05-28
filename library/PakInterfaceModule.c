#include "PakInterfaceDelegates.h"
#include "cpudef.h"

#include "TC1014.h"
#include "MenuCallbacks.h"

#include "PAKInterface.h"

PakInterfaceDelegates* InitializeDelegates(PakInterfaceDelegates*);

static PakInterfaceDelegates* delegates = InitializeDelegates(new PakInterfaceDelegates());

extern "C" {
  __declspec(dllexport) PakInterfaceDelegates* __cdecl GetPakInterfaceDelegates() {
    return delegates;
  }
}

PakInterfaceDelegates* InitializeDelegates(PakInterfaceDelegates* p) {
  p->ConfigModule = NULL;
  p->DmaMemPointer = NULL;
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
  __declspec(dllexport) BOOL __cdecl SetDelegates(HINSTANCE hInstLib) {
    delegates->GetModuleName = (GETNAME)GetProcAddress(hInstLib, "ModuleName");
    delegates->ConfigModule = (CONFIGIT)GetProcAddress(hInstLib, "ModuleConfig");
    delegates->PakPortWrite = (PACKPORTWRITE)GetProcAddress(hInstLib, "PackPortWrite");
    delegates->PakPortRead = (PACKPORTREAD)GetProcAddress(hInstLib, "PackPortRead");
    delegates->SetInterruptCallPointer = (SETINTERRUPTCALLPOINTER)GetProcAddress(hInstLib, "AssertInterrupt");
    delegates->DmaMemPointer = (DMAMEMPOINTERS)GetProcAddress(hInstLib, "MemPointers");
    delegates->HeartBeat = (HEARTBEAT)GetProcAddress(hInstLib, "HeartBeat");
    delegates->PakMemWrite8 = (MEMWRITE8)GetProcAddress(hInstLib, "PakMemWrite8");
    delegates->PakMemRead8 = (MEMREAD8)GetProcAddress(hInstLib, "PakMemRead8");
    delegates->ModuleStatus = (MODULESTATUS)GetProcAddress(hInstLib, "ModuleStatus");
    delegates->ModuleAudioSample = (MODULEAUDIOSAMPLE)GetProcAddress(hInstLib, "ModuleAudioSample");
    delegates->ModuleReset = (MODULERESET)GetProcAddress(hInstLib, "ModuleReset");
    delegates->SetIniPath = (SETINIPATH)GetProcAddress(hInstLib, "SetIniPath");
    delegates->PakSetCart = (SETCARTPOINTER)GetProcAddress(hInstLib, "SetCart");

    return delegates->GetModuleName == NULL;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl UnloadModule() {
    delegates->ConfigModule = NULL;
    delegates->DmaMemPointer = NULL;
    delegates->GetModuleName = NULL;
    delegates->HeartBeat = NULL;
    delegates->ModuleAudioSample = NULL;
    delegates->ModuleReset = NULL;
    delegates->ModuleStatus = NULL;
    delegates->PakMemRead8 = NULL;
    delegates->PakMemWrite8 = NULL;
    delegates->PakPortRead = NULL;
    delegates->PakPortWrite = NULL;
    delegates->SetInterruptCallPointer = NULL;
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl HasModuleReset()
  {
    return delegates->ModuleReset != NULL;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl InvokeModuleReset()
  {
    delegates->ModuleReset();
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

extern "C" {
  __declspec(dllexport) BOOL __cdecl HasDmaMemPointer() {
    return delegates->DmaMemPointer != NULL;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl InvokeDmaMemPointer() {
    delegates->DmaMemPointer(MemRead8, MemWrite8);
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl HasSetInterruptCallPointer() {
    return delegates->SetInterruptCallPointer != NULL;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl InvokeSetInterruptCallPointer() {
    delegates->SetInterruptCallPointer(CPUAssertInterrupt);
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl HasModuleAudioSample() {
    return delegates->ModuleAudioSample != NULL;
  }
}

extern "C" {
  __declspec(dllexport) unsigned short __cdecl ReadModuleAudioSample() {
    return delegates->ModuleAudioSample();
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl HasModuleStatus() {
    return delegates->ModuleStatus != NULL;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl InvokeModuleStatus(char* statusLine) {
    delegates->ModuleStatus(statusLine);
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl HasModulePortRead() {
    return delegates->PakPortRead != NULL;
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl ModulePortRead(unsigned char port) {
    return(delegates->PakPortRead(port));
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl HasModuleMem8Read() {
    return delegates->PakMemRead8 != NULL;
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl ModuleMem8Read(unsigned short address) {
    return(delegates->PakMemRead8(address & 32767));
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl InvokeGetModuleName(char* modName, char* catNumber) {
    delegates->GetModuleName(modName, catNumber, DynamicMenuCallback);  //Instantiate the menus from HERE!
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl HasConfigModule() {
    return delegates->ConfigModule != NULL;
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl HasPakPortWrite() {
    return delegates->PakPortWrite != NULL;
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl HasPakPortRead() {
    return delegates->PakPortRead != NULL;
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl HasHeartBeat() {
    return delegates->HeartBeat != NULL;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl InvokeHeartBeat() {
    delegates->HeartBeat();
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl HasPakMemWrite8() {
    return delegates->PakMemWrite8 != NULL;
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl HasPakMemRead8() {
    return delegates->PakMemRead8 != NULL;
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl HasPakSetCart() {
    return delegates->PakSetCart != NULL;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl InvokePakSetCart() {
    delegates->PakSetCart(SetCart);
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl HasSetIniPath() {
    return delegates->SetIniPath != NULL;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl InvokeSetIniPath(char* ini) {
    delegates->SetIniPath(ini);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl InvokeConfigModule(unsigned char menuItem) {
    delegates->ConfigModule(menuItem);
  }
}
