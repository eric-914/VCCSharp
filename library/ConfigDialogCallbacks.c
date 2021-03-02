#include <windows.h>
#include <string>
#include <ShlObj.h>

#include "../resources/resource.h"

#include "Config.h"
#include "Audio.h"
#include "VCC.h"
#include "MC6821.h"
#include "Keyboard.h"
#include "Cassette.h"
#include "Joystick.h"

const unsigned short int Cpuchoice[2] = { IDC_6809, IDC_6309 };
const unsigned short int Monchoice[2] = { IDC_COMPOSITE, IDC_RGB };
const unsigned short int PaletteChoice[2] = { IDC_ORG_PALETTE, IDC_UPD_PALETTE };
const unsigned short int Ramchoice[4] = { IDC_128K, IDC_512K, IDC_2M, IDC_8M };
const unsigned int LeftJoystickEmulation[3] = { IDC_LEFTSTANDARD, IDC_LEFTTHIRES, IDC_LEFTCCMAX };
const unsigned int RightJoystickEmulation[3] = { IDC_RIGHTSTANDARD, IDC_RIGHTTHRES, IDC_RIGHTCCMAX };

const char TapeModes[4][10] = { "STOP", "PLAY", "REC", "STOP" };

static HICON CpuIcons[2];
static HICON MonIcons[2];
static HICON JoystickIcons[4];

ConfigModel configModel;

/*
  for displaying key name
*/
char* const keyNames[] = { "","ESC","1","2","3","4","5","6","7","8","9","0","-","=","BackSp","Tab","A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z","[","]","Bkslash",";","'","Comma",".","/","CapsLk","Shift","Ctrl","Alt","Space","Enter","Insert","Delete","Home","End","PgUp","PgDown","Left","Right","Up","Down","F1","F2" };

char* GetKeyName(int x)
{
  return keyNames[x];
}

void CheckAudioChange(SystemState systemState, ConfigModel current, ConfigModel temp, SoundCardList* soundCards) {
  unsigned char currentSoundCardIndex = GetSoundCardIndex(current.SoundCardName);
  unsigned char tempSoundCardIndex = GetSoundCardIndex(temp.SoundCardName);

  if ((currentSoundCardIndex != tempSoundCardIndex) || (current.AudioRate != temp.AudioRate)) {
    SoundInit(systemState.WindowHandle, soundCards[tempSoundCardIndex].Guid, temp.AudioRate);
  }
}

extern "C" {
  __declspec(dllexport) void SetDialogTapeCount(HWND hDlg, unsigned char tapeMode) {
    SendDlgItemMessage(hDlg, IDC_MODE, WM_SETTEXT, strlen(TapeModes[tapeMode]), (LPARAM)(LPCSTR)(TapeModes[tapeMode]));
  }
}

extern "C" {
  __declspec(dllexport) LRESULT CALLBACK CreateAudioConfigDialogCallback(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
  {
    unsigned char soundCardIndex;

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

      for (unsigned char index = 0; index < configState->NumberOfSoundCards; index++) {
        SendDlgItemMessage(hDlg, IDC_SOUNDCARD, CB_ADDSTRING, (WPARAM)0, (LPARAM)(configState->SoundCards[index].CardName));
      }

      for (unsigned char index = 0; index < 4; index++) {
        SendDlgItemMessage(hDlg, IDC_RATE, CB_ADDSTRING, (WPARAM)0, (LPARAM)GetRateList(index));
      }

      SendDlgItemMessage(hDlg, IDC_RATE, CB_SETCURSEL, (WPARAM)(configModel.AudioRate), (LPARAM)0);

      soundCardIndex = GetSoundCardIndex(configModel.SoundCardName);
      SendDlgItemMessage(hDlg, IDC_SOUNDCARD, CB_SETCURSEL, (WPARAM)(soundCardIndex), (LPARAM)0);

      break;

    case WM_COMMAND:
      soundCardIndex = (unsigned char)SendDlgItemMessage(hDlg, IDC_SOUNDCARD, CB_GETCURSEL, 0, 0);
      configModel.AudioRate = (unsigned char)SendDlgItemMessage(hDlg, IDC_RATE, CB_GETCURSEL, 0, 0);

      strcpy(configModel.SoundCardName, configState->SoundCards[soundCardIndex].CardName);

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
      SendDlgItemMessage(hDlg, IDC_PRINTMON, BM_SETCHECK, configState->PrtMon, 0);

      break;

    case WM_COMMAND:

      switch (LOWORD(wParam))
      {
      case IDC_OPEN:
        SelectSerialCaptureFile(&(vccState->SystemState), configState->SerialCaptureFile);

        SendDlgItemMessage(hDlg, IDC_SERIALFILE, WM_SETTEXT, strlen(configState->SerialCaptureFile), (LPARAM)(LPCSTR)(configState->SerialCaptureFile));

        break;

      case IDC_CLOSE:
        MC6821_ClosePrintFile();
        strcpy(configState->SerialCaptureFile, "No Capture File");

        SendDlgItemMessage(hDlg, IDC_SERIALFILE, WM_SETTEXT, strlen(configState->SerialCaptureFile), (LPARAM)(LPCSTR)(configState->SerialCaptureFile));

        configState->PrtMon = FALSE;

        MC6821_SetMonState(configState->PrtMon);

        SendDlgItemMessage(hDlg, IDC_PRINTMON, BM_SETCHECK, configState->PrtMon, 0);

        break;

      case IDC_LF:
        configState->TextMode = (char)SendDlgItemMessage(hDlg, IDC_LF, BM_GETCHECK, 0, 0);

        MC6821_SetSerialParams(configState->TextMode);

        break;

      case IDC_PRINTMON:
        configState->PrtMon = (char)SendDlgItemMessage(hDlg, IDC_PRINTMON, BM_GETCHECK, 0, 0);

        MC6821_SetMonState(configState->PrtMon);
      }

      break;
    }

    return(0);
  }
}

