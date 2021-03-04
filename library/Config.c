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
#include "VCC.h"
#include "Graphics.h"
#include "Audio.h"

#include "EmuState.h"
#include "fileoperations.h"
#include "CmdLineArguments.h"
#include "ConfigDialogCallbacks.h"
#include "ConfigIO.h"

#include "macros.h"

using namespace std;

const unsigned char TranslateScan2Disp[SCAN_TRANS_COUNT] = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 32, 38, 20, 33, 35, 40, 36, 24, 30, 31, 42, 43, 55, 52, 16, 34, 19, 21, 22, 23, 25, 26, 27, 45, 46, 0, 51, 44, 41, 39, 18, 37, 17, 29, 28, 47, 48, 49, 51, 0, 53, 54, 50, 66, 67, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 58, 64, 60, 0, 62, 0, 63, 0, 59, 65, 61, 56, 57 };
const unsigned char TranslateDisp2Scan[SCAN_TRANS_COUNT] = { 78, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 30, 48, 46, 32, 18, 33, 34, 35, 23, 36, 37, 38, 50, 49, 24, 25, 16, 19, 31, 20, 22, 47, 17, 45, 21, 44, 26, 27, 43, 39, 40, 51, 52, 53, 58, 54, 29, 56, 57, 28, 82, 83, 71, 79, 73, 81, 75, 77, 72, 80, 59, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

static TCHAR AppDataPath[MAX_PATH];

ConfigState* InitializeInstance(ConfigState*);

static ConfigState* instance = InitializeInstance(new ConfigState());

extern "C" {
  __declspec(dllexport) ConfigState* __cdecl GetConfigState() {
    return instance;
  }
}

ConfigState* InitializeInstance(ConfigState* p) {
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

  if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_APPDATA, NULL, 0, AppDataPath))) {
    OutputDebugString(AppDataPath);
  }

  return p;
}

void SetWindowSize(int width, int height) {
  HWND handle = GetActiveWindow();

  SetWindowPos(handle, 0, 0, 0, width + 16, height + 81, SWP_NOMOVE | SWP_NOOWNERZORDER | SWP_NOZORDER);
}

unsigned char GetSoundCardIndex(char* soundCardName) {
  for (unsigned char index = 0; index < instance->NumberOfSoundCards; index++) {
    if (!strcmp(instance->SoundCards[index].CardName, soundCardName)) {
      return index;
    }
  }

  return 0;
}

void AdjustOverclockSpeed(EmuState* emuState, unsigned char change) {
  unsigned char cpuMultiplier = instance->Model.CPUMultiplier + change;

  if (cpuMultiplier < 2 || cpuMultiplier > instance->Model.MaxOverclock)
  {
    return;
  }

  // Send updates to the dialog if it's open.
  if (emuState->ConfigDialog != NULL)
  {
    HWND hDlg = instance->hWndConfig[1];

    SetDialogCpuMultiplier(hDlg, cpuMultiplier);
  }

  instance->Model.CPUMultiplier = cpuMultiplier;

  emuState->ResetPending = 4; // Without this, changing the config does nothing.
}

void GetIniFilePath(char* iniFilePath, char* argIniFile) {
  const char vccFolder[] = "\\VCC";
  const char iniFileName[] = "\\Vcc.ini";

  if (*argIniFile) {
    GetFullPathName(argIniFile, MAX_PATH, instance->IniFilePath, 0);
  }
  else {
    strcat(AppDataPath, vccFolder);

    if (_mkdir(AppDataPath) != 0) {
      OutputDebugString("Unable to create VCC config folder.");
    }

    strcpy(iniFilePath, AppDataPath);
    strcat(iniFilePath, iniFileName);
  }
}

