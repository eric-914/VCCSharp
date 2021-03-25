#include <windows.h>
#include <string>
#include <ShlObj.h>
#include <richedit.h>

#include "../resources/resource.h"

#include "Config.h"
#include "Audio.h"
#include "MC6821.h"
#include "Keyboard.h"
#include "Cassette.h"
#include "Joystick.h"
#include "JoystickModel.h"
#include "Emu.h"

#include "VccState.h"

#include "ConfigConstants.h"

static CHARFORMAT CounterText;
static CHARFORMAT ModeText;

static ConfigModel* configModel;

extern "C" {
  __declspec(dllexport) void SetDialogTapeCounter(HWND hDlg, unsigned int tapeCounter) {
    char temp[MAX_PATH];

    sprintf(temp, "%i", tapeCounter);

    SendDlgItemMessage(hDlg, IDC_TCOUNT, WM_SETTEXT, strlen(temp), (LPARAM)(LPCSTR)(temp));
  }
}

extern "C" {
  __declspec(dllexport) void SetDialogTapeMode(HWND hDlg, unsigned char tapeMode) {
    SendDlgItemMessage(hDlg, IDC_MODE, WM_SETTEXT, strlen(TapeModes[tapeMode]), (LPARAM)(LPCSTR)(TapeModes[tapeMode]));

    //--Update visual state
    switch (tapeMode)
    {
    case REC:
      SendDlgItemMessage(hDlg, IDC_MODE, EM_SETBKGNDCOLOR, 0, (LPARAM)RGB(0xAF, 0, 0));
      break;

    case PLAY:
      SendDlgItemMessage(hDlg, IDC_MODE, EM_SETBKGNDCOLOR, 0, (LPARAM)RGB(0, 0xAF, 0));
      break;

    default:
      SendDlgItemMessage(hDlg, IDC_MODE, EM_SETBKGNDCOLOR, 0, (LPARAM)RGB(0, 0, 0));
      break;
    }
  }
}

extern "C" {
  __declspec(dllexport) void SetDialogTapeFileName(HWND hDlg, char* tapeFileName) {
    SendDlgItemMessage(hDlg, IDC_TAPEFILE, WM_SETTEXT, strlen(tapeFileName), (LPARAM)(LPCSTR)(tapeFileName));
  }
}

extern "C" {
  __declspec(dllexport) LRESULT CALLBACK CreateTapeConfigDialogCallback(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
  {
    ConfigState* configState = GetConfigState();

    CounterText.cbSize = sizeof(CHARFORMAT);
    CounterText.dwMask = CFM_BOLD | CFM_COLOR;
    CounterText.dwEffects = CFE_BOLD;
    CounterText.crTextColor = RGB(255, 255, 255);

    ModeText.cbSize = sizeof(CHARFORMAT);
    ModeText.dwMask = CFM_BOLD | CFM_COLOR;
    ModeText.dwEffects = CFE_BOLD;
    ModeText.crTextColor = RGB(255, 0, 0);

    switch (message)
    {
    case WM_INITDIALOG:
      configState->TapeCounter = GetTapeCounter();
      GetTapeName(configState->TapeFileName);

      SetDialogTapeCounter(hDlg, configState->TapeCounter);
      SetDialogTapeMode(hDlg, configState->TapeMode);
      SetDialogTapeFileName(hDlg, configState->TapeFileName);

      SendDlgItemMessage(hDlg, IDC_TAPEFILE, WM_SETTEXT, strlen(configState->TapeFileName), (LPARAM)(LPCSTR)(configState->TapeFileName));
      SendDlgItemMessage(hDlg, IDC_TCOUNT, EM_SETBKGNDCOLOR, 0, (LPARAM)RGB(0, 0, 0));
      SendDlgItemMessage(hDlg, IDC_TCOUNT, EM_SETCHARFORMAT, SCF_ALL, (LPARAM)&CounterText);
      SendDlgItemMessage(hDlg, IDC_MODE, EM_SETBKGNDCOLOR, 0, (LPARAM)RGB(0, 0, 0));
      SendDlgItemMessage(hDlg, IDC_MODE, EM_SETCHARFORMAT, SCF_ALL, (LPARAM)&CounterText);

      configState->hDlgTape = hDlg;

      break;

    case WM_COMMAND:
      switch (LOWORD(wParam))
      {
      case IDC_PLAY:
        configState->TapeMode = PLAY;
        SetTapeMode(PLAY);
        break;

      case IDC_REC:
        configState->TapeMode = REC;
        SetTapeMode(REC);
        break;

      case IDC_STOP:
        configState->TapeMode = STOP;
        SetTapeMode(STOP);
        break;

      case IDC_EJECT:
        configState->TapeMode = EJECT;
        SetTapeMode(EJECT);
        break;

      case IDC_RESET:
        configState->TapeCounter = 0;
        SetTapeCounter(configState->TapeCounter);
        break;

      case IDC_TBROWSE:
        LoadTape();

        configState->TapeCounter = 0;

        SetTapeCounter(configState->TapeCounter);

        break;
      }

      break;
    }

    return(0);
  }
}

extern "C" {
  __declspec(dllexport) LRESULT CALLBACK CreateMainConfigDialogCallback(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
  {
    ConfigState* configState = GetConfigState();
    configModel = configState->Model;

    switch (message)
    {
    case WM_INITDIALOG:
      MainInitDialog(hDlg);
      break;

    case WM_NOTIFY:
      MainNotify(wParam);
      break;

    case WM_COMMAND:
      switch (LOWORD(wParam))
      {
      case IDOK:
        MainCommandOk(hDlg, configModel);
        break;

      case IDAPPLY:
        MainCommandApply(configModel);
        break;

      case IDCANCEL:
        MainCommandCancel(hDlg);
        break;
      }

      break;
    }

    return FALSE;
  }
}
