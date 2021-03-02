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

#include "macros.h"

using namespace std;

const unsigned short int Cpuchoice[2] = { IDC_6809, IDC_6309 };
const unsigned short int Monchoice[2] = { IDC_COMPOSITE, IDC_RGB };
const unsigned short int PaletteChoice[2] = { IDC_ORG_PALETTE, IDC_UPD_PALETTE };
const unsigned short int Ramchoice[4] = { IDC_128K, IDC_512K, IDC_2M, IDC_8M };
const unsigned int LeftJoystickEmulation[3] = { IDC_LEFTSTANDARD, IDC_LEFTTHIRES, IDC_LEFTCCMAX };
const unsigned int RightJoystickEmulation[3] = { IDC_RIGHTSTANDARD, IDC_RIGHTTHRES, IDC_RIGHTCCMAX };
const char Tmodes[4][10] = { "STOP","PLAY","REC","STOP" };
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
  p->Tmode = STOP;

  p->hDlgBar = NULL;
  p->hDlgTape = NULL;

  strcpy(p->AppName, "");
  strcpy(p->ExecDirectory, "");
  strcpy(p->IniFilePath, "");
  strcpy(p->OutBuffer, "");
  strcpy(p->SerialCaptureFile, "");
  strcpy(p->TapeFileName, "");

  ARRAYCOPY(Cpuchoice);
  ARRAYCOPY(LeftJoystickEmulation);
  ARRAYCOPY(Monchoice);
  ARRAYCOPY(PaletteChoice);
  ARRAYCOPY(Ramchoice);
  ARRAYCOPY(RightJoystickEmulation);
  ARRAYCOPY(TranslateScan2Disp);

  STRARRAYCOPY(Tmodes);

  return p;
}

void SaveConfiguration(ConfigModel model, char* iniFilePath);
ConfigModel LoadConfiguration(char* iniFilePath);

extern "C" {
  __declspec(dllexport) unsigned char __cdecl TranslateScan2Display(int x)
  {
    assert(x >= 0 && x < SCAN_TRANS_COUNT);

    return instance->TranslateScan2Disp[x];
  }
}

