#include <iostream>
#include <string>

#include "PAKInterface.h"
#include "MC6821.h"
#include "Config.h"
#include "TC1014MMU.h"
#include "EmuState.h"
#include "cpudef.h"
#include "fileoperations.h"
#include "MenuCallbacks.h"

using namespace std;

PakInterfaceState* InitializeInstance(PakInterfaceState*);
PakInterfaceDelegates* InitializeDelegates(PakInterfaceDelegates*);

static PakInterfaceState* instance = InitializeInstance(new PakInterfaceState());
static PakInterfaceDelegates* delegates = InitializeDelegates(new PakInterfaceDelegates());

extern "C" {
  __declspec(dllexport) PakInterfaceState* __cdecl GetPakInterfaceState() {
    return instance;
  }
}

extern "C" {
  __declspec(dllexport) PakInterfaceDelegates* __cdecl GetPakInterfaceDelegates() {
    return delegates;
  }
}

PakInterfaceState* InitializeInstance(PakInterfaceState* p) {
  p->BankedCartOffset = 0;
  p->ModualParms = 0;
  p->DialogOpen = false;
  p->RomPackLoaded = false;

  strcpy(p->DllPath, "");
  strcpy(p->Modname, "Blank");

  p->ExternalRomBuffer = nullptr;

  return p;
}

PakInterfaceDelegates* InitializeDelegates(PakInterfaceDelegates* p) {
  p->GetModuleName = NULL;
  p->ConfigModule = NULL;
  p->SetInterruptCallPointer = NULL;
  p->DmaMemPointer = NULL;
  p->HeartBeat = NULL;
  p->PakPortWrite = NULL;
  p->PakPortRead = NULL;
  p->PakMemWrite8 = NULL;
  p->PakMemRead8 = NULL;
  p->ModuleStatus = NULL;
  p->ModuleAudioSample = NULL;
  p->ModuleReset = NULL;
  p->SetIniPath = NULL;
  p->PakSetCart = NULL;

  return p;
}

extern "C" {
  __declspec(dllexport) void __cdecl GetCurrentModule(char* defaultModule)
  {
    strcpy(defaultModule, instance->DllPath);
  }
}

