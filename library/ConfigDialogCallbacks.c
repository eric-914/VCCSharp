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

char* GetKeyName(int x)
{
  return keyNames[x];
}

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
  __declspec(dllexport) void SetDialogAudioBars(HWND hDlg, unsigned short left, unsigned short right) {
    SendDlgItemMessage(hDlg, IDC_PROGRESSLEFT, PBM_SETPOS, left >> 5, 0);
    SendDlgItemMessage(hDlg, IDC_PROGRESSRIGHT, PBM_SETPOS, right >> 5, 0);
  }
}

extern "C" {
  __declspec(dllexport) LRESULT CALLBACK CreateAudioConfigDialogCallback(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
  {
    ConfigState* configState = GetConfigState();

    switch (message)
    {
    case WM_INITDIALOG:
      configState->hDlgBar = hDlg;	//Save the handle to update sound bars

      SendDlgItemMessage(hDlg, IDC_PROGRESSLEFT, PBM_SETPOS, 0, 0);
      SendDlgItemMessage(hDlg, IDC_PROGRESSLEFT, PBM_SETRANGE32, 0, 0x7F);
      SendDlgItemMessage(hDlg, IDC_PROGRESSRIGHT, PBM_SETPOS, 0, 0);
      SendDlgItemMessage(hDlg, IDC_PROGRESSRIGHT, PBM_SETRANGE32, 0, 0x7F);
      SendDlgItemMessage(hDlg, IDC_PROGRESSLEFT, PBM_SETPOS, 0, 0);
      SendDlgItemMessage(hDlg, IDC_PROGRESSLEFT, PBM_SETBARCOLOR, 0, 0xFFFF);
      SendDlgItemMessage(hDlg, IDC_PROGRESSRIGHT, PBM_SETBARCOLOR, 0, 0xFFFF);
      SendDlgItemMessage(hDlg, IDC_PROGRESSLEFT, PBM_SETBKCOLOR, 0, 0);
      SendDlgItemMessage(hDlg, IDC_PROGRESSRIGHT, PBM_SETBKCOLOR, 0, 0);

      break;

    case WM_COMMAND:
      break;
    }

    return(0);
  }
}

extern "C" {
  __declspec(dllexport) LRESULT CALLBACK CreateBitBangerConfigDialogCallback(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
  {
    ConfigState* configState = GetConfigState();
    VccState* vccState = GetVccState();

    switch (message)
    {
    case WM_INITDIALOG:
      if (!strlen(configState->SerialCaptureFile)) {
        strcpy(configState->SerialCaptureFile, "No Capture File");
      }

      SendDlgItemMessage(hDlg, IDC_SERIALFILE, WM_SETTEXT, strlen(configState->SerialCaptureFile), (LPARAM)(LPCSTR)(configState->SerialCaptureFile));
      SendDlgItemMessage(hDlg, IDC_LF, BM_SETCHECK, configState->TextMode, 0);
      SendDlgItemMessage(hDlg, IDC_PRINTMON, BM_SETCHECK, configState->PrintMonitorWindow, 0);

      break;

    case WM_COMMAND:

      switch (LOWORD(wParam))
      {
      case IDC_OPEN:
        SelectSerialCaptureFile(GetEmuState(), configState->SerialCaptureFile);

        SendDlgItemMessage(hDlg, IDC_SERIALFILE, WM_SETTEXT, strlen(configState->SerialCaptureFile), (LPARAM)(LPCSTR)(configState->SerialCaptureFile));

        break;

      case IDC_CLOSE:
        MC6821_ClosePrintFile();
        strcpy(configState->SerialCaptureFile, "No Capture File");

        SendDlgItemMessage(hDlg, IDC_SERIALFILE, WM_SETTEXT, strlen(configState->SerialCaptureFile), (LPARAM)(LPCSTR)(configState->SerialCaptureFile));

        configState->PrintMonitorWindow = FALSE;

        MC6821_SetMonState(configState->PrintMonitorWindow);

        SendDlgItemMessage(hDlg, IDC_PRINTMON, BM_SETCHECK, configState->PrintMonitorWindow, 0);

        break;

      case IDC_LF:
        configState->TextMode = (char)SendDlgItemMessage(hDlg, IDC_LF, BM_GETCHECK, 0, 0);

        MC6821_SetSerialParams(configState->TextMode);

        break;

      case IDC_PRINTMON:
        configState->PrintMonitorWindow = (char)SendDlgItemMessage(hDlg, IDC_PRINTMON, BM_GETCHECK, 0, 0);

        MC6821_SetMonState(configState->PrintMonitorWindow);
      }

      break;
    }

    return(0);
  }
}

extern "C" {
  __declspec(dllexport) LRESULT CALLBACK CreateDisplayConfigDialogCallback(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
  {
    static bool isRGB;

    ConfigState* configState = GetConfigState();

    switch (message)
    {
    case WM_INITDIALOG:
      SendDlgItemMessage(hDlg, IDC_FRAMESKIP, TBM_SETRANGE, TRUE, MAKELONG(1, 6));
      SendDlgItemMessage(hDlg, IDC_FRAMESKIP, TBM_SETPOS, TRUE, configModel->FrameSkip);

      sprintf(configState->OutBuffer, "%i", configModel->FrameSkip);

      SendDlgItemMessage(hDlg, IDC_FRAMEDISPLAY, WM_SETTEXT, strlen(configState->OutBuffer), (LPARAM)(LPCSTR)(configState->OutBuffer));

      for (unsigned char temp = 0; temp <= 1; temp++) {
        SendDlgItemMessage(hDlg, Monchoice[temp], BM_SETCHECK, (temp == configModel->MonitorType), 0);
      }

      if (configModel->MonitorType == 1) { //If RGB monitor is chosen, gray out palette choice
        isRGB = TRUE;

        SendDlgItemMessage(hDlg, IDC_ORG_PALETTE, BM_SETSTATE, 1, 0);
        SendDlgItemMessage(hDlg, IDC_UPD_PALETTE, BM_SETSTATE, 1, 0);
        SendDlgItemMessage(hDlg, IDC_ORG_PALETTE, BM_SETDONTCLICK, 1, 0);
        SendDlgItemMessage(hDlg, IDC_UPD_PALETTE, BM_SETDONTCLICK, 1, 0);
      }

      SendDlgItemMessage(hDlg, IDC_MONTYPE, STM_SETIMAGE, (WPARAM)IMAGE_ICON, (LPARAM)(MonIcons[configModel->MonitorType]));

      for (unsigned char temp = 0; temp <= 1; temp++) {
        SendDlgItemMessage(hDlg, PaletteChoice[temp], BM_SETCHECK, (temp == configModel->PaletteType), 0);
      }

      break;

    case WM_HSCROLL:
      configModel->FrameSkip = (unsigned char)SendDlgItemMessage(hDlg, IDC_FRAMESKIP, TBM_GETPOS, (WPARAM)0, (WPARAM)0);

      sprintf(configState->OutBuffer, "%i", configModel->FrameSkip);

      SendDlgItemMessage(hDlg, IDC_FRAMEDISPLAY, WM_SETTEXT, strlen(configState->OutBuffer), (LPARAM)(LPCSTR)(configState->OutBuffer));

      break;

    case WM_COMMAND:
      //POINT p = { 640,480 };
      switch (LOWORD(wParam))
      {
      case IDC_COMPOSITE:
        isRGB = FALSE;
        for (unsigned char temp = 0; temp <= 1; temp++) { //This finds the current Monitor choice, then sets both buttons in the nested loop.
          if (LOWORD(wParam) == Monchoice[temp])
          {
            for (unsigned char temp2 = 0; temp2 <= 1; temp2++) {
              SendDlgItemMessage(hDlg, Monchoice[temp2], BM_SETCHECK, 0, 0);
            }

            SendDlgItemMessage(hDlg, Monchoice[temp], BM_SETCHECK, 1, 0);

            configModel->MonitorType = temp;
          }
        }

        SendDlgItemMessage(hDlg, IDC_MONTYPE, STM_SETIMAGE, (WPARAM)IMAGE_ICON, (LPARAM)(MonIcons[configModel->MonitorType]));
        SendDlgItemMessage(hDlg, IDC_ORG_PALETTE, BM_SETSTATE, 0, 0);
        SendDlgItemMessage(hDlg, IDC_UPD_PALETTE, BM_SETSTATE, 0, 0);

        break;

      case IDC_RGB:
        isRGB = TRUE;

        for (unsigned char temp = 0; temp <= 1; temp++) { //This finds the current Monitor choice, then sets both buttons in the nested loop.
          if (LOWORD(wParam) == Monchoice[temp])
          {
            for (unsigned char temp2 = 0; temp2 <= 1; temp2++) {
              SendDlgItemMessage(hDlg, Monchoice[temp2], BM_SETCHECK, 0, 0);
            }

            SendDlgItemMessage(hDlg, Monchoice[temp], BM_SETCHECK, 1, 0);

            configModel->MonitorType = temp;
          }
        }

        SendDlgItemMessage(hDlg, IDC_MONTYPE, STM_SETIMAGE, (WPARAM)IMAGE_ICON, (LPARAM)(MonIcons[configModel->MonitorType]));
        //If RGB is chosen, disable palette buttons.
        SendDlgItemMessage(hDlg, IDC_ORG_PALETTE, BM_SETSTATE, 1, 0);
        SendDlgItemMessage(hDlg, IDC_UPD_PALETTE, BM_SETSTATE, 1, 0);
        SendDlgItemMessage(hDlg, IDC_ORG_PALETTE, BM_SETDONTCLICK, 1, 0);
        SendDlgItemMessage(hDlg, IDC_UPD_PALETTE, BM_SETDONTCLICK, 1, 0);

        break;

      case IDC_ORG_PALETTE:
        if (!isRGB) {
          //Original Composite palette
          SendDlgItemMessage(hDlg, IDC_ORG_PALETTE, BM_SETCHECK, 1, 0);
          SendDlgItemMessage(hDlg, IDC_UPD_PALETTE, BM_SETCHECK, 0, 0);
          configModel->PaletteType = 0;
        }
        else {
          SendDlgItemMessage(hDlg, IDC_ORG_PALETTE, BM_SETSTATE, 1, 0);
        }
        break;

      case IDC_UPD_PALETTE:
        if (!isRGB) {
          //New Composite palette
          SendDlgItemMessage(hDlg, IDC_UPD_PALETTE, BM_SETCHECK, 1, 0);
          SendDlgItemMessage(hDlg, IDC_ORG_PALETTE, BM_SETCHECK, 0, 0);
          configModel->PaletteType = 1;
        }
        else {
          SendDlgItemMessage(hDlg, IDC_UPD_PALETTE, BM_SETSTATE, 1, 0);
        }
        break;
      }
      break;
    }

    return(0);
  }
}

extern "C" {
  __declspec(dllexport) LRESULT CALLBACK CreateJoyStickConfigDialogCallback(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
  {
    const int LeftJoyStick[6] = { IDC_LEFT_LEFT, IDC_LEFT_RIGHT, IDC_LEFT_UP, IDC_LEFT_DOWN, IDC_LEFT_FIRE1, IDC_LEFT_FIRE2 };
    const int RightJoyStick[6] = { IDC_RIGHT_LEFT, IDC_RIGHT_RIGHT, IDC_RIGHT_UP, IDC_RIGHT_DOWN, IDC_RIGHT_FIRE1, IDC_RIGHT_FIRE2 };
    const int LeftRadios[4] = { IDC_LEFT_KEYBOARD, IDC_LEFT_USEMOUSE, IDC_LEFTAUDIO, IDC_LEFTJOYSTICK };
    const int RightRadios[4] = { IDC_RIGHT_KEYBOARD, IDC_RIGHT_USEMOUSE, IDC_RIGHTAUDIO, IDC_RIGHTJOYSTICK };

    VccState* vccState = GetVccState();
    EmuState* emuState = GetEmuState();
    ConfigState* configState = GetConfigState();

    JoystickModel* left = configState->Model->Left;
    JoystickModel* right = configState->Model->Right;

    unsigned char numberOfJoysticks = configState->NumberOfJoysticks;

    switch (message)
    {
    case WM_INITDIALOG:
      JoystickIcons[0] = LoadIcon(emuState->Resources, (LPCTSTR)IDI_KEYBOARD);
      JoystickIcons[1] = LoadIcon(emuState->Resources, (LPCTSTR)IDI_MOUSE);
      JoystickIcons[2] = LoadIcon(emuState->Resources, (LPCTSTR)IDI_AUDIO);
      JoystickIcons[3] = LoadIcon(emuState->Resources, (LPCTSTR)IDI_JOYSTICK);

      for (unsigned char temp = 0; temp < 68; temp++)
      {
        for (unsigned char temp2 = 0; temp2 < 6; temp2++)
        {
          SendDlgItemMessage(hDlg, LeftJoyStick[temp2], CB_ADDSTRING, (WPARAM)0, (LPARAM)GetKeyName(temp));
          SendDlgItemMessage(hDlg, RightJoyStick[temp2], CB_ADDSTRING, (WPARAM)0, (LPARAM)GetKeyName(temp));
        }
      }

      for (unsigned char temp = 0; temp < 6; temp++)
      {
        EnableWindow(GetDlgItem(hDlg, LeftJoyStick[temp]), (left->UseMouse == 0));
      }

      for (unsigned char temp = 0; temp < 6; temp++)
      {
        EnableWindow(GetDlgItem(hDlg, RightJoyStick[temp]), (right->UseMouse == 0));
      }

      EnableWindow(GetDlgItem(hDlg, IDC_LEFTAUDIODEVICE), (left->UseMouse == 2));
      EnableWindow(GetDlgItem(hDlg, IDC_RIGHTAUDIODEVICE), (right->UseMouse == 2));
      EnableWindow(GetDlgItem(hDlg, IDC_LEFTJOYSTICKDEVICE), (left->UseMouse == 3));
      EnableWindow(GetDlgItem(hDlg, IDC_RIGHTJOYSTICKDEVICE), (right->UseMouse == 3));

      EnableWindow(GetDlgItem(hDlg, IDC_LEFTJOYSTICK), (numberOfJoysticks > 0));		//Grey the Joystick Radios if
      EnableWindow(GetDlgItem(hDlg, IDC_RIGHTJOYSTICK), (numberOfJoysticks > 0));	  //No Joysticks are present

      //populate joystick combo boxs
      for (unsigned char index = 0; index < numberOfJoysticks; index++)
      {
        SendDlgItemMessage(hDlg, IDC_RIGHTJOYSTICKDEVICE, CB_ADDSTRING, (WPARAM)0, (LPARAM)GetStickName(index));
        SendDlgItemMessage(hDlg, IDC_LEFTJOYSTICKDEVICE, CB_ADDSTRING, (WPARAM)0, (LPARAM)GetStickName(index));
      }

      SendDlgItemMessage(hDlg, IDC_RIGHTJOYSTICKDEVICE, CB_SETCURSEL, (WPARAM)right->DiDevice, (LPARAM)0);
      SendDlgItemMessage(hDlg, IDC_LEFTJOYSTICKDEVICE, CB_SETCURSEL, (WPARAM)left->DiDevice, (LPARAM)0);

      SendDlgItemMessage(hDlg, LeftJoyStick[0], CB_SETCURSEL, (WPARAM)TranslateScan2Display(left->Left), (LPARAM)0);
      SendDlgItemMessage(hDlg, LeftJoyStick[1], CB_SETCURSEL, (WPARAM)TranslateScan2Display(left->Right), (LPARAM)0);
      SendDlgItemMessage(hDlg, LeftJoyStick[2], CB_SETCURSEL, (WPARAM)TranslateScan2Display(left->Up), (LPARAM)0);
      SendDlgItemMessage(hDlg, LeftJoyStick[3], CB_SETCURSEL, (WPARAM)TranslateScan2Display(left->Down), (LPARAM)0);
      SendDlgItemMessage(hDlg, LeftJoyStick[4], CB_SETCURSEL, (WPARAM)TranslateScan2Display(left->Fire1), (LPARAM)0);
      SendDlgItemMessage(hDlg, LeftJoyStick[5], CB_SETCURSEL, (WPARAM)TranslateScan2Display(left->Fire2), (LPARAM)0);

      SendDlgItemMessage(hDlg, RightJoyStick[0], CB_SETCURSEL, (WPARAM)TranslateScan2Display(right->Left), (LPARAM)0);
      SendDlgItemMessage(hDlg, RightJoyStick[1], CB_SETCURSEL, (WPARAM)TranslateScan2Display(right->Right), (LPARAM)0);
      SendDlgItemMessage(hDlg, RightJoyStick[2], CB_SETCURSEL, (WPARAM)TranslateScan2Display(right->Up), (LPARAM)0);
      SendDlgItemMessage(hDlg, RightJoyStick[3], CB_SETCURSEL, (WPARAM)TranslateScan2Display(right->Down), (LPARAM)0);
      SendDlgItemMessage(hDlg, RightJoyStick[4], CB_SETCURSEL, (WPARAM)TranslateScan2Display(right->Fire1), (LPARAM)0);
      SendDlgItemMessage(hDlg, RightJoyStick[5], CB_SETCURSEL, (WPARAM)TranslateScan2Display(right->Fire2), (LPARAM)0);

      for (unsigned char temp = 0; temp <= 3; temp++)
      {
        SendDlgItemMessage(hDlg, LeftRadios[temp], BM_SETCHECK, temp == left->UseMouse, 0);
      }

      for (unsigned char temp = 0; temp <= 3; temp++)
      {
        SendDlgItemMessage(hDlg, RightRadios[temp], BM_SETCHECK, temp == right->UseMouse, 0);
      }

      SendDlgItemMessage(hDlg, IDC_LEFTICON, STM_SETIMAGE, (WPARAM)IMAGE_ICON, (LPARAM)(JoystickIcons[left->UseMouse]));
      SendDlgItemMessage(hDlg, IDC_RIGHTICON, STM_SETIMAGE, (WPARAM)IMAGE_ICON, (LPARAM)(JoystickIcons[right->UseMouse]));

      break;

    case WM_COMMAND:
      for (unsigned char temp = 0; temp <= 3; temp++)
      {
        if (LOWORD(wParam) == LeftRadios[temp])
        {
          for (unsigned char temp2 = 0; temp2 <= 3; temp2++) {
            SendDlgItemMessage(hDlg, LeftRadios[temp2], BM_SETCHECK, 0, 0);
          }

          SendDlgItemMessage(hDlg, LeftRadios[temp], BM_SETCHECK, 1, 0);

          left->UseMouse = temp;
        }
      }

      for (unsigned char temp = 0; temp <= 3; temp++) {
        if (LOWORD(wParam) == RightRadios[temp])
        {
          for (unsigned char temp2 = 0; temp2 <= 3; temp2++) {
            SendDlgItemMessage(hDlg, RightRadios[temp2], BM_SETCHECK, 0, 0);
          }

          SendDlgItemMessage(hDlg, RightRadios[temp], BM_SETCHECK, 1, 0);

          right->UseMouse = temp;
        }
      }

      for (unsigned char temp = 0; temp < 6; temp++)
      {
        EnableWindow(GetDlgItem(hDlg, LeftJoyStick[temp]), (left->UseMouse == 0));
      }

      for (unsigned char temp = 0; temp < 6; temp++)
      {
        EnableWindow(GetDlgItem(hDlg, RightJoyStick[temp]), (right->UseMouse == 0));
      }

      EnableWindow(GetDlgItem(hDlg, IDC_LEFTAUDIODEVICE), (left->UseMouse == 2));
      EnableWindow(GetDlgItem(hDlg, IDC_RIGHTAUDIODEVICE), (right->UseMouse == 2));
      EnableWindow(GetDlgItem(hDlg, IDC_LEFTJOYSTICKDEVICE), (left->UseMouse == 3));
      EnableWindow(GetDlgItem(hDlg, IDC_RIGHTJOYSTICKDEVICE), (right->UseMouse == 3));

      left->Left = TranslateDisplay2Scan(SendDlgItemMessage(hDlg, LeftJoyStick[0], CB_GETCURSEL, 0, 0));
      left->Right = TranslateDisplay2Scan(SendDlgItemMessage(hDlg, LeftJoyStick[1], CB_GETCURSEL, 0, 0));
      left->Up = TranslateDisplay2Scan(SendDlgItemMessage(hDlg, LeftJoyStick[2], CB_GETCURSEL, 0, 0));
      left->Down = TranslateDisplay2Scan(SendDlgItemMessage(hDlg, LeftJoyStick[3], CB_GETCURSEL, 0, 0));
      left->Fire1 = TranslateDisplay2Scan(SendDlgItemMessage(hDlg, LeftJoyStick[4], CB_GETCURSEL, 0, 0));
      left->Fire2 = TranslateDisplay2Scan(SendDlgItemMessage(hDlg, LeftJoyStick[5], CB_GETCURSEL, 0, 0));

      //char t[10];
      //_itoa((int)left->Fire2, t, 10);
      //OutputDebugString(t);

      right->Left = TranslateDisplay2Scan(SendDlgItemMessage(hDlg, RightJoyStick[0], CB_GETCURSEL, 0, 0));
      right->Right = TranslateDisplay2Scan(SendDlgItemMessage(hDlg, RightJoyStick[1], CB_GETCURSEL, 0, 0));
      right->Up = TranslateDisplay2Scan(SendDlgItemMessage(hDlg, RightJoyStick[2], CB_GETCURSEL, 0, 0));
      right->Down = TranslateDisplay2Scan(SendDlgItemMessage(hDlg, RightJoyStick[3], CB_GETCURSEL, 0, 0));
      right->Fire1 = TranslateDisplay2Scan(SendDlgItemMessage(hDlg, RightJoyStick[4], CB_GETCURSEL, 0, 0));
      right->Fire2 = TranslateDisplay2Scan(SendDlgItemMessage(hDlg, RightJoyStick[5], CB_GETCURSEL, 0, 0));

      right->DiDevice = (unsigned char)SendDlgItemMessage(hDlg, IDC_RIGHTJOYSTICKDEVICE, CB_GETCURSEL, 0, 0);
      left->DiDevice = (unsigned char)SendDlgItemMessage(hDlg, IDC_LEFTJOYSTICKDEVICE, CB_GETCURSEL, 0, 0);	//Fix Me;

      SendDlgItemMessage(hDlg, IDC_LEFTICON, STM_SETIMAGE, (WPARAM)IMAGE_ICON, (LPARAM)(JoystickIcons[left->UseMouse]));
      SendDlgItemMessage(hDlg, IDC_RIGHTICON, STM_SETIMAGE, (WPARAM)IMAGE_ICON, (LPARAM)(JoystickIcons[right->UseMouse]));

      break;
    }

    return(0);
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

        SetTapeMode(configState->TapeMode);

        break;

      case IDC_REC:
        configState->TapeMode = REC;

        SetTapeMode(configState->TapeMode);

        break;

      case IDC_STOP:
        configState->TapeMode = STOP;

        SetTapeMode(configState->TapeMode);

        break;

      case IDC_EJECT:
        configState->TapeMode = EJECT;

        SetTapeMode(configState->TapeMode);

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
