#include <string>

#include "PAKInterface.h"
#include "PakInterfaceModule.h"

typedef struct {
  HINSTANCE hInstLib;

  unsigned char* ExternalRomBuffer;

  unsigned int BankedCartOffset;
} PakInterfaceState;

PakInterfaceState* InitializeInstance(PakInterfaceState*);

static PakInterfaceState* instance = InitializeInstance(new PakInterfaceState());

extern "C" {
  __declspec(dllexport) PakInterfaceState* __cdecl GetPakInterfaceState() {
    return instance;
  }
}

PakInterfaceState* InitializeInstance(PakInterfaceState* p) {
  p->BankedCartOffset = 0;

  p->ExternalRomBuffer = nullptr;

  return p;
}

extern "C" {
  __declspec(dllexport) void* GetFunction(HMODULE hModule, LPCSTR  lpProcName) {
    return GetProcAddress(hModule, lpProcName);
  }
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
  __declspec(dllexport) void __cdecl ResetRomBuffer(unsigned char* buffer)
  {
    constexpr size_t PAK_MAX_MEM = 0x40000;

    // If there is an existing ROM, ditch it
    FreeMemory(instance->ExternalRomBuffer);

    // Allocate memory for the ROM
    instance->ExternalRomBuffer = (uint8_t*)malloc(PAK_MAX_MEM);
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
