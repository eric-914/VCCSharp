#include <windows.h>

typedef unsigned char (*PAKMEMREAD8)(unsigned short);
typedef void (*PAKMEMWRITE8)(unsigned char, unsigned short);

typedef unsigned char (*PAKPORTREAD)(unsigned char);
typedef void (*PAKPORTWRITE)(unsigned char, unsigned char);

typedef unsigned short (*MODULEAUDIOSAMPLE)(void);

typedef void (*ASSERTINTERRUPT) (unsigned char, unsigned char);
typedef void (*CONFIGMODULE)(unsigned char);
typedef void (*DMAMEMPOINTERS) (PAKMEMREAD8, PAKMEMWRITE8);
typedef void (*DYNAMICMENUCALLBACK)(char*, int, int);
typedef void (*GETMODULENAME)(char*, char*, DYNAMICMENUCALLBACK);
typedef void (*HEARTBEAT) (void);
typedef void (*MODULERESET)(void);
typedef void (*MODULESTATUS)(char*);
typedef void (*SETCART)(unsigned char);
typedef void (*PAKSETCART)(SETCART);
typedef void (*SETINIPATH)(char*);
typedef void (*SETINTERRUPTCALLPOINTER) (ASSERTINTERRUPT);

typedef struct {
  unsigned char (*PakMemRead8)(unsigned short);
  void (*PakMemWrite8)(unsigned char, unsigned short);

  unsigned char (*PakPortRead)(unsigned char);
  void (*PakPortWrite)(unsigned char, unsigned char);

  unsigned short (*ModuleAudioSample)(void);

  void (*ConfigModule)(unsigned char);
  void (*DmaMemPointers) (PAKMEMREAD8, PAKMEMWRITE8);
  void (*GetModuleName)(char*, char*, DYNAMICMENUCALLBACK);
  void (*HeartBeat)(void);
  void (*ModuleReset) (void);
  void (*ModuleStatus)(char*);
  void (*PakSetCart)(SETCART);
  void (*SetIniPath) (char*);
  void (*SetInterruptCallPointer) (ASSERTINTERRUPT);
} PakInterfaceDelegates;

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
