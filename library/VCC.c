#include <stdio.h>
#include <process.h>

#include "VCC.h"

#include "Config.h"
#include "Coco.h"
#include "PAKInterface.h"
#include "Keyboard.h"
#include "Graphics.h"
#include "Audio.h"
#include "MC6821.h"
#include "MC6809.h"
#include "HD6309.h"
#include "TC1014Registers.h"
#include "TC1014MMU.h"
#include "QuickLoad.h"
#include "DirectDraw.h"
#include "Throttle.h"

#include "cpudef.h"
#include "fileoperations.h"
#include "ProcessMessage.h"
#include "Emu.h"

VccState* InitializeInstance(VccState*);

static VccState* instance = InitializeInstance(new VccState());
//static EmuState* _emu;

extern "C" {
  __declspec(dllexport) VccState* __cdecl GetVccState() {
    return instance;
  }
}

VccState* InitializeInstance(VccState* p) {
  p->DialogOpen = false;

  p->AutoStart = true;
  p->KB_save1 = 0;
  p->KB_save2 = 0;
  p->KeySaveToggle = 0;
  p->Qflag = 0;
  p->SC_save1 = 0;
  p->SC_save2 = 0;
  p->Throttle = 0;

  p->hEmuThread = NULL;
  p->FlagEmuStop = TH_RUNNING;

  strcpy(p->CpuName, "CPUNAME");
  strcpy(p->AppName, "");

  return p;
}

extern "C" {
  __declspec(dllexport) void __cdecl SaveConfig(void) {
    OPENFILENAME ofn;
    char curini[MAX_PATH];
    char newini[MAX_PATH + 4];  // Save room for '.ini' if needed

    static EmuState* _emu = GetEmuState();

    GetIniFilePath(curini);  // EJJ get current ini file path
    strcpy(newini, curini);   // Let GetOpenFilename suggest it

    memset(&ofn, 0, sizeof(ofn));

    ofn.lStructSize = sizeof(OPENFILENAME);
    ofn.hwndOwner = _emu->WindowHandle;
    ofn.lpstrFilter = "INI\0*.ini\0\0";      // filter string
    ofn.nFilterIndex = 1;                    // current filter index
    ofn.lpstrFile = newini;                  // contains full path on return
    ofn.nMaxFile = MAX_PATH;                 // sizeof lpstrFile
    ofn.lpstrFileTitle = NULL;               // filename and extension only
    ofn.nMaxFileTitle = MAX_PATH;            // sizeof lpstrFileTitle
    ofn.lpstrInitialDir = AppDirectory();    // EJJ initial directory
    ofn.lpstrTitle = TEXT("Save Vcc Config"); // title bar string
    ofn.Flags = OFN_HIDEREADONLY | OFN_PATHMUSTEXIST;

    if (GetOpenFileName(&ofn)) {
      if (ofn.nFileExtension == 0) {
        strcat(newini, ".ini");  //Add extension if none
      }

      WriteIniFile(_emu); // Flush current config

      if (_stricmp(curini, newini) != 0) {
        if (!CopyFile(curini, newini, false)) { // Copy it to new file
          MessageBox(0, "Copy config failed", "error", 0);
        }
      }
    }
  }
}