extern "C" {
  __declspec(dllexport) LRESULT CALLBACK CreateCpuConfigDialogCallback(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
  {
    ConfigState* configState = GetConfigState();

    switch (message)
    {
    case WM_INITDIALOG:
      SendDlgItemMessage(hDlg, IDC_CLOCKSPEED, TBM_SETRANGE, TRUE, MAKELONG(2, configState->Model.MaxOverclock));	//Maximum overclock

      sprintf(configState->OutBuffer, "%2.3f Mhz", (float)(configModel.CPUMultiplier) * .894);

      SendDlgItemMessage(hDlg, IDC_CLOCKDISPLAY, WM_SETTEXT, strlen(configState->OutBuffer), (LPARAM)(LPCSTR)(configState->OutBuffer));
      SendDlgItemMessage(hDlg, IDC_CLOCKSPEED, TBM_SETPOS, TRUE, configModel.CPUMultiplier);

      for (unsigned char temp = 0; temp <= 3; temp++) {
        SendDlgItemMessage(hDlg, Ramchoice[temp], BM_SETCHECK, (temp == configModel.RamSize), 0);
      }

      for (unsigned char temp = 0; temp <= 1; temp++) {
        SendDlgItemMessage(hDlg, Cpuchoice[temp], BM_SETCHECK, (temp == configModel.CpuType), 0);
      }

      SendDlgItemMessage(hDlg, IDC_CPUICON, STM_SETIMAGE, (WPARAM)IMAGE_ICON, (LPARAM)(CpuIcons[configModel.CpuType]));

      break;

    case WM_HSCROLL:
      configModel.CPUMultiplier = (unsigned char)SendDlgItemMessage(hDlg, IDC_CLOCKSPEED, TBM_GETPOS, (WPARAM)0, (WPARAM)0);

      sprintf(configState->OutBuffer, "%2.3f Mhz", (float)(configModel.CPUMultiplier) * .894);

      SendDlgItemMessage(hDlg, IDC_CLOCKDISPLAY, WM_SETTEXT, strlen(configState->OutBuffer), (LPARAM)(LPCSTR)(configState->OutBuffer));

      break;

    case WM_COMMAND:
      switch (LOWORD(wParam))
      {
      case IDC_128K:
      case IDC_512K:
      case IDC_2M:
      case IDC_8M:
        for (unsigned char temp = 0; temp <= 3; temp++) {
          if (LOWORD(wParam) == Ramchoice[temp])
          {
            for (unsigned char temp2 = 0; temp2 <= 3; temp2++) {
              SendDlgItemMessage(hDlg, Ramchoice[temp2], BM_SETCHECK, 0, 0);
            }

            SendDlgItemMessage(hDlg, Ramchoice[temp], BM_SETCHECK, 1, 0);

            configModel.RamSize = temp;
          }
        }

        break;

      case IDC_6809:
      case IDC_6309:
        for (unsigned char temp = 0; temp <= 1; temp++) {
          if (LOWORD(wParam) == Cpuchoice[temp])
          {
            for (unsigned char temp2 = 0; temp2 <= 1; temp2++) {
              SendDlgItemMessage(hDlg, Cpuchoice[temp2], BM_SETCHECK, 0, 0);
            }

            SendDlgItemMessage(hDlg, Cpuchoice[temp], BM_SETCHECK, 1, 0);

            configModel.CpuType = temp;

            SendDlgItemMessage(hDlg, IDC_CPUICON, STM_SETIMAGE, (WPARAM)IMAGE_ICON, (LPARAM)(CpuIcons[configModel.CpuType]));
          }
        }

        break;
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
      SendDlgItemMessage(hDlg, IDC_SCANLINES, BM_SETCHECK, configModel.ScanLines, 0);
      SendDlgItemMessage(hDlg, IDC_THROTTLE, BM_SETCHECK, configModel.SpeedThrottle, 0);
      SendDlgItemMessage(hDlg, IDC_FRAMESKIP, TBM_SETPOS, TRUE, configModel.FrameSkip);
      SendDlgItemMessage(hDlg, IDC_RESIZE, BM_SETCHECK, configModel.AllowResize, 0);
      SendDlgItemMessage(hDlg, IDC_ASPECT, BM_SETCHECK, configModel.ForceAspect, 0);
      SendDlgItemMessage(hDlg, IDC_REMEMBER_SIZE, BM_SETCHECK, configModel.RememberSize, 0);

      sprintf(configState->OutBuffer, "%i", configModel.FrameSkip);

      SendDlgItemMessage(hDlg, IDC_FRAMEDISPLAY, WM_SETTEXT, strlen(configState->OutBuffer), (LPARAM)(LPCSTR)(configState->OutBuffer));

      for (unsigned char temp = 0; temp <= 1; temp++) {
        SendDlgItemMessage(hDlg, Monchoice[temp], BM_SETCHECK, (temp == configModel.MonitorType), 0);
      }

      if (configModel.MonitorType == 1) { //If RGB monitor is chosen, gray out palette choice
        isRGB = TRUE;

        SendDlgItemMessage(hDlg, IDC_ORG_PALETTE, BM_SETSTATE, 1, 0);
        SendDlgItemMessage(hDlg, IDC_UPD_PALETTE, BM_SETSTATE, 1, 0);
        SendDlgItemMessage(hDlg, IDC_ORG_PALETTE, BM_SETDONTCLICK, 1, 0);
        SendDlgItemMessage(hDlg, IDC_UPD_PALETTE, BM_SETDONTCLICK, 1, 0);
      }

      SendDlgItemMessage(hDlg, IDC_MONTYPE, STM_SETIMAGE, (WPARAM)IMAGE_ICON, (LPARAM)(MonIcons[configModel.MonitorType]));

      for (unsigned char temp = 0; temp <= 1; temp++) {
        SendDlgItemMessage(hDlg, PaletteChoice[temp], BM_SETCHECK, (temp == configModel.PaletteType), 0);
      }

      break;

    case WM_HSCROLL:
      configModel.FrameSkip = (unsigned char)SendDlgItemMessage(hDlg, IDC_FRAMESKIP, TBM_GETPOS, (WPARAM)0, (WPARAM)0);

      sprintf(configState->OutBuffer, "%i", configModel.FrameSkip);

      SendDlgItemMessage(hDlg, IDC_FRAMEDISPLAY, WM_SETTEXT, strlen(configState->OutBuffer), (LPARAM)(LPCSTR)(configState->OutBuffer));

      break;

    case WM_COMMAND:
      configModel.AllowResize = 1;
      configModel.ForceAspect = (unsigned char)SendDlgItemMessage(hDlg, IDC_ASPECT, BM_GETCHECK, 0, 0);
      configModel.ScanLines = (unsigned char)SendDlgItemMessage(hDlg, IDC_SCANLINES, BM_GETCHECK, 0, 0);
      configModel.SpeedThrottle = (unsigned char)SendDlgItemMessage(hDlg, IDC_THROTTLE, BM_GETCHECK, 0, 0);
      configModel.RememberSize = (unsigned char)SendDlgItemMessage(hDlg, IDC_REMEMBER_SIZE, BM_GETCHECK, 0, 0);

      //POINT p = { 640,480 };
      switch (LOWORD(wParam))
      {
      case IDC_REMEMBER_SIZE:
        configModel.AllowResize = 1;

        SendDlgItemMessage(hDlg, IDC_RESIZE, BM_GETCHECK, 1, 0);

        break;

      case IDC_COMPOSITE:
        isRGB = FALSE;
        for (unsigned char temp = 0; temp <= 1; temp++) { //This finds the current Monitor choice, then sets both buttons in the nested loop.
          if (LOWORD(wParam) == Monchoice[temp])
          {
            for (unsigned char temp2 = 0; temp2 <= 1; temp2++) {
              SendDlgItemMessage(hDlg, Monchoice[temp2], BM_SETCHECK, 0, 0);
            }

            SendDlgItemMessage(hDlg, Monchoice[temp], BM_SETCHECK, 1, 0);

            configModel.MonitorType = temp;
          }
        }

        SendDlgItemMessage(hDlg, IDC_MONTYPE, STM_SETIMAGE, (WPARAM)IMAGE_ICON, (LPARAM)(MonIcons[configModel.MonitorType]));
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

            configModel.MonitorType = temp;
          }
        }

        SendDlgItemMessage(hDlg, IDC_MONTYPE, STM_SETIMAGE, (WPARAM)IMAGE_ICON, (LPARAM)(MonIcons[configModel.MonitorType]));
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
          configModel.PaletteType = 0;
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
          configModel.PaletteType = 1;
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
  __declspec(dllexport) LRESULT CALLBACK CreateInputConfigDialogCallback(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
  {
    ConfigState* configState = GetConfigState();

    switch (message)
    {
    case WM_INITDIALOG:
      // copy keyboard layout names to the pull-down menu
      for (int x = 0; x < kKBLayoutCount; x++)
      {
        SendDlgItemMessage(hDlg, IDC_KBCONFIG, CB_ADDSTRING, (WPARAM)0, (LPARAM)k_keyboardLayoutNames[x]);
      }

      // select the current layout
      SendDlgItemMessage(hDlg, IDC_KBCONFIG, CB_SETCURSEL, (WPARAM)(configState->Model.KeyMapIndex), (LPARAM)0);
      break;

    case WM_COMMAND:
      configModel.KeyMapIndex = (unsigned char)SendDlgItemMessage(hDlg, IDC_KBCONFIG, CB_GETCURSEL, 0, 0);
      break;
    }

    return(0);
  }
}

extern "C" {
  __declspec(dllexport) LRESULT CALLBACK CreateJoyStickConfigDialogCallback(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
  {
    static int LeftJoyStick[6] = { IDC_LEFT_LEFT, IDC_LEFT_RIGHT, IDC_LEFT_UP, IDC_LEFT_DOWN, IDC_LEFT_FIRE1, IDC_LEFT_FIRE2 };
    static int RightJoyStick[6] = { IDC_RIGHT_LEFT, IDC_RIGHT_RIGHT, IDC_RIGHT_UP, IDC_RIGHT_DOWN, IDC_RIGHT_FIRE1, IDC_RIGHT_FIRE2 };
    static int LeftRadios[4] = { IDC_LEFT_KEYBOARD, IDC_LEFT_USEMOUSE, IDC_LEFTAUDIO, IDC_LEFTJOYSTICK };
    static int RightRadios[4] = { IDC_RIGHT_KEYBOARD, IDC_RIGHT_USEMOUSE, IDC_RIGHTAUDIO, IDC_RIGHTJOYSTICK };

    ConfigState* configState = GetConfigState();
    JoystickState* joystickState = GetJoystickState();
    VccState* vccState = GetVccState();

    switch (message)
    {
    case WM_INITDIALOG:
      JoystickIcons[0] = LoadIcon(vccState->SystemState.Resources, (LPCTSTR)IDI_KEYBOARD);
      JoystickIcons[1] = LoadIcon(vccState->SystemState.Resources, (LPCTSTR)IDI_MOUSE);
      JoystickIcons[2] = LoadIcon(vccState->SystemState.Resources, (LPCTSTR)IDI_AUDIO);
      JoystickIcons[3] = LoadIcon(vccState->SystemState.Resources, (LPCTSTR)IDI_JOYSTICK);

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
        EnableWindow(GetDlgItem(hDlg, LeftJoyStick[temp]), (joystickState->Left.UseMouse == 0));
      }

      for (unsigned char temp = 0; temp < 6; temp++)
      {
        EnableWindow(GetDlgItem(hDlg, RightJoyStick[temp]), (joystickState->Right.UseMouse == 0));
      }

      for (unsigned char temp = 0; temp <= 2; temp++)
      {
        SendDlgItemMessage(hDlg, LeftJoystickEmulation[temp], BM_SETCHECK, (temp == joystickState->Left.HiRes), 0);
        SendDlgItemMessage(hDlg, RightJoystickEmulation[temp], BM_SETCHECK, (temp == joystickState->Right.HiRes), 0);
      }

      EnableWindow(GetDlgItem(hDlg, IDC_LEFTAUDIODEVICE), (joystickState->Left.UseMouse == 2));
      EnableWindow(GetDlgItem(hDlg, IDC_RIGHTAUDIODEVICE), (joystickState->Right.UseMouse == 2));
      EnableWindow(GetDlgItem(hDlg, IDC_LEFTJOYSTICKDEVICE), (joystickState->Left.UseMouse == 3));
      EnableWindow(GetDlgItem(hDlg, IDC_RIGHTJOYSTICKDEVICE), (joystickState->Right.UseMouse == 3));

      EnableWindow(GetDlgItem(hDlg, IDC_LEFTJOYSTICK), (configState->NumberOfJoysticks > 0));		//Grey the Joystick Radios if
      EnableWindow(GetDlgItem(hDlg, IDC_RIGHTJOYSTICK), (configState->NumberOfJoysticks > 0));	  //No Joysticks are present

      //populate joystick combo boxs
      for (unsigned char index = 0; index < configState->NumberOfJoysticks; index++)
      {
        SendDlgItemMessage(hDlg, IDC_RIGHTJOYSTICKDEVICE, CB_ADDSTRING, (WPARAM)0, (LPARAM)GetStickName(index));
        SendDlgItemMessage(hDlg, IDC_LEFTJOYSTICKDEVICE, CB_ADDSTRING, (WPARAM)0, (LPARAM)GetStickName(index));
      }

      SendDlgItemMessage(hDlg, IDC_RIGHTJOYSTICKDEVICE, CB_SETCURSEL, (WPARAM)joystickState->Right.DiDevice, (LPARAM)0);
      SendDlgItemMessage(hDlg, IDC_LEFTJOYSTICKDEVICE, CB_SETCURSEL, (WPARAM)joystickState->Left.DiDevice, (LPARAM)0);

      SendDlgItemMessage(hDlg, LeftJoyStick[0], CB_SETCURSEL, (WPARAM)TranslateScan2Display(joystickState->Left.Left), (LPARAM)0);
      SendDlgItemMessage(hDlg, LeftJoyStick[1], CB_SETCURSEL, (WPARAM)TranslateScan2Display(joystickState->Left.Right), (LPARAM)0);
      SendDlgItemMessage(hDlg, LeftJoyStick[2], CB_SETCURSEL, (WPARAM)TranslateScan2Display(joystickState->Left.Up), (LPARAM)0);
      SendDlgItemMessage(hDlg, LeftJoyStick[3], CB_SETCURSEL, (WPARAM)TranslateScan2Display(joystickState->Left.Down), (LPARAM)0);
      SendDlgItemMessage(hDlg, LeftJoyStick[4], CB_SETCURSEL, (WPARAM)TranslateScan2Display(joystickState->Left.Fire1), (LPARAM)0);
      SendDlgItemMessage(hDlg, LeftJoyStick[5], CB_SETCURSEL, (WPARAM)TranslateScan2Display(joystickState->Left.Fire2), (LPARAM)0);

      SendDlgItemMessage(hDlg, RightJoyStick[0], CB_SETCURSEL, (WPARAM)TranslateScan2Display(joystickState->Right.Left), (LPARAM)0);
      SendDlgItemMessage(hDlg, RightJoyStick[1], CB_SETCURSEL, (WPARAM)TranslateScan2Display(joystickState->Right.Right), (LPARAM)0);
      SendDlgItemMessage(hDlg, RightJoyStick[2], CB_SETCURSEL, (WPARAM)TranslateScan2Display(joystickState->Right.Up), (LPARAM)0);
      SendDlgItemMessage(hDlg, RightJoyStick[3], CB_SETCURSEL, (WPARAM)TranslateScan2Display(joystickState->Right.Down), (LPARAM)0);
      SendDlgItemMessage(hDlg, RightJoyStick[4], CB_SETCURSEL, (WPARAM)TranslateScan2Display(joystickState->Right.Fire1), (LPARAM)0);
      SendDlgItemMessage(hDlg, RightJoyStick[5], CB_SETCURSEL, (WPARAM)TranslateScan2Display(joystickState->Right.Fire2), (LPARAM)0);

      for (unsigned char temp = 0; temp <= 3; temp++)
      {
        SendDlgItemMessage(hDlg, LeftRadios[temp], BM_SETCHECK, temp == joystickState->Left.UseMouse, 0);
      }

      for (unsigned char temp = 0; temp <= 3; temp++)
      {
        SendDlgItemMessage(hDlg, RightRadios[temp], BM_SETCHECK, temp == joystickState->Right.UseMouse, 0);
      }

      configState->Model.Left = joystickState->Left;
      configState->Model.Right = joystickState->Right;

      SendDlgItemMessage(hDlg, IDC_LEFTICON, STM_SETIMAGE, (WPARAM)IMAGE_ICON, (LPARAM)(JoystickIcons[joystickState->Left.UseMouse]));
      SendDlgItemMessage(hDlg, IDC_RIGHTICON, STM_SETIMAGE, (WPARAM)IMAGE_ICON, (LPARAM)(JoystickIcons[joystickState->Right.UseMouse]));
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

          configState->Model.Left.UseMouse = temp;
        }
      }

      for (unsigned char temp = 0; temp <= 3; temp++) {
        if (LOWORD(wParam) == RightRadios[temp])
        {
          for (unsigned char temp2 = 0; temp2 <= 3; temp2++) {
            SendDlgItemMessage(hDlg, RightRadios[temp2], BM_SETCHECK, 0, 0);
          }

          SendDlgItemMessage(hDlg, RightRadios[temp], BM_SETCHECK, 1, 0);

          configState->Model.Right.UseMouse = temp;
        }
      }

      for (unsigned char temp = 0; temp <= 2; temp++) {
        if (LOWORD(wParam) == LeftJoystickEmulation[temp])
        {
          for (unsigned char temp2 = 0; temp2 <= 2; temp2++) {
            SendDlgItemMessage(hDlg, LeftJoystickEmulation[temp2], BM_SETCHECK, 0, 0);
          }

          SendDlgItemMessage(hDlg, LeftJoystickEmulation[temp], BM_SETCHECK, 1, 0);

          configState->Model.Left.HiRes = temp;
        }
      }

      for (unsigned char temp = 0; temp <= 2; temp++)
      {
        if (LOWORD(wParam) == RightJoystickEmulation[temp])
        {
          for (unsigned char temp2 = 0; temp2 <= 2; temp2++) {
            SendDlgItemMessage(hDlg, RightJoystickEmulation[temp2], BM_SETCHECK, 0, 0);
          }

          SendDlgItemMessage(hDlg, RightJoystickEmulation[temp], BM_SETCHECK, 1, 0);

          configState->Model.Right.HiRes = temp;
        }
      }

      for (unsigned char temp = 0; temp < 6; temp++)
      {
        EnableWindow(GetDlgItem(hDlg, LeftJoyStick[temp]), (configState->Model.Left.UseMouse == 0));
      }

      for (unsigned char temp = 0; temp < 6; temp++)
      {
        EnableWindow(GetDlgItem(hDlg, RightJoyStick[temp]), (configState->Model.Right.UseMouse == 0));
      }

      EnableWindow(GetDlgItem(hDlg, IDC_LEFTAUDIODEVICE), (configState->Model.Left.UseMouse == 2));
      EnableWindow(GetDlgItem(hDlg, IDC_RIGHTAUDIODEVICE), (configState->Model.Right.UseMouse == 2));
      EnableWindow(GetDlgItem(hDlg, IDC_LEFTJOYSTICKDEVICE), (configState->Model.Left.UseMouse == 3));
      EnableWindow(GetDlgItem(hDlg, IDC_RIGHTJOYSTICKDEVICE), (configState->Model.Right.UseMouse == 3));

      configState->Model.Left.Left = TranslateDisplay2Scan(SendDlgItemMessage(hDlg, LeftJoyStick[0], CB_GETCURSEL, 0, 0));
      configState->Model.Left.Right = TranslateDisplay2Scan(SendDlgItemMessage(hDlg, LeftJoyStick[1], CB_GETCURSEL, 0, 0));
      configState->Model.Left.Up = TranslateDisplay2Scan(SendDlgItemMessage(hDlg, LeftJoyStick[2], CB_GETCURSEL, 0, 0));
      configState->Model.Left.Down = TranslateDisplay2Scan(SendDlgItemMessage(hDlg, LeftJoyStick[3], CB_GETCURSEL, 0, 0));
      configState->Model.Left.Fire1 = TranslateDisplay2Scan(SendDlgItemMessage(hDlg, LeftJoyStick[4], CB_GETCURSEL, 0, 0));
      configState->Model.Left.Fire2 = TranslateDisplay2Scan(SendDlgItemMessage(hDlg, LeftJoyStick[5], CB_GETCURSEL, 0, 0));

      configState->Model.Right.Left = TranslateDisplay2Scan(SendDlgItemMessage(hDlg, RightJoyStick[0], CB_GETCURSEL, 0, 0));
      configState->Model.Right.Right = TranslateDisplay2Scan(SendDlgItemMessage(hDlg, RightJoyStick[1], CB_GETCURSEL, 0, 0));
      configState->Model.Right.Up = TranslateDisplay2Scan(SendDlgItemMessage(hDlg, RightJoyStick[2], CB_GETCURSEL, 0, 0));
      configState->Model.Right.Down = TranslateDisplay2Scan(SendDlgItemMessage(hDlg, RightJoyStick[3], CB_GETCURSEL, 0, 0));
      configState->Model.Right.Fire1 = TranslateDisplay2Scan(SendDlgItemMessage(hDlg, RightJoyStick[4], CB_GETCURSEL, 0, 0));
      configState->Model.Right.Fire2 = TranslateDisplay2Scan(SendDlgItemMessage(hDlg, RightJoyStick[5], CB_GETCURSEL, 0, 0));

      configState->Model.Right.DiDevice = (unsigned char)SendDlgItemMessage(hDlg, IDC_RIGHTJOYSTICKDEVICE, CB_GETCURSEL, 0, 0);
      configState->Model.Left.DiDevice = (unsigned char)SendDlgItemMessage(hDlg, IDC_LEFTJOYSTICKDEVICE, CB_GETCURSEL, 0, 0);	//Fix Me;

      SendDlgItemMessage(hDlg, IDC_LEFTICON, STM_SETIMAGE, (WPARAM)IMAGE_ICON, (LPARAM)(JoystickIcons[configState->Model.Left.UseMouse]));
      SendDlgItemMessage(hDlg, IDC_RIGHTICON, STM_SETIMAGE, (WPARAM)IMAGE_ICON, (LPARAM)(JoystickIcons[configState->Model.Right.UseMouse]));

      break;
    }

    return(0);
  }
}

extern "C" {
  __declspec(dllexport) LRESULT CALLBACK CreateMiscConfigDialogCallback(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
  {
    ConfigState* configState = GetConfigState();

    switch (message)
    {
    case WM_INITDIALOG:
      SendDlgItemMessage(hDlg, IDC_AUTOSTART, BM_SETCHECK, configModel.AutoStart, 0);
      SendDlgItemMessage(hDlg, IDC_AUTOCART, BM_SETCHECK, configModel.CartAutoStart, 0);

      break;

    case WM_COMMAND:
      configModel.AutoStart = (unsigned char)SendDlgItemMessage(hDlg, IDC_AUTOSTART, BM_GETCHECK, 0, 0);
      configModel.CartAutoStart = (unsigned char)SendDlgItemMessage(hDlg, IDC_AUTOCART, BM_GETCHECK, 0, 0);

      break;
    }

    return(0);
  }
}

extern "C" {
  __declspec(dllexport) LRESULT CALLBACK CreateTapeConfigDialogCallback(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
  {
    ConfigState* configState = GetConfigState();

    configState->CounterText.cbSize = sizeof(CHARFORMAT);
    configState->CounterText.dwMask = CFM_BOLD | CFM_COLOR;
    configState->CounterText.dwEffects = CFE_BOLD;
    configState->CounterText.crTextColor = RGB(255, 255, 255);

    configState->ModeText.cbSize = sizeof(CHARFORMAT);
    configState->ModeText.dwMask = CFM_BOLD | CFM_COLOR;
    configState->ModeText.dwEffects = CFE_BOLD;
    configState->ModeText.crTextColor = RGB(255, 0, 0);

    switch (message)
    {
    case WM_INITDIALOG:
      configState->TapeCounter = GetTapeCounter();

      sprintf(configState->OutBuffer, "%i", configState->TapeCounter);

      SendDlgItemMessage(hDlg, IDC_TCOUNT, WM_SETTEXT, strlen(configState->OutBuffer), (LPARAM)(LPCSTR)(configState->OutBuffer));
      SetDialogTapeCount(hDlg, configState->TapeMode);

      GetTapeName(configState->TapeFileName);

      SendDlgItemMessage(hDlg, IDC_TAPEFILE, WM_SETTEXT, strlen(configState->TapeFileName), (LPARAM)(LPCSTR)(configState->TapeFileName));
      SendDlgItemMessage(hDlg, IDC_TCOUNT, EM_SETBKGNDCOLOR, 0, (LPARAM)RGB(0, 0, 0));
      SendDlgItemMessage(hDlg, IDC_TCOUNT, EM_SETCHARFORMAT, SCF_ALL, (LPARAM) & (configState->CounterText));
      SendDlgItemMessage(hDlg, IDC_MODE, EM_SETBKGNDCOLOR, 0, (LPARAM)RGB(0, 0, 0));
      SendDlgItemMessage(hDlg, IDC_MODE, EM_SETCHARFORMAT, SCF_ALL, (LPARAM) & (configState->CounterText));

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
    static char tabTitles[TABS][10] = { "Audio", "CPU", "Display", "Keyboard", "Joysticks", "Misc", "Tape", "BitBanger" };
    static unsigned char tabCount = 0, selectedTab = 0;
    static HWND hWndTabDialog;
    TCITEM tabs = TCITEM();

    ConfigState* configState = GetConfigState();
    JoystickState* joystickState = GetJoystickState();
    VccState* vccState = GetVccState();

    switch (message)
    {
    case WM_INITDIALOG:
      InitCommonControls();

      configModel = configState->Model;
      CpuIcons[0] = LoadIcon(vccState->SystemState.Resources, (LPCTSTR)IDI_MOTO);
      CpuIcons[1] = LoadIcon(vccState->SystemState.Resources, (LPCTSTR)IDI_HITACHI2);
      MonIcons[0] = LoadIcon(vccState->SystemState.Resources, (LPCTSTR)IDI_COMPOSITE);
      MonIcons[1] = LoadIcon(vccState->SystemState.Resources, (LPCTSTR)IDI_RGB);

      hWndTabDialog = GetDlgItem(hDlg, IDC_CONFIGTAB); //get handle of Tabbed Dialog

      //get handles to all the sub panels in the control
      configState->hWndConfig[0] = CreateDialog(vccState->SystemState.Resources, MAKEINTRESOURCE(IDD_AUDIO), hWndTabDialog, (DLGPROC)CreateAudioConfigDialogCallback);
      configState->hWndConfig[1] = CreateDialog(vccState->SystemState.Resources, MAKEINTRESOURCE(IDD_CPU), hWndTabDialog, (DLGPROC)CreateCpuConfigDialogCallback);
      configState->hWndConfig[2] = CreateDialog(vccState->SystemState.Resources, MAKEINTRESOURCE(IDD_DISPLAY), hWndTabDialog, (DLGPROC)CreateDisplayConfigDialogCallback);
      configState->hWndConfig[3] = CreateDialog(vccState->SystemState.Resources, MAKEINTRESOURCE(IDD_INPUT), hWndTabDialog, (DLGPROC)CreateInputConfigDialogCallback);
      configState->hWndConfig[4] = CreateDialog(vccState->SystemState.Resources, MAKEINTRESOURCE(IDD_JOYSTICK), hWndTabDialog, (DLGPROC)CreateJoyStickConfigDialogCallback);
      configState->hWndConfig[5] = CreateDialog(vccState->SystemState.Resources, MAKEINTRESOURCE(IDD_MISC), hWndTabDialog, (DLGPROC)CreateMiscConfigDialogCallback);
      configState->hWndConfig[6] = CreateDialog(vccState->SystemState.Resources, MAKEINTRESOURCE(IDD_CASSETTE), hWndTabDialog, (DLGPROC)CreateTapeConfigDialogCallback);
      configState->hWndConfig[7] = CreateDialog(vccState->SystemState.Resources, MAKEINTRESOURCE(IDD_BITBANGER), hWndTabDialog, (DLGPROC)CreateBitBangerConfigDialogCallback);

      //Set the title text for all tabs
      for (tabCount = 0; tabCount < TABS; tabCount++)
      {
        tabs.mask = TCIF_TEXT | TCIF_IMAGE;
        tabs.iImage = -1;
        tabs.pszText = tabTitles[tabCount];

        TabCtrl_InsertItem(hWndTabDialog, tabCount, &tabs);
      }

      TabCtrl_SetCurSel(hWndTabDialog, 0);	//Set Initial Tab to 0

      for (tabCount = 0; tabCount < TABS; tabCount++) {	//Hide All the Sub Panels
        ShowWindow(configState->hWndConfig[tabCount], SW_HIDE);
      }

      SetWindowPos(configState->hWndConfig[0], HWND_TOP, 10, 30, 0, 0, SWP_NOSIZE | SWP_SHOWWINDOW);
      RefreshJoystickStatus();

      break;

    case WM_NOTIFY:
      if ((LOWORD(wParam)) == IDC_CONFIGTAB) {
        selectedTab = TabCtrl_GetCurSel(hWndTabDialog);

        for (tabCount = 0; tabCount < TABS; tabCount++) {
          ShowWindow(configState->hWndConfig[tabCount], SW_HIDE);
        }

        SetWindowPos(configState->hWndConfig[selectedTab], HWND_TOP, 10, 30, 0, 0, SWP_NOSIZE | SWP_SHOWWINDOW);
      }

      break;

    case WM_COMMAND:
      switch (LOWORD(wParam))
      {
      case IDOK:
        configState->hDlgBar = NULL;
        configState->hDlgTape = NULL;
        vccState->SystemState.ResetPending = 4;

        if ((configState->Model.RamSize != configModel.RamSize) || (configState->Model.CpuType != configModel.CpuType)) {
          vccState->SystemState.ResetPending = 2;
        }

        CheckAudioChange(vccState->SystemState, configState->Model, configModel, configState->SoundCards);

        configState->Model = configModel;

        vccKeyboardBuildRuntimeTable((keyboardlayout_e)(configState->Model.KeyMapIndex));

        joystickState->Right = configState->Model.Right;
        joystickState->Left = configState->Model.Left;

        SetStickNumbers(joystickState->Left.DiDevice, joystickState->Right.DiDevice);

        for (unsigned char temp = 0; temp < TABS; temp++)
        {
          DestroyWindow(configState->hWndConfig[temp]);
        }

#ifdef CONFIG_DIALOG_MODAL
        EndDialog(hDlg, LOWORD(wParam));
#else
        DestroyWindow(hDlg);
#endif
        vccState->SystemState.ConfigDialog = NULL;
        break;

      case IDAPPLY:
        vccState->SystemState.ResetPending = 4;

        if ((configState->Model.RamSize != configModel.RamSize) || (configState->Model.CpuType != configModel.CpuType)) {
          vccState->SystemState.ResetPending = 2;
        }

        CheckAudioChange(vccState->SystemState, configState->Model, configModel, configState->SoundCards);

        configState->Model = configModel;

        vccKeyboardBuildRuntimeTable((keyboardlayout_e)(configState->Model.KeyMapIndex));

        joystickState->Right = configState->Model.Right;
        joystickState->Left = configState->Model.Left;

        SetStickNumbers(joystickState->Left.DiDevice, joystickState->Right.DiDevice);

        break;

      case IDCANCEL:
        for (unsigned char temp = 0; temp < TABS; temp++)
        {
          DestroyWindow(configState->hWndConfig[temp]);
        }

#ifdef CONFIG_DIALOG_MODAL
        EndDialog(hDlg, LOWORD(wParam));
#else
        DestroyWindow(hDlg);
#endif

        vccState->SystemState.ConfigDialog = NULL;
        break;
        }

      break;
        }

    return FALSE;
      }
    }