extern "C" {
  __declspec(dllexport) char* __cdecl BasicRomName(void)
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

extern "C" {
  __declspec(dllexport) void __cdecl GetProfileText(LPCSTR lpAppName, LPCSTR lpKeyName, LPCSTR lpDefault, LPSTR lpReturnedString) {
    GetPrivateProfileString(lpAppName, lpKeyName, lpDefault, lpReturnedString, MAX_PATH, instance->IniFilePath);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetProfileText(LPCSTR lpAppName, LPCSTR lpKeyName, LPCSTR lpString) {
    WritePrivateProfileString(lpAppName, lpKeyName, lpString, instance->IniFilePath);
  }
}

extern "C" {
  __declspec(dllexport) unsigned short __cdecl GetProfileShort(LPCSTR lpAppName, LPCSTR lpKeyName, int nDefault) {
    return GetPrivateProfileInt(lpAppName, lpKeyName, nDefault, instance->IniFilePath);
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl GetProfileByte(LPCSTR lpAppName, LPCSTR lpKeyName, int nDefault) {
    return GetPrivateProfileInt(lpAppName, lpKeyName, nDefault, instance->IniFilePath);
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

    SendDlgItemMessage(hDlg, IDC_CLOCKSPEED, TBM_SETPOS, TRUE, cpuMultiplier);

    sprintf(instance->OutBuffer, "%2.3f Mhz", (float)(cpuMultiplier) * 0.894);

    SendDlgItemMessage(hDlg, IDC_CLOCKDISPLAY, WM_SETTEXT, strlen(instance->OutBuffer), (LPARAM)(LPCSTR)(instance->OutBuffer));
  }

  instance->Model.CPUMultiplier = cpuMultiplier;

  systemState->ResetPending = 4; // Without this, changing the config does nothing.
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
  __declspec(dllexport) void __cdecl UpdateTapeCounter(unsigned int counter, unsigned char tapeMode)
  {
    if (instance->hDlgTape == NULL) {
      return;
    }

    instance->TapeCounter = counter;
    instance->Tmode = tapeMode;

    sprintf(instance->OutBuffer, "%i", instance->TapeCounter);

    SendDlgItemMessage(instance->hDlgTape, IDC_TCOUNT, WM_SETTEXT, strlen(instance->OutBuffer), (LPARAM)(LPCSTR)(instance->OutBuffer));
    SendDlgItemMessage(instance->hDlgTape, IDC_MODE, WM_SETTEXT, strlen(instance->Tmodes[instance->Tmode]), (LPARAM)(LPCSTR)(instance->Tmodes[instance->Tmode]));

    GetTapeName(instance->TapeFileName);
    FilePathStripPath(instance->TapeFileName);

    SendDlgItemMessage(instance->hDlgTape, IDC_TAPEFILE, WM_SETTEXT, strlen(instance->TapeFileName), (LPARAM)(LPCSTR)(instance->TapeFileName));

    switch (instance->Tmode)
    {
    case REC:
      SendDlgItemMessage(instance->hDlgTape, IDC_MODE, EM_SETBKGNDCOLOR, 0, (LPARAM)RGB(0xAF, 0, 0));
      break;

    case PLAY:
      SendDlgItemMessage(instance->hDlgTape, IDC_MODE, EM_SETBKGNDCOLOR, 0, (LPARAM)RGB(0, 0xAF, 0));
      break;

    default:
      SendDlgItemMessage(instance->hDlgTape, IDC_MODE, EM_SETBKGNDCOLOR, 0, (LPARAM)RGB(0, 0, 0));
      break;
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl WriteIniFile(void)
  {
    POINT tp = GetCurrentWindowSize();

    instance->Model.AllowResize = 1;

    GetCurrentModule(instance->Model.ModulePath);
    FileValidatePath(instance->Model.ModulePath);
    FileValidatePath(instance->Model.ExternalBasicImage);

    JoystickState* joystickState = GetJoystickState();

    instance->Model.Left = joystickState->Left;
    instance->Model.Right = joystickState->Right;

    strcpy(instance->Model.Release, instance->AppName); //--Set "version" I guess

    SaveConfiguration(instance->Model, instance->IniFilePath);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl UpdateSoundBar(unsigned short left, unsigned short right)
  {
    if (instance->hDlgBar == NULL) {
      return;
    }

    SendDlgItemMessage(instance->hDlgBar, IDC_PROGRESSLEFT, PBM_SETPOS, left >> 8, 0);
    SendDlgItemMessage(instance->hDlgBar, IDC_PROGRESSRIGHT, PBM_SETPOS, right >> 8, 0);
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

extern "C" {
  __declspec(dllexport) unsigned char __cdecl ReadIniFile(SystemState* systemState)
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

    return(0);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl UpdateConfig(SystemState* systemState)
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

    UpdateConfig(systemState);
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
        WriteIniFile();
      }
    }
  }
}

void SaveConfiguration(ConfigModel model, char* iniFilePath) {
  POINT tp = GetCurrentWindowSize();

  //[Version]
  WritePrivateProfileString("Version", "Release", model.Release, iniFilePath); //## Write-only ##//

  //[CPU]
  FileWritePrivateProfileInt("CPU", "CPUMultiplier", model.CPUMultiplier, iniFilePath);
  FileWritePrivateProfileInt("CPU", "FrameSkip", model.FrameSkip, iniFilePath);
  FileWritePrivateProfileInt("CPU", "SpeedThrottle", model.SpeedThrottle, iniFilePath);
  FileWritePrivateProfileInt("CPU", "CpuType", model.CpuType, iniFilePath);
  FileWritePrivateProfileInt("CPU", "MaxOverClock", model.MaxOverclock, iniFilePath);

  //[Audio]
  WritePrivateProfileString("Audio", "SoundCardName", model.SoundCardName, iniFilePath);
  FileWritePrivateProfileInt("Audio", "AudioRate", model.AudioRate, iniFilePath);

  //[Video]
  FileWritePrivateProfileInt("Video", "MonitorType", model.MonitorType, iniFilePath);
  FileWritePrivateProfileInt("Video", "PaletteType", model.PaletteType, iniFilePath);
  FileWritePrivateProfileInt("Video", "ScanLines", model.ScanLines, iniFilePath);
  FileWritePrivateProfileInt("Video", "AllowResize", model.AllowResize, iniFilePath);
  FileWritePrivateProfileInt("Video", "ForceAspect", model.ForceAspect, iniFilePath);
  FileWritePrivateProfileInt("Video", "RememberSize", model.RememberSize, iniFilePath);
  FileWritePrivateProfileInt("Video", "WindowSizeX", tp.x, iniFilePath);
  FileWritePrivateProfileInt("Video", "WindowSizeY", tp.y, iniFilePath);

  //[Memory]
  FileWritePrivateProfileInt("Memory", "RamSize", model.RamSize, iniFilePath);
  WritePrivateProfileString("Memory", "ExternalBasicImage", model.ExternalBasicImage, iniFilePath);

  //[Misc]
  FileWritePrivateProfileInt("Misc", "AutoStart", model.AutoStart, iniFilePath);
  FileWritePrivateProfileInt("Misc", "CartAutoStart", model.CartAutoStart, iniFilePath);
  FileWritePrivateProfileInt("Misc", "KeyMapIndex", model.KeyMapIndex, iniFilePath);

  //[Module]
  WritePrivateProfileString("Module", "ModulePath", model.ModulePath, iniFilePath);

  //[LeftJoyStick]
  FileWritePrivateProfileInt("LeftJoyStick", "UseMouse", model.Left.UseMouse, iniFilePath);
  FileWritePrivateProfileInt("LeftJoyStick", "Left", model.Left.Left, iniFilePath);
  FileWritePrivateProfileInt("LeftJoyStick", "Right", model.Left.Right, iniFilePath);
  FileWritePrivateProfileInt("LeftJoyStick", "Up", model.Left.Up, iniFilePath);
  FileWritePrivateProfileInt("LeftJoyStick", "Down", model.Left.Down, iniFilePath);
  FileWritePrivateProfileInt("LeftJoyStick", "Fire1", model.Left.Fire1, iniFilePath);
  FileWritePrivateProfileInt("LeftJoyStick", "Fire2", model.Left.Fire2, iniFilePath);
  FileWritePrivateProfileInt("LeftJoyStick", "DiDevice", model.Left.DiDevice, iniFilePath);
  FileWritePrivateProfileInt("LeftJoyStick", "HiResDevice", model.Left.HiRes, iniFilePath);

  //[RightJoyStick]
  FileWritePrivateProfileInt("RightJoyStick", "UseMouse", model.Right.UseMouse, iniFilePath);
  FileWritePrivateProfileInt("RightJoyStick", "Left", model.Right.Left, iniFilePath);
  FileWritePrivateProfileInt("RightJoyStick", "Right", model.Right.Right, iniFilePath);
  FileWritePrivateProfileInt("RightJoyStick", "Up", model.Right.Up, iniFilePath);
  FileWritePrivateProfileInt("RightJoyStick", "Down", model.Right.Down, iniFilePath);
  FileWritePrivateProfileInt("RightJoyStick", "Fire1", model.Right.Fire1, iniFilePath);
  FileWritePrivateProfileInt("RightJoyStick", "Fire2", model.Right.Fire2, iniFilePath);
  FileWritePrivateProfileInt("RightJoyStick", "DiDevice", model.Right.DiDevice, iniFilePath);
  FileWritePrivateProfileInt("RightJoyStick", "HiResDevice", model.Right.HiRes, iniFilePath);

  //[DefaultPaths]
  WritePrivateProfileString("DefaultPaths", "CassPath", model.CassPath, iniFilePath);
  WritePrivateProfileString("DefaultPaths", "PakPath", model.PakPath, iniFilePath);
  WritePrivateProfileString("DefaultPaths", "FloppyPath", model.FloppyPath, iniFilePath);
  WritePrivateProfileString("DefaultPaths", "CoCoRomPath", model.CoCoRomPath, iniFilePath);
  WritePrivateProfileString("DefaultPaths", "SerialCaptureFilePath", model.SerialCaptureFilePath, iniFilePath);

  //--Flush .ini file
  WritePrivateProfileString(NULL, NULL, NULL, iniFilePath);
}

ConfigModel LoadConfiguration(char* iniFilePath) {
  ConfigModel model = ConfigModel();

  //[Version]

  //[CPU]
  model.CPUMultiplier = GetPrivateProfileInt("CPU", "CPUMultiplier", 2, iniFilePath);
  model.FrameSkip = GetPrivateProfileInt("CPU", "FrameSkip", 1, iniFilePath);
  model.SpeedThrottle = GetPrivateProfileInt("CPU", "SpeedThrottle", 1, iniFilePath);
  model.CpuType = GetPrivateProfileInt("CPU", "CpuType", 0, iniFilePath);
  model.MaxOverclock = GetPrivateProfileInt("CPU", "MaxOverClock", 227, iniFilePath);

  //[Audio]
  model.AudioRate = GetPrivateProfileInt("Audio", "AudioRate", 3, iniFilePath);
  GetPrivateProfileString("Audio", "SoundCardName", "", model.SoundCardName, 63, iniFilePath);

  //[Video]
  model.MonitorType = GetPrivateProfileInt("Video", "MonitorType", 1, iniFilePath);
  model.PaletteType = GetPrivateProfileInt("Video", "PaletteType", 1, iniFilePath);
  model.ScanLines = GetPrivateProfileInt("Video", "ScanLines", 0, iniFilePath);
  model.AllowResize = GetPrivateProfileInt("Video", "AllowResize", 0, iniFilePath);
  model.ForceAspect = GetPrivateProfileInt("Video", "ForceAspect", 0, iniFilePath);
  model.RememberSize = GetPrivateProfileInt("Video", "RememberSize", 0, iniFilePath);
  model.WindowSizeX = GetPrivateProfileInt("Video", "WindowSizeX", 640, iniFilePath);
  model.WindowSizeY = GetPrivateProfileInt("Video", "WindowSizeY", 480, iniFilePath);

  //[Memory]
  model.RamSize = GetPrivateProfileInt("Memory", "RamSize", 1, iniFilePath);
  GetPrivateProfileString("Memory", "ExternalBasicImage", "", model.ExternalBasicImage, MAX_PATH, iniFilePath);

  //[Misc]
  model.AutoStart = GetPrivateProfileInt("Misc", "AutoStart", 1, iniFilePath);
  model.CartAutoStart = GetPrivateProfileInt("Misc", "CartAutoStart", 1, iniFilePath);
  model.KeyMapIndex = GetPrivateProfileInt("Misc", "KeyMapIndex", 0, iniFilePath);

  //[Module]
  GetPrivateProfileString("Module", "ModulePath", "", model.ModulePath, MAX_PATH, iniFilePath);

  //[LeftJoyStick]
  model.Left.UseMouse = GetPrivateProfileInt("LeftJoyStick", "UseMouse", 1, iniFilePath);
  model.Left.Left = GetPrivateProfileInt("LeftJoyStick", "Left", 75, iniFilePath);
  model.Left.Right = GetPrivateProfileInt("LeftJoyStick", "Right", 77, iniFilePath);
  model.Left.Up = GetPrivateProfileInt("LeftJoyStick", "Up", 72, iniFilePath);
  model.Left.Down = GetPrivateProfileInt("LeftJoyStick", "Down", 80, iniFilePath);
  model.Left.Fire1 = GetPrivateProfileInt("LeftJoyStick", "Fire1", 59, iniFilePath);
  model.Left.Fire2 = GetPrivateProfileInt("LeftJoyStick", "Fire2", 60, iniFilePath);
  model.Left.DiDevice = GetPrivateProfileInt("LeftJoyStick", "DiDevice", 0, iniFilePath);
  model.Left.HiRes = GetPrivateProfileInt("LeftJoyStick", "HiResDevice", 0, iniFilePath);

  //[RightJoyStick]
  model.Right.UseMouse = GetPrivateProfileInt("RightJoyStick", "UseMouse", 1, iniFilePath);
  model.Right.Left = GetPrivateProfileInt("RightJoyStick", "Left", 75, iniFilePath);
  model.Right.Right = GetPrivateProfileInt("RightJoyStick", "Right", 77, iniFilePath);
  model.Right.Up = GetPrivateProfileInt("RightJoyStick", "Up", 72, iniFilePath);
  model.Right.Down = GetPrivateProfileInt("RightJoyStick", "Down", 80, iniFilePath);
  model.Right.Fire1 = GetPrivateProfileInt("RightJoyStick", "Fire1", 59, iniFilePath);
  model.Right.Fire2 = GetPrivateProfileInt("RightJoyStick", "Fire2", 60, iniFilePath);
  model.Right.DiDevice = GetPrivateProfileInt("RightJoyStick", "DiDevice", 0, iniFilePath);
  model.Right.HiRes = GetPrivateProfileInt("RightJoyStick", "HiResDevice", 0, iniFilePath);

  //[DefaultPaths]
  GetPrivateProfileString("DefaultPaths", "CassPath", "", model.CassPath, MAX_PATH, iniFilePath);
  GetPrivateProfileString("DefaultPaths", "FloppyPath", "", model.FloppyPath, MAX_PATH, iniFilePath);
  GetPrivateProfileString("DefaultPaths", "CoCoRomPath", "", model.CoCoRomPath, MAX_PATH, iniFilePath);
  GetPrivateProfileString("DefaultPaths", "SerialCaptureFilePath", "", model.SerialCaptureFilePath, MAX_PATH, iniFilePath);
  GetPrivateProfileString("DefaultPaths", "PakPath", "", model.PakPath, MAX_PATH, iniFilePath);

  return model;
}
