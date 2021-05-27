#include <string>

#include "PAKInterface.h"
#include "PakInterfaceDelegates.h"
#include "PakInterfaceModule.h"

#include "Config.h"
#include "fileoperations.h"
#include "MenuCallbacks.h"

#define MAX_LOADSTRING 255

#define NOMODULE	1
#define NOTVCC	2

#define MENU_REFRESH 2

PakInterfaceState* InitializeInstance(PakInterfaceState*);

static PakInterfaceState* instance = InitializeInstance(new PakInterfaceState());

extern "C" {
  __declspec(dllexport) PakInterfaceState* __cdecl GetPakInterfaceState() {
    return instance;
  }
}

PakInterfaceState* InitializeInstance(PakInterfaceState* p) {
  p->CartInserted = 0;

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
  __declspec(dllexport) HINSTANCE __cdecl PAKLoadLibrary(char* modulePath) {
    return LoadLibrary(modulePath);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl PAKFreeLibrary(HINSTANCE hInstLib) {
    FreeLibrary(hInstLib);
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
  __declspec(dllexport) void __cdecl SetCart(unsigned char cart)
  {
    instance->CartInserted = cart;
  }
}

extern "C" {
  __declspec(dllexport) int __cdecl FileID(char* filename)
  {
    FILE* handle = NULL;
    char temp[3] = "";

    handle = fopen(filename, "rb");

    if (handle == NULL) {
      return 0;	//File Doesn't exist
    }

    temp[0] = fgetc(handle);
    temp[1] = fgetc(handle);
    temp[2] = 0;
    fclose(handle);

    if (strcmp(temp, "MZ") == 0) {
      return 1;	//DLL File
    }

    return 2;		//Rom Image 
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl PakPortRead(unsigned char port)
  {
    if (HasModulePortRead()) {
      return ModulePortRead(port);
    }

    return NULL;
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

    int offset = (address & 32767) + instance->BankedCartOffset;
    if (instance->ExternalRomBuffer != NULL) {
      //TODO: Threading makes it possible to reach here where ExternalRomBuffer = NULL despite check.
      return(instance->ExternalRomBuffer[offset]);
    }

    return 0;
  }
}

extern "C" {
  __declspec(dllexport) unsigned short __cdecl PakAudioSample()
  {
    if (HasModuleAudioSample()) {
      return(ReadModuleAudioSample());
    }

    return NULL;
  }
}

//===============================================================
//TODO: Partially ported to C#
// Used by LoadROMPack(...), UnloadPack(...), InsertModule(...)
//===============================================================
extern "C" {
  __declspec(dllexport) void __cdecl UnloadDll(unsigned char emulationRunning)
  {
    if ((instance->DialogOpen) && (emulationRunning))
    {
      MessageBox(0, "Close Configuration Dialog before unloading", "Ok", 0);

      return;
    }

    UnloadModule();

    if (instance->hInstLib != NULL) {
      FreeLibrary(instance->hInstLib);
    }

    instance->hInstLib = NULL;

    DynamicMenuCallback(NULL, MENU_REFRESH, IGNORE);
  }
}

/**
Load a ROM pack
return total bytes loaded, or 0 on failure
*/
extern "C" {
  __declspec(dllexport) int __cdecl LoadROMPack(unsigned char emulationRunning, char* filename)
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

    UnloadDll(emulationRunning);

    instance->BankedCartOffset = 0;
    instance->RomPackLoaded = true;

    return index;
  }
}
