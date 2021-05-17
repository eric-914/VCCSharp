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

#include "VccState.h"

#include "macros.h"

#include "resource.h"
#include "JoystickModel.h"

#include "ConfigConstants.h"

using namespace std;

ConfigState* InitializeInstance(ConfigState*, ConfigModel*, JoystickModel*, JoystickModel*);
JoystickModel* InitializeModel(JoystickModel* p);

static ConfigState* instance = InitializeInstance(new ConfigState(), new ConfigModel(), InitializeModel(new JoystickModel()), InitializeModel(new JoystickModel()));
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

JoystickModel* InitializeModel(JoystickModel* p) {
  p->DiDevice = 0;
  p->HiRes = 0;
  p->UseMouse = 0;

  return p;
}

ConfigState* InitializeInstance(ConfigState* p, ConfigModel* m, JoystickModel* l, JoystickModel* r) {
  p->Model = model = m;
  model->Left = left = l;
  model->Right = right = r;

  OutputDebugString("Here!");

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
  __declspec(dllexport) void __cdecl SetIniFilePath(char* path)
  {
    //  Path must be to an existing ini file
    strcpy(instance->IniFilePath, path);
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
    SetDialogTapeFileName(instance->hDlgTape, instance->TapeFileName);
  }
}
static HWND hWndTabDialog;

void MainInitDialog(HWND hDlg) {
  TCITEM tabs = TCITEM();

  ConfigState* configState = GetConfigState();
  EmuState* emuState = GetEmuState();

  InitCommonControls();

  //configModel = configState->Model;

  hWndTabDialog = GetDlgItem(hDlg, IDC_CONFIGTAB); //get handle of Tabbed Dialog

  //get handles to all the sub panels in the control
  configState->hWndConfig[0] = CreateDialog(emuState->Resources, MAKEINTRESOURCE(IDD_CASSETTE), hWndTabDialog, (DLGPROC)CreateTapeConfigDialogCallback);

  //Set the title text for all tabs
  for (unsigned char tabCount = 0; tabCount < TABS; tabCount++)
  {
    tabs.mask = TCIF_TEXT | TCIF_IMAGE;
    tabs.iImage = -1;
    tabs.pszText = TabTitles[tabCount];

    TabCtrl_InsertItem(hWndTabDialog, tabCount, &tabs);
  }

  TabCtrl_SetCurSel(hWndTabDialog, 0);	//Set Initial Tab to 0

  for (unsigned char tabCount = 0; tabCount < TABS; tabCount++) {	//Hide All the Sub Panels
    ShowWindow(configState->hWndConfig[tabCount], SW_HIDE);
  }

  SetWindowPos(configState->hWndConfig[0], HWND_TOP, 10, 30, 0, 0, SWP_NOSIZE | SWP_SHOWWINDOW);
}

void MainNotify(WPARAM wParam) {
  ConfigState* configState = GetConfigState();

  if ((LOWORD(wParam)) == IDC_CONFIGTAB) {
    unsigned char selectedTab = TabCtrl_GetCurSel(hWndTabDialog);

    for (unsigned char tabCount = 0; tabCount < TABS; tabCount++) {
      ShowWindow(configState->hWndConfig[tabCount], SW_HIDE);
    }

    SetWindowPos(configState->hWndConfig[selectedTab], HWND_TOP, 10, 30, 0, 0, SWP_NOSIZE | SWP_SHOWWINDOW);
  }
}

void CheckAudioChange(EmuState* emuState, ConfigModel* current, SoundCardList* soundCards) {
  //unsigned char currentSoundCardIndex = GetSoundCardIndex(current->SoundCardName);
  //unsigned char tempSoundCardIndex = GetSoundCardIndex(temp->SoundCardName);
  unsigned char tempSoundCardIndex = GetSoundCardIndex(current->SoundCardName);

  //TODO: current and temp are pointing to the same object.
  //if ((currentSoundCardIndex != tempSoundCardIndex) || (current->AudioRate != temp->AudioRate)) {
  //SoundInit(emuState->WindowHandle, soundCards[tempSoundCardIndex].Guid, temp->AudioRate);
  SoundInit(emuState->WindowHandle, soundCards[tempSoundCardIndex].Guid, current->AudioRate);
  //}
}

void MainCommandApply(ConfigModel* model) {
  ConfigState* configState = GetConfigState();
  EmuState* emuState = GetEmuState();

  JoystickModel* left = configState->Model->Left;
  JoystickModel* right = configState->Model->Right;

  emuState->ResetPending = RESET_CLS_SYNCH;

  //if ((configState->Model->RamSize != configModel->RamSize) || (configState->Model->CpuType != configModel->CpuType)) {
  emuState->ResetPending = RESET_HARD;
  //}

  //CheckAudioChange(emuState, configState->Model, configModel, configState->SoundCards);
  CheckAudioChange(emuState, configState->Model, configState->SoundCards);

  //configState->Model = configModel;

  vccKeyboardBuildRuntimeTable(configState->Model->KeyMapIndex);

  SetStickNumbers(left->DiDevice, right->DiDevice);
}

void MainCommandCancel(HWND hDlg) {
  ConfigState* configState = GetConfigState();
  EmuState* emuState = GetEmuState();

  for (unsigned char temp = 0; temp < TABS; temp++)
  {
    DestroyWindow(configState->hWndConfig[temp]);
  }

#ifdef CONFIG_DIALOG_MODAL
  EndDialog(hDlg, LOWORD(wParam));
#else
  DestroyWindow(hDlg);
#endif

  emuState->ConfigDialog = NULL;
  }

void MainCommandOk(HWND hDlg, ConfigModel* model) {
  ConfigState* configState = GetConfigState();

  configState->hDlgBar = NULL;
  configState->hDlgTape = NULL;

  MainCommandApply(model);
  MainCommandCancel(hDlg);
}
