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
  __declspec(dllexport) void SetDialogTapeFileName(HWND hDlg, char* tapeFileName) {
    SendDlgItemMessage(hDlg, IDC_TAPEFILE, WM_SETTEXT, strlen(tapeFileName), (LPARAM)(LPCSTR)(tapeFileName));
  }
}

extern "C" {
  __declspec(dllexport) void TapeInitialize(HWND hDlg) {
    ConfigState* configState = GetConfigState();

    configState->TapeCounter = GetTapeCounter();
    GetTapeName(configState->TapeFileName);

    SetDialogTapeCounter(hDlg, configState->TapeCounter);
    SetDialogTapeFileName(hDlg, configState->TapeFileName);

    SendDlgItemMessage(hDlg, IDC_TAPEFILE, WM_SETTEXT, strlen(configState->TapeFileName), (LPARAM)(LPCSTR)(configState->TapeFileName));
    SendDlgItemMessage(hDlg, IDC_TCOUNT, EM_SETBKGNDCOLOR, 0, (LPARAM)RGB(0, 0, 0));
    SendDlgItemMessage(hDlg, IDC_TCOUNT, EM_SETCHARFORMAT, SCF_ALL, (LPARAM)&CounterText);

    configState->hDlgTape = hDlg;
  }
}

extern "C" {
  __declspec(dllexport) LRESULT CALLBACK CreateTapeConfigDialogCallback(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
  {
    switch (message)
    {
    case WM_INITDIALOG:
      TapeInitialize(hDlg);
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
