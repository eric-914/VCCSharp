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

#include "systemstate.h"
#include "fileoperations.h"
#include "CmdLineArguments.h"
#include "ConfigDialogCallbacks.h"
#include "ConfigIO.h"

#include "macros.h"

using namespace std;

const unsigned char TranslateScan2Disp[SCAN_TRANS_COUNT] = { 0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,32,38,20,33,35,40,36,24,30,31,42,43,55,52,16,34,19,21,22,23,25,26,27,45,46,0,51,44,41,39,18,37,17,29,28,47,48,49,51,0,53,54,50,66,67,0,0,0,0,0,0,0,0,0,0,58,64,60,0,62,0,63,0,59,65,61,56,57 };

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
  p->PrtMon = 0;
  p->TapeCounter = 0;
  p->TextMode = 1;
  p->TapeMode = STOP;

  p->hDlgBar = NULL;
  p->hDlgTape = NULL;

  strcpy(p->AppName, "");
  strcpy(p->ExecDirectory, "");
  strcpy(p->IniFilePath, "");
  strcpy(p->OutBuffer, "");
  strcpy(p->SerialCaptureFile, "");
  strcpy(p->TapeFileName, "");

  ARRAYCOPY(TranslateScan2Disp);

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

void AdjustOverclockSpeed(SystemState* systemState, unsigned char change) {
  unsigned char cpuMultiplier = instance->Model.CPUMultiplier + change;

  if (cpuMultiplier < 2 || cpuMultiplier > instance->Model.MaxOverclock)
  {
    return;
  }

  // Send updates to the dialog if it's open.
  if (systemState->ConfigDialog != NULL)
  {
    HWND hDlg = instance->hWndConfig[1];

    SetDialogCpuMultiplier(hDlg, cpuMultiplier);
  }

  instance->Model.CPUMultiplier = cpuMultiplier;

  systemState->ResetPending = 4; // Without this, changing the config does nothing.
}