// Save last two key down events
extern "C" {
  __declspec(dllexport) void __cdecl SaveLastTwoKeyDownEvents(unsigned char kb_char, unsigned char oemScan) {
    // Ignore zero scan code
    if (oemScan == 0) {
      return;
    }

    // Remember it
    instance->KeySaveToggle = !instance->KeySaveToggle;

    if (instance->KeySaveToggle) {
      instance->KB_save1 = kb_char;
      instance->SC_save1 = oemScan;
    }
    else {
      instance->KB_save2 = kb_char;
      instance->SC_save2 = oemScan;
    }
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl SetAutoStart(unsigned char autostart)
  {
    if (autostart != QUERY) {
      instance->AutoStart = autostart;
    }

    return(instance->AutoStart);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetCPUMultiplierFlag(unsigned char double_speed)
  {
    static EmuState* _emu = GetEmuState();

    SetClockSpeed(1);

    _emu->DoubleSpeedFlag = double_speed;

    if (_emu->DoubleSpeedFlag) {
      SetClockSpeed(_emu->DoubleSpeedMultiplier * _emu->TurboSpeedFlag);
    }

    _emu->CPUCurrentSpeed = .894;

    if (_emu->DoubleSpeedFlag) {
      _emu->CPUCurrentSpeed *= ((double)_emu->DoubleSpeedMultiplier * (double)_emu->TurboSpeedFlag);
    }
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl SetCPUMultiplier(unsigned char multiplier)
  {
    static EmuState* _emu = GetEmuState();

    if (multiplier != QUERY)
    {
      _emu->DoubleSpeedMultiplier = multiplier;

      SetCPUMultiplierFlag(_emu->DoubleSpeedFlag);
    }

    return(_emu->DoubleSpeedMultiplier);
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl SetCpuType(unsigned char cpuType)
  {
    static EmuState* _emu = GetEmuState();

    switch (cpuType)
    {
    case 0:
      _emu->CpuType = 0;

      strcpy(instance->CpuName, "MC6809");

      break;

    case 1:
      _emu->CpuType = 1;

      strcpy(instance->CpuName, "HD6309");

      break;
    }

    return(_emu->CpuType);
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl SetFrameSkip(unsigned char skip)
  {
    static EmuState* _emu = GetEmuState();

    if (skip != QUERY) {
      _emu->FrameSkip = skip;
    }

    return(_emu->FrameSkip);
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl SetRamSize(unsigned char size)
  {
    static EmuState* _emu = GetEmuState();

    if (size != QUERY) {
      _emu->RamSize = size;
    }

    return(_emu->RamSize);
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl SetSpeedThrottle(unsigned char throttle)
  {
    if (throttle != QUERY) {
      instance->Throttle = throttle;
    }

    return(instance->Throttle);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetTurboMode(unsigned char data)
  {
    static EmuState* _emu = GetEmuState();

    _emu->TurboSpeedFlag = (data & 1) + 1;

    SetClockSpeed(1);

    if (_emu->DoubleSpeedFlag) {
      SetClockSpeed(_emu->DoubleSpeedMultiplier * _emu->TurboSpeedFlag);
    }

    _emu->CPUCurrentSpeed = .894;

    if (_emu->DoubleSpeedFlag) {
      _emu->CPUCurrentSpeed *= ((double)_emu->DoubleSpeedMultiplier * (double)_emu->TurboSpeedFlag);
    }
  }
}

extern "C" {
  __declspec(dllexport) unsigned __stdcall CartLoad(void* dummy)
  {
    static EmuState* _emu = GetEmuState();

    LoadCart(_emu);

    _emu->EmulationRunning = true;
    instance->DialogOpen = false;

    return(NULL);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl LoadPack(void)
  {
    unsigned threadID;

    if (instance->DialogOpen) {
      return;
    }

    instance->DialogOpen = true;

    _beginthreadex(NULL, 0, &CartLoad, CreateEvent(NULL, FALSE, FALSE, NULL), 0, &threadID);
  }
}

// LoadIniFile allows user to browse for an ini file and reloads the config from it.
extern "C" {
  __declspec(dllexport) void __cdecl LoadIniFile(void)
  {
    OPENFILENAME ofn;
    char szFileName[MAX_PATH] = "";

    static EmuState* _emu = GetEmuState();

    GetIniFilePath(szFileName); // EJJ load current ini file path

    memset(&ofn, 0, sizeof(ofn));

    ofn.lStructSize = sizeof(OPENFILENAME);
    ofn.hwndOwner = _emu->WindowHandle;
    ofn.lpstrFilter = "INI\0*.ini\0\0";
    ofn.nFilterIndex = 1;
    ofn.lpstrFile = szFileName;
    ofn.nMaxFile = MAX_PATH;
    ofn.nMaxFileTitle = MAX_PATH;
    ofn.lpstrFileTitle = NULL;
    ofn.lpstrInitialDir = AppDirectory();
    ofn.lpstrTitle = TEXT("Load Vcc Config File");
    ofn.Flags = OFN_HIDEREADONLY | OFN_FILEMUSTEXIST;

    if (GetOpenFileName(&ofn)) {
      WriteIniFile(_emu);    // Flush current profile
      SetIniFilePath(szFileName);          // Set new ini file path
      ReadIniFile(_emu);     // Load it
      SynchSystemWithConfig(_emu);

      _emu->ResetPending = RESET_HARD;
    }
  }
}

// Force keys up if main widow keyboard focus is lost.  Otherwise
// down keys will cause issues with OS-9 on return
// Send key up events to keyboard handler for saved keys
extern "C" {
  __declspec(dllexport) void __cdecl SendSavedKeyEvents() {
    if (instance->SC_save1) {
      vccKeyboardHandleKey(instance->KB_save1, instance->SC_save1, kEventKeyUp);
    }

    if (instance->SC_save2) {
      vccKeyboardHandleKey(instance->KB_save2, instance->SC_save2, kEventKeyUp);
    }

    instance->SC_save1 = 0;
    instance->SC_save2 = 0;
  }
}

void GimeReset(void)
{
  ResetGraphicsState();

  MakeRGBPalette();
  MakeCMPpalette(GetPaletteType());

  CocoReset();
  ResetAudio();
}

extern "C" {
  __declspec(dllexport) void __cdecl SoftReset(void)
  {
    MC6883Reset();
    MC6821_PiaReset();

    GetCPU()->CPUReset();

    GimeReset();
    MmuReset();
    CopyRom();
    ResetBus();

    static EmuState* _emu = GetEmuState();

    _emu->TurboSpeedFlag = 1;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl HardReset(EmuState* emuState)
  {
    //Allocate RAM/ROM & copy ROM Images from source
    if (MmuInit(emuState->RamSize) == NULL)
    {
      MessageBox(NULL, "Can't allocate enough RAM, out of memory", "Error", 0);

      exit(0);
    }

    CPU* cpu = GetCPU();

    if (emuState->CpuType == 1)
    {
      cpu->CPUInit = HD6309Init;
      cpu->CPUExec = HD6309Exec;
      cpu->CPUReset = HD6309Reset;
      cpu->CPUAssertInterrupt = HD6309AssertInterrupt;
      cpu->CPUDeAssertInterrupt = HD6309DeAssertInterrupt;
      cpu->CPUForcePC = HD6309ForcePC;
    }
    else
    {
      cpu->CPUInit = MC6809Init;
      cpu->CPUExec = MC6809Exec;
      cpu->CPUReset = MC6809Reset;
      cpu->CPUAssertInterrupt = MC6809AssertInterrupt;
      cpu->CPUDeAssertInterrupt = MC6809DeAssertInterrupt;
      cpu->CPUForcePC = MC6809ForcePC;
    }

    MC6821_PiaReset();
    MC6883Reset();	//Captures interal rom pointer for CPU Interrupt Vectors

    cpu->CPUInit();
    cpu->CPUReset();		// Zero all CPU Registers and sets the PC to VRESET

    GimeReset();
    UpdateBusPointer();

    static EmuState* _emu = GetEmuState();

    _emu->TurboSpeedFlag = 1;

    ResetBus();
    SetClockSpeed(1);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl EmuLoop() {
    static float fps;
    static unsigned int frameCounter = 0;

    static EmuState* _emu = GetEmuState();

    while (true)
    {
      if (instance->FlagEmuStop == TH_REQWAIT)
      {
        instance->FlagEmuStop = TH_WAITING; //Signal Main thread we are waiting

        while (instance->FlagEmuStop == TH_WAITING) {
          Sleep(1);
        }
      }

      fps = 0;

      if ((instance->Qflag == 255) && (frameCounter == 30))
      {
        instance->Qflag = 0;

        QuickLoad(_emu, instance->QuickLoadFile);
      }

      StartRender();

      for (uint8_t frames = 1; frames <= _emu->FrameSkip; frames++)
      {
        frameCounter++;

        if (_emu->ResetPending != RESET_CLEAR) {
          switch (_emu->ResetPending)
          {
          case RESET_SOFT:	//Soft Reset
            SoftReset();
            break;

          case RESET_HARD:	//Hard Reset
            SynchSystemWithConfig(_emu);
            DoCls(_emu);
            HardReset(_emu);

            break;

          case RESET_CLS:
            DoCls(_emu);
            break;

          case RESET_CLS_SYNCH:
            SynchSystemWithConfig(_emu);
            DoCls(_emu);

            break;

          default:
            break;
          }

          _emu->ResetPending = RESET_CLEAR;
        }

        if (_emu->EmulationRunning) {
          fps += RenderFrame(_emu);
        }
        else {
          fps += Static(_emu);
        }
      }

      EndRender(_emu->FrameSkip);

      fps /= _emu->FrameSkip;

      GetModuleStatus(_emu);

      char ttbuff[256];

      snprintf(ttbuff, sizeof(ttbuff), "Skip:%2.2i | FPS:%3.0f | %s @ %2.2fMhz| %s", _emu->FrameSkip, fps, instance->CpuName, _emu->CPUCurrentSpeed, _emu->StatusLine);

      SetStatusBarText(ttbuff, _emu);

      if (instance->Throttle) { //Do nothing until the frame is over returning unused time to OS
        FrameWait();
      }
    }
  }
}

extern "C" {
  __declspec(dllexport) unsigned __stdcall EmuLoopRun(void* dummy)
  {
    HANDLE hEvent = (HANDLE)dummy;

    //NOTE: This function isn't working in library.dll
    timeBeginPeriod(1);	//Needed to get max resolution from the timer normally its 10Ms
    CalibrateThrottle();
    timeEndPeriod(1);

    Sleep(30);
    SetEvent(hEvent);

    EmuLoop();

    return(NULL);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl CheckQuickLoad(char* qLoadFile) {
    char temp1[MAX_PATH] = "";
    char temp2[MAX_PATH] = " Running on ";

    if (strlen(qLoadFile) != 0)
    {
      strcpy(instance->QuickLoadFile, qLoadFile);
      strcpy(temp1, qLoadFile);

      FilePathStripPath(temp1);

      _strlwr(temp1);

      temp1[0] = toupper(temp1[0]);

      strcat(temp1, temp2);
      strcat(temp1, instance->AppName);
      strcpy(instance->AppName, temp1);

      instance->Qflag = 0xFF;
    }
  };
}

/*--------------------------------------------------------------------------*/
// The Window Procedure
LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
  ProcessMessage(hWnd, message, wParam, lParam);

  return DefWindowProc(hWnd, message, wParam, lParam);
}

extern "C" {
  __declspec(dllexport) void __cdecl CreatePrimaryWindow() {
    static EmuState* _emu = GetEmuState();

    if (!CreateDirectDrawWindow(_emu, WndProc))
    {
      MessageBox(0, "Can't create primary window", "Error", 0);

      exit(0);
    }
  }
}

extern "C" {
  __declspec(dllexport) void CheckScreenModeChange() {
    if (instance->FlagEmuStop == TH_WAITING)		//Need to stop the EMU thread for screen mode change
    {								                  //As it holds the Secondary screen buffer open while running
      FullScreenToggle(WndProc);

      instance->FlagEmuStop = TH_RUNNING;
    }
  }
}

HANDLE CreateEventHandle() {
  HANDLE hEvent = CreateEvent(NULL, FALSE, FALSE, NULL);

  if (hEvent == NULL)
  {
    MessageBox(0, "Can't create event thread!!", "Error", 0);

    exit(0);
  }

  return hEvent;
}

HANDLE CreateThreadHandle(HANDLE hEvent) {
  unsigned threadID;

  HANDLE hThread = (HANDLE)_beginthreadex(NULL, 0, &EmuLoopRun, hEvent, 0, &threadID);

  if (hThread == NULL)
  {
    MessageBox(0, "Can't Start main Emulation Thread!", "Ok", 0);

    exit(0);
  }

  return hThread;
}

extern "C" {
  __declspec(dllexport) void __cdecl VccStartupThreading() {
    instance->hEventThread = CreateEventHandle();
    instance->hEmuThread = CreateThreadHandle(instance->hEventThread);

    WaitForSingleObject(instance->hEventThread, INFINITE);
    SetThreadPriority(instance->hEmuThread, THREAD_PRIORITY_NORMAL);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl VccStartup(HINSTANCE hInstance, CmdLineArguments* cmdArg, EmuState* emu) {
    HANDLE OleInitialize(NULL); //Work around fixs app crashing in "Open file" system dialogs (related to Adobe acrobat 7+)

    InitConfig(emu, cmdArg);

    if (strlen(cmdArg->QLoadFile) != 0)
    {
      emu->EmulationRunning = true;
    }

    Cls(0, emu);
    DynamicMenuCallback(emu, "", 0, 0);
    DynamicMenuCallback(emu, "", 1, 0);

    emu->ResetPending = RESET_HARD;
    emu->EmulationRunning = instance->AutoStart;

    instance->BinaryRunning = true;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl VccRun() {

    MSG* msg = &(instance->msg);

    while (instance->BinaryRunning)
    {
      CheckScreenModeChange();

      GetMessage(msg, NULL, 0, 0);		//Seems if the main loop stops polling for Messages the child threads stall

      TranslateMessage(msg);

      DispatchMessage(msg);
    }
  }
}

extern "C" {
  __declspec(dllexport) INT __cdecl VccShutdown() {
    static EmuState* _emu = GetEmuState();

    CloseHandle(instance->hEventThread);
    CloseHandle(instance->hEmuThread);
    UnloadDll(_emu);
    SoundDeInit();
    WriteIniFile(_emu); //Save Any changes to ini File

    return (INT)(instance->msg.wParam);
  }
}