void ValidateModel(ConfigModel model) {
  if (instance->Model.KeyMapIndex > 3) {
    instance->Model.KeyMapIndex = 0;	//Default to DECB Mapping
  }

  FileCheckPath(instance->Model.ModulePath);
  FileCheckPath(instance->Model.ExternalBasicImage);

  FileValidatePath(instance->Model.ModulePath); //--If module is in same location as .exe, strip off path portion, leaving only module name
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
  __declspec(dllexport) char* __cdecl ExternalBasicImage(void)
  {
    return(instance->Model.ExternalBasicImage);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl GetIniFilePath(char* path)
  {
    strcpy(path, instance->IniFilePath);
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
  __declspec(dllexport) char* __cdecl AppDirectory()
  {
    // This only works after LoadConfig has been called
    return AppDataPath;
  }
}

extern "C" {
  __declspec(dllexport) int __cdecl GetCurrentKeyboardLayout() {
    return(instance->Model.KeyMapIndex);
  }
}

extern "C" {
  __declspec(dllexport) int __cdecl GetPaletteType() {
    return(instance->Model.PaletteType);
  }
}

extern "C" {
  __declspec(dllexport) int __cdecl GetRememberSize() {
    return((int)(instance->Model.RememberSize));
  }
}

extern "C" {
  __declspec(dllexport) POINT __cdecl GetIniWindowSize() {
    POINT out = POINT();

    out.x = instance->Model.WindowSizeX;
    out.y = instance->Model.WindowSizeY;

    return(out);
  }
}

/**
 * Increase the overclock speed, as seen after a POKE 65497,0.
 * Valid values are [2,100].
 */
extern "C" {
  __declspec(dllexport) void __cdecl IncreaseOverclockSpeed(EmuState* emuState)
  {
    AdjustOverclockSpeed(emuState, 1);
  }
}

/**
 * Decrease the overclock speed, as seen after a POKE 65497,0.
 *
 * Setting this value to 0 will make the emulator pause.  Hence the minimum of 2.
 */
extern "C" {
  __declspec(dllexport) void __cdecl DecreaseOverclockSpeed(EmuState* emuState)
  {
    AdjustOverclockSpeed(emuState, -1);
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
  __declspec(dllexport) void __cdecl UpdateSoundBar(unsigned short left, unsigned short right)
  {
    if (instance->hDlgBar == NULL) {
      return;
    }

    SetDialogAudioBars(instance->hDlgBar, left, right);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl ConfigureJoysticks(void)
  {
    bool temp = false;

    JoystickModel left = instance->Model.Left;
    JoystickModel right = instance->Model.Right;

    instance->NumberOfJoysticks = EnumerateJoysticks();

    for (unsigned char index = 0; index < instance->NumberOfJoysticks; index++) {
      temp = InitJoyStick(index);
    }

    if (right.DiDevice >= instance->NumberOfJoysticks) {
      right.DiDevice = 0;
    }

    if (left.DiDevice >= instance->NumberOfJoysticks) {
      left.DiDevice = 0;
    }

    SetStickNumbers(left.DiDevice, right.DiDevice);

    if (instance->NumberOfJoysticks == 0)	//Use Mouse input if no Joysticks present
    {
      if (left.UseMouse == 3) {
        left.UseMouse = 1;
      }

      if (right.UseMouse == 3) {
        right.UseMouse = 1;
      }
    }
  }
}

extern "C" {
  __declspec(dllexport) int __cdecl SelectSerialCaptureFile(EmuState* emuState, char* filename)
  {
    OPENFILENAME ofn;
    char dummy[MAX_PATH] = "";
    char tempFileName[MAX_PATH] = "";
    char* serialCaptureFilePath = instance->Model.SerialCaptureFilePath;

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

        strcpy(instance->Model.SerialCaptureFilePath, tmp.c_str());
      }
    }

    strcpy(filename, tempFileName);

    return(1);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SynchSystemWithConfig(EmuState* emuState)
  {
    SetPaletteType();
    SetAspect(instance->Model.ForceAspect);
    SetScanLines(emuState, instance->Model.ScanLines);
    SetFrameSkip(instance->Model.FrameSkip);
    SetAutoStart(instance->Model.AutoStart);
    SetSpeedThrottle(instance->Model.SpeedThrottle);
    SetCPUMultiplayer(instance->Model.CPUMultiplier);
    SetRamSize(instance->Model.RamSize);
    SetCpuType(instance->Model.CpuType);
    SetMonitorType(instance->Model.MonitorType);
    MC6821_SetCartAutoStart(instance->Model.CartAutoStart);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl InitConfig(EmuState* emuState, CmdLineArguments* cmdArg)
  {
    HANDLE hr = NULL;
    int lasterror;

    LoadString(emuState->Resources, IDS_APP_TITLE, instance->Model.Release, MAX_LOADSTRING); //--A kind of "versioning" I guess

    GetIniFilePath(instance->IniFilePath, cmdArg->IniFile);

    instance->NumberOfSoundCards = GetSoundCardList(instance->SoundCards);

    //--Synch joysticks to config instance
    JoystickState* joystickState = GetJoystickState();

    joystickState->Left = &(instance->Model.Left);
    joystickState->Right = &(instance->Model.Right);

    ReadIniFile(emuState);

    SynchSystemWithConfig(emuState);
    ConfigureJoysticks();

    unsigned char soundCardIndex = GetSoundCardIndex(instance->Model.SoundCardName);
    SoundInit(emuState->WindowHandle, instance->SoundCards[soundCardIndex].Guid, instance->Model.AudioRate);

    //  Try to open the config file.  Create it if necessary.  Abort if failure.
    hr = CreateFile(instance->IniFilePath,
      GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ,
      NULL, OPEN_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);

    lasterror = GetLastError();

    if (hr == INVALID_HANDLE_VALUE) { // Fatal could not open ini file
      MessageBox(0, "Could not open ini file", "Error", 0);

      exit(0);
    }
    else {
      CloseHandle(hr);

      if (lasterror != ERROR_ALREADY_EXISTS) {
        WriteIniFile(emuState);
      }
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl ReadIniFile(EmuState* emuState)
  {
    instance->Model = LoadConfiguration(instance->IniFilePath);

    ValidateModel(instance->Model);

    vccKeyboardBuildRuntimeTable((keyboardlayout_e)(instance->Model.KeyMapIndex));

    InsertModule(emuState, instance->Model.ModulePath);	// Should this be here?

    if (instance->Model.RememberSize) {
      SetWindowSize(instance->Model.WindowSizeX, instance->Model.WindowSizeY);
    }
    else {
      SetWindowSize(640, 480);
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl WriteIniFile(EmuState* emuState)
  {
    instance->Model.WindowSizeX = (unsigned short)emuState->WindowSize.x;
    instance->Model.WindowSizeY = (unsigned short)emuState->WindowSize.y;

    GetCurrentModule(instance->Model.ModulePath);

    ValidateModel(instance->Model);

    SaveConfiguration(instance->Model, instance->IniFilePath);
  }
}