extern "C" {
  __declspec(dllexport) void __cdecl BuildTransDisp2ScanTable()
  {
    for (int i = 0; i < SCAN_TRANS_COUNT; i++) {
      for (int j = SCAN_TRANS_COUNT - 1; j >= 0; j--) {
        if (j == instance->TranslateScan2Disp[i]) {
          instance->TranslateDisp2Scan[j] = (unsigned char)i;
        }
      }
    }

    instance->TranslateDisp2Scan[0] = 0;

    // Left and Right Shift
    instance->TranslateDisp2Scan[51] = DIK_LSHIFT;
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl TranslateDisplay2Scan(LRESULT x)
  {
    assert(x >= 0 && x < SCAN_TRANS_COUNT);

    return instance->TranslateDisp2Scan[x];
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl TranslateScan2Display(int x)
  {
    assert(x >= 0 && x < SCAN_TRANS_COUNT);

    return instance->TranslateScan2Disp[x];
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
    return instance->AppDataPath;
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
  __declspec(dllexport) void __cdecl IncreaseOverclockSpeed(SystemState* systemState)
  {
    AdjustOverclockSpeed(systemState, 1);
  }
}

/**
 * Decrease the overclock speed, as seen after a POKE 65497,0.
 *
 * Setting this value to 0 will make the emulator pause.  Hence the minimum of 2.
 */
extern "C" {
  __declspec(dllexport) void __cdecl DecreaseOverclockSpeed(SystemState* systemState)
  {
    AdjustOverclockSpeed(systemState, -1);
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
  __declspec(dllexport) void __cdecl RefreshJoystickStatus(void)
  {
    bool temp = false;

    JoystickState* joystickState = GetJoystickState();

    instance->NumberOfJoysticks = EnumerateJoysticks();

    for (unsigned char index = 0; index < instance->NumberOfJoysticks; index++) {
      temp = InitJoyStick(index);
    }

    if (joystickState->Right.DiDevice > (instance->NumberOfJoysticks - 1)) {
      joystickState->Right.DiDevice = 0;
    }

    if (joystickState->Left.DiDevice > (instance->NumberOfJoysticks - 1)) {
      joystickState->Left.DiDevice = 0;
    }

    SetStickNumbers(joystickState->Left.DiDevice, joystickState->Right.DiDevice);

    if (instance->NumberOfJoysticks == 0)	//Use Mouse input if no Joysticks present
    {
      if (joystickState->Left.UseMouse == 3) {
        joystickState->Left.UseMouse = 1;
      }

      if (joystickState->Right.UseMouse == 3) {
        joystickState->Right.UseMouse = 1;
      }
    }
  }
}

extern "C" {
  __declspec(dllexport) int __cdecl SelectSerialCaptureFile(SystemState* systemState, char* filename)
  {
    OPENFILENAME ofn;
    char dummy[MAX_PATH] = "";
    char tempFileName[MAX_PATH] = "";
    char* serialCaptureFilePath = instance->Model.SerialCaptureFilePath;

    memset(&ofn, 0, sizeof(ofn));

    ofn.lStructSize = sizeof(OPENFILENAME);
    ofn.hwndOwner = systemState->WindowHandle; // GetTopWindow(NULL);
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
  __declspec(dllexport) void __cdecl SynchSystemWithConfig(SystemState* systemState)
  {
    SetPaletteType();
    SetResize(instance->Model.AllowResize);
    SetAspect(instance->Model.ForceAspect);
    SetScanLines(systemState, instance->Model.ScanLines);
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
  __declspec(dllexport) void __cdecl LoadConfig(SystemState* systemState, CmdLineArguments cmdArg)
  {
    HANDLE hr = NULL;
    int lasterror;
    char iniFileName[] = "Vcc.ini";

    BuildTransDisp2ScanTable();

    LoadString(systemState->Resources, IDS_APP_TITLE, instance->AppName, MAX_LOADSTRING);
    GetModuleFileName(NULL, instance->ExecDirectory, MAX_PATH);

    FilePathRemoveFileSpec(instance->ExecDirectory);

    if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_APPDATA, NULL, 0, instance->AppDataPath))) {
      OutputDebugString(instance->AppDataPath);
    }

    strcat(instance->AppDataPath, "\\VCC");

    if (_mkdir(instance->AppDataPath) != 0) {
      OutputDebugString("Unable to create VCC config folder.");
    }

    if (*cmdArg.IniFile) {
      GetFullPathNameA(cmdArg.IniFile, MAX_PATH, instance->IniFilePath, 0);
    }
    else {
      strcpy(instance->IniFilePath, instance->AppDataPath);
      strcat(instance->IniFilePath, "\\");
      strcat(instance->IniFilePath, iniFileName);
    }

    systemState->ScanLines = 0;

    instance->NumberOfSoundCards = GetSoundCardList(instance->SoundCards);

    ReadIniFile(systemState);

    SynchSystemWithConfig(systemState);
    RefreshJoystickStatus();

    unsigned char soundCardIndex = GetSoundCardIndex(instance->Model.SoundCardName);
    SoundInit(systemState->WindowHandle, instance->SoundCards[soundCardIndex].Guid, instance->Model.AudioRate);

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
        WriteIniFile(*systemState);
      }
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl ReadIniFile(SystemState* systemState)
  {
    instance->Model = LoadConfiguration(instance->IniFilePath);

    if (instance->Model.KeyMapIndex > 3) {
      instance->Model.KeyMapIndex = 0;	//Default to DECB Mapping
    }

    vccKeyboardBuildRuntimeTable((keyboardlayout_e)(instance->Model.KeyMapIndex));

    FileCheckPath(instance->Model.ModulePath);
    FileCheckPath(instance->Model.ExternalBasicImage);

    JoystickState* joystickState = GetJoystickState();

    joystickState->Left = instance->Model.Left;
    joystickState->Right = instance->Model.Right;

    InsertModule(systemState, instance->Model.ModulePath);	// Should this be here?

    instance->Model.AllowResize = 1; //Checkbox removed. Remove this from the ini? 

    if (instance->Model.RememberSize) {
      SetWindowSize(instance->Model.WindowSizeX, instance->Model.WindowSizeY);
    }
    else {
      SetWindowSize(640, 480);
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl WriteIniFile(SystemState systemState)
  {
    instance->Model.AllowResize = 1;
    instance->Model.WindowSizeX = systemState.WindowSizeX;
    instance->Model.WindowSizeY = systemState.WindowSizeY;

    GetCurrentModule(instance->Model.ModulePath);
    FileValidatePath(instance->Model.ModulePath);

    JoystickState* joystickState = GetJoystickState();

    instance->Model.Left = joystickState->Left;
    instance->Model.Right = joystickState->Right;

    strcpy(instance->Model.Release, instance->AppName); //--Set "version" I guess

    SaveConfiguration(instance->Model, instance->IniFilePath);
  }
}