extern "C" {
  __declspec(dllexport) int __cdecl FileID(char* filename)
  {
    FILE* handle = NULL;
    char temp[3] = "";

    handle = fopen(filename, "rb");

    if (handle == NULL) {
      return(0);	//File Doesn't exist
    }

    temp[0] = fgetc(handle);
    temp[1] = fgetc(handle);
    temp[2] = 0;
    fclose(handle);

    if (strcmp(temp, "MZ") == 0) {
      return(1);	//DLL File
    }

    return(2);		//Rom Image 
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl PakTimer()
  {
    if (delegates->HeartBeat != NULL) {
      delegates->HeartBeat();
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl ResetBus()
  {
    instance->BankedCartOffset = 0;

    if (delegates->ModuleReset != NULL) {
      delegates->ModuleReset();
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl GetModuleStatus(EmuState* emuState)
  {
    if (delegates->ModuleStatus != NULL) {
      delegates->ModuleStatus(emuState->StatusLine);
    }
    else {
      sprintf(emuState->StatusLine, "");
    }
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl PakPortRead(unsigned char port)
  {
    if (delegates->PakPortRead != NULL) {
      return(delegates->PakPortRead(port));
    }
    else {
      return(NULL);
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl PakPortWrite(unsigned char port, unsigned char data)
  {
    if (delegates->PakPortWrite != NULL)
    {
      delegates->PakPortWrite(port, data);
      return;
    }

    if ((port == 0x40) && (instance->RomPackLoaded == true)) {
      instance->BankedCartOffset = (data & 15) << 14;
    }
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl PakMem8Read(unsigned short address)
  {
    if (delegates->PakMemRead8 != NULL) {
      return(delegates->PakMemRead8(address & 32767));
    }

    if (instance->ExternalRomBuffer != NULL) {
      return(instance->ExternalRomBuffer[(address & 32767) + instance->BankedCartOffset]);
    }

    return(0);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl PakMem8Write(unsigned char port, unsigned char data)
  {
    //TODO: This really is empty
  }
}

extern "C" {
  __declspec(dllexport) unsigned short __cdecl PakAudioSample(void)
  {
    if (delegates->ModuleAudioSample != NULL) {
      return(delegates->ModuleAudioSample());
    }

    return(NULL);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl UnloadDll(EmuState* emuState)
  {
    if ((instance->DialogOpen) && (emuState->EmulationRunning))
    {
      MessageBox(0, "Close Configuration Dialog before unloading", "Ok", 0);

      return;
    }

    delegates->GetModuleName = NULL;
    delegates->ConfigModule = NULL;
    delegates->PakPortWrite = NULL;
    delegates->PakPortRead = NULL;
    delegates->SetInterruptCallPointer = NULL;
    delegates->DmaMemPointer = NULL;
    delegates->HeartBeat = NULL;
    delegates->PakMemWrite8 = NULL;
    delegates->PakMemRead8 = NULL;
    delegates->ModuleStatus = NULL;
    delegates->ModuleAudioSample = NULL;
    delegates->ModuleReset = NULL;

    if (instance->hInstLib != NULL) {
      FreeLibrary(instance->hInstLib);
    }

    instance->hInstLib = NULL;

    DynamicMenuCallback(emuState, NULL, MENU_REFRESH, IGNORE);
  }
}

/**
Load a ROM pack
return total bytes loaded, or 0 on failure
*/
extern "C" {
  __declspec(dllexport) int __cdecl LoadROMPack(EmuState* emuState, char* filename)
  {
    constexpr size_t PAK_MAX_MEM = 0x40000;

    // If there is an existing ROM, ditch it
    if (instance->ExternalRomBuffer != nullptr) {
      free(instance->ExternalRomBuffer);
    }

    // Allocate memory for the ROM
    instance->ExternalRomBuffer = (uint8_t*)malloc(PAK_MAX_MEM);

    // If memory was unable to be allocated, fail
    if (instance->ExternalRomBuffer == nullptr) {
      MessageBox(0, "cant allocate ram", "Ok", 0);

      return 0;
    }

    // Open the ROM file, fail if unable to
    FILE* rom_handle = fopen(filename, "rb");

    if (rom_handle == nullptr) return 0;

    // Load the file, one byte at a time.. (TODO: Get size and read entire block)
    int index = 0;

    while ((feof(rom_handle) == 0) && (index < PAK_MAX_MEM)) {
      instance->ExternalRomBuffer[index++] = fgetc(rom_handle);
    }

    fclose(rom_handle);

    UnloadDll(emuState);

    instance->BankedCartOffset = 0;
    instance->RomPackLoaded = true;

    return index;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl UnloadPack(EmuState* emuState)
  {
    UnloadDll(emuState);

    strcpy(instance->DllPath, "");
    strcpy(instance->Modname, "Blank");

    instance->RomPackLoaded = false;

    SetCart(0);

    if (instance->ExternalRomBuffer != nullptr) {
      free(instance->ExternalRomBuffer);
    }

    instance->ExternalRomBuffer = nullptr;

    emuState->ResetPending = RESET_HARD;

    DynamicMenuCallback(emuState, NULL, MENU_REFRESH, IGNORE);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl UpdateBusPointer()
  {
    if (delegates->SetInterruptCallPointer != NULL) {
      delegates->SetInterruptCallPointer(GetCPU()->CPUAssertInterrupt);
    }
  }
}

extern "C" {
  __declspec(dllexport) int __cdecl InsertModule(EmuState* emuState, char* modulePath)
  {
    char catNumber[MAX_LOADSTRING] = "";
    char temp[MAX_LOADSTRING] = "";
    char text[1024] = "";
    char ini[MAX_PATH] = "";
    unsigned char fileType = 0;

    fileType = FileID(modulePath);

    switch (fileType)
    {
    case 0:		//File doesn't exist
      return(NOMODULE);
      break;

    case 2:		//File is a ROM image
      UnloadDll(emuState);

      LoadROMPack(emuState, modulePath);

      strncpy(instance->Modname, modulePath, MAX_PATH);

      FilePathStripPath(instance->Modname);

      DynamicMenuCallback(emuState, NULL, MENU_REFRESH, IGNORE);

      emuState->ResetPending = RESET_HARD;

      SetCart(1);

      return(0);

    case 1:		//File is a DLL
      UnloadDll(emuState);
      instance->hInstLib = LoadLibrary(modulePath);

      if (instance->hInstLib == NULL) {
        return(NOMODULE);
      }

      SetCart(0);

      delegates->GetModuleName = (GETNAME)GetProcAddress(instance->hInstLib, "ModuleName");
      delegates->ConfigModule = (CONFIGIT)GetProcAddress(instance->hInstLib, "ModuleConfig");
      delegates->PakPortWrite = (PACKPORTWRITE)GetProcAddress(instance->hInstLib, "PackPortWrite");
      delegates->PakPortRead = (PACKPORTREAD)GetProcAddress(instance->hInstLib, "PackPortRead");
      delegates->SetInterruptCallPointer = (SETINTERRUPTCALLPOINTER)GetProcAddress(instance->hInstLib, "AssertInterrupt");
      delegates->DmaMemPointer = (DMAMEMPOINTERS)GetProcAddress(instance->hInstLib, "MemPointers");
      delegates->HeartBeat = (HEARTBEAT)GetProcAddress(instance->hInstLib, "HeartBeat");
      delegates->PakMemWrite8 = (MEMWRITE8)GetProcAddress(instance->hInstLib, "PakMemWrite8");
      delegates->PakMemRead8 = (MEMREAD8)GetProcAddress(instance->hInstLib, "PakMemRead8");
      delegates->ModuleStatus = (MODULESTATUS)GetProcAddress(instance->hInstLib, "ModuleStatus");
      delegates->ModuleAudioSample = (MODULEAUDIOSAMPLE)GetProcAddress(instance->hInstLib, "ModuleAudioSample");
      delegates->ModuleReset = (MODULERESET)GetProcAddress(instance->hInstLib, "ModuleReset");
      delegates->SetIniPath = (SETINIPATH)GetProcAddress(instance->hInstLib, "SetIniPath");
      delegates->PakSetCart = (SETCARTPOINTER)GetProcAddress(instance->hInstLib, "SetCart");

      if (delegates->GetModuleName == NULL)
      {
        FreeLibrary(instance->hInstLib);

        instance->hInstLib = NULL;

        return(NOTVCC);
      }

      instance->BankedCartOffset = 0;

      if (delegates->DmaMemPointer != NULL) {
        delegates->DmaMemPointer(MemRead8, MemWrite8);
      }

      if (delegates->SetInterruptCallPointer != NULL) {
        delegates->SetInterruptCallPointer(GetCPU()->CPUAssertInterrupt);
      }

      delegates->GetModuleName(instance->Modname, catNumber, DynamicMenuCallback);  //Instantiate the menus from HERE!

      sprintf(temp, "Configure %s", instance->Modname);

      strcat(text, "Module Name: ");
      strcat(text, instance->Modname);
      strcat(text, "\n");

      if (delegates->ConfigModule != NULL)
      {
        instance->ModualParms |= 1;

        strcat(text, "Has Configurable options\n");
      }

      if (delegates->PakPortWrite != NULL)
      {
        instance->ModualParms |= 2;

        strcat(text, "Is IO writable\n");
      }

      if (delegates->PakPortRead != NULL)
      {
        instance->ModualParms |= 4;

        strcat(text, "Is IO readable\n");
      }

      if (delegates->SetInterruptCallPointer != NULL)
      {
        instance->ModualParms |= 8;

        strcat(text, "Generates Interrupts\n");
      }

      if (delegates->DmaMemPointer != NULL)
      {
        instance->ModualParms |= 16;

        strcat(text, "Generates DMA Requests\n");
      }

      if (delegates->HeartBeat != NULL)
      {
        instance->ModualParms |= 32;

        strcat(text, "Needs Heartbeat\n");
      }

      if (delegates->ModuleAudioSample != NULL)
      {
        instance->ModualParms |= 64;

        strcat(text, "Analog Audio Outputs\n");
      }

      if (delegates->PakMemWrite8 != NULL)
      {
        instance->ModualParms |= 128;

        strcat(text, "Needs ChipSelect Write\n");
      }

      if (delegates->PakMemRead8 != NULL)
      {
        instance->ModualParms |= 256;

        strcat(text, "Needs ChipSelect Read\n");
      }

      if (delegates->ModuleStatus != NULL)
      {
        instance->ModualParms |= 512;

        strcat(text, "Returns Status\n");
      }

      if (delegates->ModuleReset != NULL)
      {
        instance->ModualParms |= 1024;

        strcat(text, "Needs Reset Notification\n");
      }

      if (delegates->SetIniPath != NULL)
      {
        instance->ModualParms |= 2048;

        GetIniFilePath(ini);

        delegates->SetIniPath(ini);
      }

      if (delegates->PakSetCart != NULL)
      {
        instance->ModualParms |= 4096;

        strcat(text, "Can Assert CART\n");

        delegates->PakSetCart(SetCart);
      }

      strcpy(instance->DllPath, modulePath);

      emuState->ResetPending = RESET_HARD;

      return(0);
    }

    return(NOMODULE);
  }
}

