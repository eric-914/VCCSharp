#include "di.version.h"
#include <dinput.h>
#include <direct.h>
#include <assert.h>
#include <ShlObj.h>
#include <string>

#include "../resources/resource.h"

#include "Config.h"
#include "DirectDraw.h"
#include "PAKInterface.h"
#include "Keyboard.h"
#include "Cassette.h"
#include "Joystick.h"
#include "MC6821.h"
#include "Graphics.h"
#include "Audio.h"
#include "Emu.h"

#include "fileoperations.h"
#include "CmdLineArguments.h"
#include "ConfigDialogCallbacks.h"
#include "ConfigIO.h"

#include "VccState.h"

#include "macros.h"

#include "resource.h"
#include "JoystickModel.h"

using namespace std;

const unsigned char TranslateScan2Disp[SCAN_TRANS_COUNT] = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 32, 38, 20, 33, 35, 40, 36, 24, 30, 31, 42, 43, 55, 52, 16, 34, 19, 21, 22, 23, 25, 26, 27, 45, 46, 0, 51, 44, 41, 39, 18, 37, 17, 29, 28, 47, 48, 49, 51, 0, 53, 54, 50, 66, 67, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 58, 64, 60, 0, 62, 0, 63, 0, 59, 65, 61, 56, 57 };
const unsigned char TranslateDisp2Scan[SCAN_TRANS_COUNT] = { 78, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 30, 48, 46, 32, 18, 33, 34, 35, 23, 36, 37, 38, 50, 49, 24, 25, 16, 19, 31, 20, 22, 47, 17, 45, 21, 44, 26, 27, 43, 39, 40, 51, 52, 53, 58, 54, 29, 56, 57, 28, 82, 83, 71, 79, 73, 81, 75, 77, 72, 80, 59, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

ConfigState* InitializeInstance(ConfigState*);

static ConfigState* instance = InitializeInstance(new ConfigState());
static ConfigModel* model;
static JoystickModel* left;
static JoystickModel* right;

extern "C" {
  __declspec(dllexport) ConfigState* __cdecl GetConfigState() {
    return instance;
  }
}

extern "C" {
  __declspec(dllexport) ConfigModel* __cdecl GetConfigModel() {
    return model;
  }
}

extern "C" {
  __declspec(dllexport) JoystickModel* __cdecl GetLeftJoystick() {
    return left;
  }
}

extern "C" {
  __declspec(dllexport) JoystickModel* __cdecl GetRightJoystick() {
    return right;
  }
}

ConfigState* InitializeInstance(ConfigState* p) {
  left = new JoystickModel();
  right = new JoystickModel();

  model = new ConfigModel();

  model->Left = left;
  model->Right = right;

  p->Model = model;

  p->NumberOfSoundCards = 0;
  p->NumberOfJoysticks = 0;
  p->PrintMonitorWindow = 0;
  p->TapeCounter = 0;
  p->TextMode = 1;
  p->TapeMode = STOP;

  p->hDlgBar = NULL;
  p->hDlgTape = NULL;

  strcpy(p->IniFilePath, "");
  strcpy(p->OutBuffer, "");
  strcpy(p->SerialCaptureFile, "");
  strcpy(p->TapeFileName, "");

  GetModuleFileName(NULL, p->ExecDirectory, MAX_PATH);
  FilePathRemoveFileSpec(p->ExecDirectory);

  static TCHAR AppDataPath[MAX_PATH];

  if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_APPDATA, NULL, 0, AppDataPath))) {
    OutputDebugString(AppDataPath);
  }

  strcpy(p->AppDataPath, AppDataPath);

  return p;
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl GetSoundCardIndex(char* soundCardName) {
    for (unsigned char index = 0; index < instance->NumberOfSoundCards; index++) {
      if (!strcmp(instance->SoundCards[index].CardName, soundCardName)) {
        return index;
      }
    }

    return 0;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl ValidateModel(ConfigModel* model) {
    if (model->KeyMapIndex > 3) {
      model->KeyMapIndex = 0;	//Default to DECB Mapping
    }

    FileCheckPath(model->ModulePath);
    FileCheckPath(model->ExternalBasicImage);

    FileValidatePath(model->ModulePath); //--If module is in same location as .exe, strip off path portion, leaving only module name
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl TranslateDisplay2Scan(LRESULT x)
  {
    assert(x >= 0 && x < SCAN_TRANS_COUNT);

    return TranslateDisp2Scan[x];
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl TranslateScan2Display(int x)
  {
    assert(x >= 0 && x < SCAN_TRANS_COUNT);

    return TranslateScan2Disp[x];
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetIniFilePath(char* path)
  {
    //  Path must be to an existing ini file
    strcpy(instance->IniFilePath, path);
  }
}

extern "C" {
  __declspec(dllexport) int __cdecl GetPaletteType() {
    return(instance->Model->PaletteType);
  }
}

extern "C" {
  __declspec(dllexport) int __cdecl GetRememberSize() {
    return((int)(instance->Model->RememberSize));
  }
}

extern "C" {
  __declspec(dllexport) POINT __cdecl GetIniWindowSize() {
    POINT out = POINT();

    out.x = instance->Model->WindowSizeX;
    out.y = instance->Model->WindowSizeY;

    return(out);
  }
}


extern "C" {
  __declspec(dllexport) void __cdecl UpdateTapeDialog(unsigned int counter, unsigned char tapeMode)
  {
    if (instance->hDlgTape == NULL) {
      return;
    }

    instance->TapeCounter = counter;
    instance->TapeMode = tapeMode;

    GetTapeName(instance->TapeFileName);
    FilePathStripPath(instance->TapeFileName);

    SetDialogTapeCounter(instance->hDlgTape, instance->TapeCounter);
    SetDialogTapeMode(instance->hDlgTape, instance->TapeMode);
    SetDialogTapeFileName(instance->hDlgTape, instance->TapeFileName);
  }
}

extern "C" {
  __declspec(dllexport) int __cdecl SelectSerialCaptureFile(EmuState* emuState, char* filename)
  {
    OPENFILENAME ofn;
    char dummy[MAX_PATH] = "";
    char tempFileName[MAX_PATH] = "";
    char* serialCaptureFilePath = instance->Model->SerialCaptureFilePath;

    memset(&ofn, 0, sizeof(ofn));

    ofn.lStructSize = sizeof(OPENFILENAME);
    ofn.hwndOwner = emuState->WindowHandle; // GetTopWindow(NULL);
    ofn.Flags = OFN_HIDEREADONLY;
    ofn.hInstance = GetModuleHandle(0);
    ofn.lpstrDefExt = "txt";
    ofn.lpstrFilter = "Text File\0*.txt\0\0";
    ofn.nFilterIndex = 0;					      // current filter index
    ofn.lpstrFile = tempFileName;		    // contains full path and filename on return
    ofn.nMaxFile = MAX_PATH;			      // sizeof lpstrFile
    ofn.lpstrFileTitle = NULL;				  // filename and extension only
    ofn.nMaxFileTitle = MAX_PATH;			  // sizeof lpstrFileTitle
    ofn.lpstrInitialDir = serialCaptureFilePath;  // initial directory
    ofn.lpstrTitle = "Open print capture file";		// title bar string

    if (GetOpenFileName(&ofn)) {
      if (!(MC6821_OpenPrintFile(tempFileName))) {
        MessageBox(0, "Can't Open File", "Can't open the file specified.", 0);
      }

      if (ofn.lpstrFile != "") {
        string tmp = ofn.lpstrFile;
        size_t idx = tmp.find_last_of("\\");
        tmp = tmp.substr(0, idx);

        strcpy(instance->Model->SerialCaptureFilePath, tmp.c_str());
      }
    }

    strcpy(filename, tempFileName);

    return(1);
  }
}
