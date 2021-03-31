#include <process.h>
#include <string>

#include "Emu.h"
#include "Config.h"
#include "PAKInterface.h"
#include "VccState.h"

extern "C" {
  __declspec(dllexport) int __cdecl OpenLoadCartFileDialog(EmuState* emuState)
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
    ofn.lpstrInitialDir = GetConfigState()->Model->PakPath;  // initial directory
    ofn.lpstrTitle = TEXT("Load Program Pack");	// title bar string
    ofn.Flags = OFN_HIDEREADONLY;

    if (GetOpenFileName(&ofn)) {
      if (!InsertModule(emuState, szFileName)) {
        std::string tmp = ofn.lpstrFile;
        size_t idx = tmp.find_last_of("\\");
        tmp = tmp.substr(0, idx);

        strcpy(GetConfigState()->Model->PakPath, tmp.c_str());

        return(0);
      }
    }

    return(1);
  }
}

extern "C" {
  __declspec(dllexport) unsigned __stdcall CartLoad(void* dummy)
  {
    EmuState* emuState = GetEmuState();
    VccState* vccState = GetVccState();

    OpenLoadCartFileDialog(emuState);

    emuState->EmulationRunning = true;
    vccState->DialogOpen = false;

    return(NULL);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl LoadPack()
  {
    unsigned threadID;

    VccState* vccState = GetVccState();

    if (vccState->DialogOpen) {
      return;
    }

    vccState->DialogOpen = true;

    _beginthreadex(NULL, 0, &CartLoad, CreateEvent(NULL, FALSE, FALSE, NULL), 0, &threadID);
  }
}
