#include <iostream>
#include <string>

#include "PAKInterface.h"
#include "VCC.h"
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
  __declspec(dllexport) void __cdecl PakTimer(void)
  {
    if (instance->HeartBeat != NULL) {
      instance->HeartBeat();
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl ResetBus(void)
  {
    instance->BankedCartOffset = 0;

    if (instance->ModuleReset != NULL) {
      instance->ModuleReset();
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl GetModuleStatus(EmuState* emuState)
  {
    if (instance->ModuleStatus != NULL) {
      instance->ModuleStatus(emuState->StatusLine);
    }
    else {
      sprintf(emuState->StatusLine, "");
    }
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl PakPortRead(unsigned char port)
  {
    if (instance->PakPortRead != NULL) {
      return(instance->PakPortRead(port));
    }
    else {
      return(NULL);
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl PakPortWrite(unsigned char port, unsigned char data)
  {
    if (instance->PakPortWrite != NULL)
    {
      instance->PakPortWrite(port, data);
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
    if (instance->PakMemRead8 != NULL) {
      return(instance->PakMemRead8(address & 32767));
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
    if (instance->ModuleAudioSample != NULL) {
      return(instance->ModuleAudioSample());
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

    instance->GetModuleName = NULL;
    instance->ConfigModule = NULL;
    instance->PakPortWrite = NULL;
    instance->PakPortRead = NULL;
    instance->SetInterruptCallPointer = NULL;
    instance->DmaMemPointer = NULL;
    instance->HeartBeat = NULL;
    instance->PakMemWrite8 = NULL;
    instance->PakMemRead8 = NULL;
    instance->ModuleStatus = NULL;
    instance->ModuleAudioSample = NULL;
    instance->ModuleReset = NULL;

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
  __declspec(dllexport) void __cdecl UpdateBusPointer(void)
  {
    if (instance->SetInterruptCallPointer != NULL) {
      instance->SetInterruptCallPointer(GetCPU()->CPUAssertInterrupt);
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

      instance->GetModuleName = (GETNAME)GetProcAddress(instance->hInstLib, "ModuleName");
      instance->ConfigModule = (CONFIGIT)GetProcAddress(instance->hInstLib, "ModuleConfig");
      instance->PakPortWrite = (PACKPORTWRITE)GetProcAddress(instance->hInstLib, "PackPortWrite");
      instance->PakPortRead = (PACKPORTREAD)GetProcAddress(instance->hInstLib, "PackPortRead");
      instance->SetInterruptCallPointer = (SETINTERRUPTCALLPOINTER)GetProcAddress(instance->hInstLib, "AssertInterrupt");
      instance->DmaMemPointer = (DMAMEMPOINTERS)GetProcAddress(instance->hInstLib, "MemPointers");
      instance->HeartBeat = (HEARTBEAT)GetProcAddress(instance->hInstLib, "HeartBeat");
      instance->PakMemWrite8 = (MEMWRITE8)GetProcAddress(instance->hInstLib, "PakMemWrite8");
      instance->PakMemRead8 = (MEMREAD8)GetProcAddress(instance->hInstLib, "PakMemRead8");
      instance->ModuleStatus = (MODULESTATUS)GetProcAddress(instance->hInstLib, "ModuleStatus");
      instance->ModuleAudioSample = (MODULEAUDIOSAMPLE)GetProcAddress(instance->hInstLib, "ModuleAudioSample");
      instance->ModuleReset = (MODULERESET)GetProcAddress(instance->hInstLib, "ModuleReset");
      instance->SetIniPath = (SETINIPATH)GetProcAddress(instance->hInstLib, "SetIniPath");
      instance->PakSetCart = (SETCARTPOINTER)GetProcAddress(instance->hInstLib, "SetCart");

      if (instance->GetModuleName == NULL)
      {
        FreeLibrary(instance->hInstLib);

        instance->hInstLib = NULL;

        return(NOTVCC);
      }

      instance->BankedCartOffset = 0;

      if (instance->DmaMemPointer != NULL) {
        instance->DmaMemPointer(MemRead8, MemWrite8);
      }

      if (instance->SetInterruptCallPointer != NULL) {
        instance->SetInterruptCallPointer(GetCPU()->CPUAssertInterrupt);
      }

      instance->GetModuleName(instance->Modname, catNumber, DynamicMenuCallback);  //Instantiate the menus from HERE!

      sprintf(temp, "Configure %s", instance->Modname);

      strcat(text, "Module Name: ");
      strcat(text, instance->Modname);
      strcat(text, "\n");

      if (instance->ConfigModule != NULL)
      {
        instance->ModualParms |= 1;

        strcat(text, "Has Configurable options\n");
      }

      if (instance->PakPortWrite != NULL)
      {
        instance->ModualParms |= 2;

        strcat(text, "Is IO writable\n");
      }

      if (instance->PakPortRead != NULL)
      {
        instance->ModualParms |= 4;

        strcat(text, "Is IO readable\n");
      }

      if (instance->SetInterruptCallPointer != NULL)
      {
        instance->ModualParms |= 8;

        strcat(text, "Generates Interrupts\n");
      }

      if (instance->DmaMemPointer != NULL)
      {
        instance->ModualParms |= 16;

        strcat(text, "Generates DMA Requests\n");
      }

      if (instance->HeartBeat != NULL)
      {
        instance->ModualParms |= 32;

        strcat(text, "Needs Heartbeat\n");
      }

      if (instance->ModuleAudioSample != NULL)
      {
        instance->ModualParms |= 64;

        strcat(text, "Analog Audio Outputs\n");
      }

      if (instance->PakMemWrite8 != NULL)
      {
        instance->ModualParms |= 128;

        strcat(text, "Needs ChipSelect Write\n");
      }

      if (instance->PakMemRead8 != NULL)
      {
        instance->ModualParms |= 256;

        strcat(text, "Needs ChipSelect Read\n");
      }

      if (instance->ModuleStatus != NULL)
      {
        instance->ModualParms |= 512;

        strcat(text, "Returns Status\n");
      }

      if (instance->ModuleReset != NULL)
      {
        instance->ModualParms |= 1024;

        strcat(text, "Needs Reset Notification\n");
      }

      if (instance->SetIniPath != NULL)
      {
        instance->ModualParms |= 2048;

        GetIniFilePath(ini);

        instance->SetIniPath(ini);
      }

      if (instance->PakSetCart != NULL)
      {
        instance->ModualParms |= 4096;

        strcat(text, "Can Assert CART\n");

        instance->PakSetCart(SetCart);
      }

      strcpy(instance->DllPath, modulePath);

      emuState->ResetPending = RESET_HARD;

      return(0);
    }

    return(NOMODULE);
  }
}

extern "C" {
  __declspec(dllexport) int __cdecl LoadCart(EmuState* emuState)
  {
    OPENFILENAME ofn;
    char szFileName[MAX_PATH] = "";

    memset(&ofn, 0, sizeof(ofn));

    ofn.lStructSize = sizeof(OPENFILENAME);
    ofn.hwndOwner = emuState->WindowHandle;
    ofn.lpstrFilter = "Program Packs\0*.ROM;*.ccc;*.DLL;*.pak\0\0";			// filter string
    ofn.nFilterIndex = 1;							          // current filter index
    ofn.lpstrFile = szFileName;				          // contains full path and filename on return
    ofn.nMaxFile = MAX_PATH;					          // sizeof lpstrFile
    ofn.lpstrFileTitle = NULL;						      // filename and extension only
    ofn.nMaxFileTitle = MAX_PATH;					      // sizeof lpstrFileTitle
    ofn.lpstrInitialDir = GetConfigState()->Model.PakPath;  // initial directory
    ofn.lpstrTitle = TEXT("Load Program Pack");	// title bar string
    ofn.Flags = OFN_HIDEREADONLY;

    if (GetOpenFileName(&ofn)) {
      if (!InsertModule(emuState, szFileName)) {
        string tmp = ofn.lpstrFile;
        size_t idx = tmp.find_last_of("\\");
        tmp = tmp.substr(0, idx);

        strcpy(GetConfigState()->Model.PakPath, tmp.c_str());

        return(0);
      }
    }

    return(1);
  }
}
