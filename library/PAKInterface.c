#include <iostream>
#include <string>

#include "PAKInterface.h"
#include "PakInterfaceDelegates.h"
#include "PakInterfaceModule.h"

#include "MC6821.h"
#include "Config.h"
#include "TC1014MMU.h"
#include "EmuState.h"
#include "cpudef.h"
#include "fileoperations.h"
#include "MenuCallbacks.h"

using namespace std;

PakInterfaceState* InitializeInstance(PakInterfaceState*);

static PakInterfaceState* instance = InitializeInstance(new PakInterfaceState());

extern "C" {
  __declspec(dllexport) PakInterfaceState* __cdecl GetPakInterfaceState() {
    return instance;
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
  __declspec(dllexport) void __cdecl ResetBus()
  {
    instance->BankedCartOffset = 0;

    if (HasModuleReset()) {
      InvokeModuleReset();
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl GetModuleStatus(EmuState* emuState)
  {
    if (HasModuleStatus()) {
      InvokeModuleStatus(emuState->StatusLine);
    }
    else {
      sprintf(emuState->StatusLine, "");
    }
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl PakPortRead(unsigned char port)
  {
    if (HasModulePortRead()) {
      return ModulePortRead(port);
    }

    return(NULL);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl PakPortWrite(unsigned char port, unsigned char data)
  {
    if (ModulePortWrite(port, data) == 1) {
      if ((port == 0x40) && (instance->RomPackLoaded)) {
        instance->BankedCartOffset = (data & 15) << 14;
      }
    }
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl PakMem8Read(unsigned short address)
  {
    if (HasModuleMem8Read()) {
      return ModuleMem8Read(address);
    }

    if (instance->ExternalRomBuffer != NULL) {
      //TODO: Threading makes it possible to reach here where ExternalRomBuffer = NULL despite check.
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
    if (HasModuleAudioSample()) {
      return(ReadModuleAudioSample());
    }

    return(NULL);
  }
}

//===============================================================
//TODO: Partially ported to C#
// Used by LoadROMPack(...), UnloadPack(...), InsertModule(...)
//===============================================================
extern "C" {
  __declspec(dllexport) void __cdecl UnloadDll(EmuState* emuState)
  {
    if ((instance->DialogOpen) && (emuState->EmulationRunning))
    {
      MessageBox(0, "Close Configuration Dialog before unloading", "Ok", 0);

      return;
    }

    UnloadModule();

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
    FreeMemory(instance->ExternalRomBuffer);

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

    FreeMemory(instance->ExternalRomBuffer);

    instance->ExternalRomBuffer = NULL;

    emuState->ResetPending = RESET_HARD;

    DynamicMenuCallback(emuState, NULL, MENU_REFRESH, IGNORE);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl UpdateBusPointer()
  {
    if (HasSetInterruptCallPointer()) {
      InvokeSetInterruptCallPointer();
    }
  }
}

//File doesn't exist
extern "C" {
  __declspec(dllexport) int __cdecl InsertModuleCase0() {
    return(NOMODULE);
  }
}

//File is a DLL
extern "C" {
  __declspec(dllexport) int __cdecl InsertModuleCase1(EmuState* emuState, char* modulePath) {
    char catNumber[MAX_LOADSTRING] = "";
    char temp[MAX_LOADSTRING] = "";
    char text[1024] = "";
    char ini[MAX_PATH] = "";

    UnloadDll(emuState);
    instance->hInstLib = LoadLibrary(modulePath);

    if (instance->hInstLib == NULL) {
      return(NOMODULE);
    }

    SetCart(0);

    if (SetDelegates(instance->hInstLib))
    {
      FreeLibrary(instance->hInstLib);

      instance->hInstLib = NULL;

      return(NOTVCC);
    };

    instance->BankedCartOffset = 0;

    if (HasDmaMemPointer()) {
      InvokeDmaMemPointer();
    }

    if (HasSetInterruptCallPointer()) {
      InvokeSetInterruptCallPointer();
    }

    InvokeGetModuleName(instance->Modname, catNumber);  //Instantiate the menus from HERE!

    sprintf(temp, "Configure %s", instance->Modname);

    strcat(text, "Module Name: ");
    strcat(text, instance->Modname);
    strcat(text, "\n");

    if (HasConfigModule())
    {
      instance->ModualParms |= 1;

      strcat(text, "Has Configurable options\n");
    }

    if (HasPakPortWrite())
    {
      instance->ModualParms |= 2;

      strcat(text, "Is IO writable\n");
    }

    if (HasPakPortRead())
    {
      instance->ModualParms |= 4;

      strcat(text, "Is IO readable\n");
    }

    if (HasSetInterruptCallPointer())
    {
      instance->ModualParms |= 8;

      strcat(text, "Generates Interrupts\n");
    }

    if (HasDmaMemPointer())
    {
      instance->ModualParms |= 16;

      strcat(text, "Generates DMA Requests\n");
    }

    if (HasHeartBeat())
    {
      instance->ModualParms |= 32;

      strcat(text, "Needs Heartbeat\n");
    }

    if (HasModuleAudioSample())
    {
      instance->ModualParms |= 64;

      strcat(text, "Analog Audio Outputs\n");
    }

    if (HasPakMemWrite8())
    {
      instance->ModualParms |= 128;

      strcat(text, "Needs ChipSelect Write\n");
    }

    if (HasPakMemRead8())
    {
      instance->ModualParms |= 256;

      strcat(text, "Needs ChipSelect Read\n");
    }

    if (HasModuleStatus())
    {
      instance->ModualParms |= 512;

      strcat(text, "Returns Status\n");
    }

    if (HasModuleReset())
    {
      instance->ModualParms |= 1024;

      strcat(text, "Needs Reset Notification\n");
    }

    if (HasSetIniPath())
    {
      instance->ModualParms |= 2048;

      strcpy(ini, GetConfigState()->IniFilePath);

      InvokeSetIniPath(ini);
    }

    if (HasPakSetCart())
    {
      instance->ModualParms |= 4096;

      strcat(text, "Can Assert CART\n");

      InvokePakSetCart();
    }

    strcpy(instance->DllPath, modulePath);

    emuState->ResetPending = RESET_HARD;

    return(0);
  }
}

//File is a ROM image
extern "C" {
  __declspec(dllexport) int __cdecl InsertModuleCase2(EmuState* emuState, char* modulePath) {
    UnloadDll(emuState);

    LoadROMPack(emuState, modulePath);

    strncpy(instance->Modname, modulePath, MAX_PATH);

    FilePathStripPath(instance->Modname);

    DynamicMenuCallback(emuState, NULL, MENU_REFRESH, IGNORE);

    emuState->ResetPending = RESET_HARD;

    SetCart(1);

    return(0);
  }
}

extern "C" {
  __declspec(dllexport) int __cdecl InsertModule(EmuState* emuState, char* modulePath)
  {
    unsigned char fileType = fileType = FileID(modulePath);

    switch (fileType)
    {
    case 0:		//File doesn't exist
      return InsertModuleCase0();

    case 1:		//File is a DLL
      return InsertModuleCase1(emuState, modulePath);

    case 2:		//File is a ROM image
      return InsertModuleCase2(emuState, modulePath);
    }

    return(NOMODULE);
  }
}
