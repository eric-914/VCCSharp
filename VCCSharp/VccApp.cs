using System;
using System.Windows.Interop;
using VCCSharp.Enums;
using VCCSharp.Libraries;
using VCCSharp.Models;
using VCCSharp.Modules;

namespace VCCSharp
{
    public class VccApp
    {
        private IntPtr _hResources;
        //private EmuState _emuState;

        private readonly DirectDraw _directDraw = new DirectDraw();
        private readonly CoCo _coco = new CoCo();
        private readonly Config _config = new Config();
        private readonly MenuCallbacks _menuCallbacks = new MenuCallbacks();
        private readonly QuickLoad _quickLoad = new QuickLoad();
        private readonly PAKInterface _pakInterface = new PAKInterface();
        private readonly Audio _audio = new Audio();

        public void Startup(IntPtr hInstance, CmdLineArguments cmdLineArgs)
        {
            unsafe
            {
                _hResources = Kernel.LoadLibrary("resources.dll");

                EmuState* emuState = Library.Emu.GetEmuState();
                VccState* vccState = Library.Vcc.GetVccState();

                emuState->Resources = _hResources;

                //TODO: Redundant at the moment
                Library.Emu.SetEmuState(emuState);

                _directDraw.InitDirectDraw(hInstance, _hResources);

                _coco.SetClockSpeed(1);  //Default clock speed .89 MHZ	

                if (!string.IsNullOrEmpty(cmdLineArgs.QLoadFile))
                {
                    if (_quickLoad.QuickStart(emuState, cmdLineArgs.QLoadFile) == (int)QuickStartStatuses.Ok)
                    {
                        Library.Vcc.SetAppTitle(_hResources, cmdLineArgs.QLoadFile); //TODO: No app title if no quick load
                    }

                    emuState->EmulationRunning = Define.TRUE;
                }

                Library.Vcc.CreatePrimaryWindow();

                //NOTE: Sound is lost if this isn't done after CreatePrimaryWindow();
                //Loads the default config file Vcc.ini from the exec directory
                _config.InitConfig(emuState, ref cmdLineArgs);

                _directDraw.ClearScreen();

                emuState->ResetPending = (byte)ResetPendingStates.Cls;

                _menuCallbacks.DynamicMenuCallback(emuState, null, (int)MenuActions.Refresh, Define.IGNORE);

                emuState->ResetPending = (byte)ResetPendingStates.Hard;

                emuState->EmulationRunning = vccState->AutoStart;

                vccState->BinaryRunning = Define.TRUE;
            }
        }

        public void Threading()
        {
            unsafe
            {
                VccState* vccState = Library.Vcc.GetVccState();

                vccState->hEventThread = Library.Vcc.CreateEventHandle();
                vccState->hEmuThread = Library.Vcc.CreateThreadHandle(vccState->hEventThread);

                Kernel.WaitForSingleObject(vccState->hEventThread, Define.INFINITE);
                Kernel.SetThreadPriority(vccState->hEmuThread, Define.THREAD_PRIORITY_NORMAL);
            }
        }

        public void Run()
        {
            unsafe
            {
                VccState* vccState = Library.Vcc.GetVccState();

                while (vccState->BinaryRunning == Define.TRUE)
                {
                    Library.Vcc.CheckScreenModeChange();

                    MSG* msg = &(vccState->msg);

                    User32.GetMessageA(msg, IntPtr.Zero, 0, 0);   //Seems if the main loop stops polling for Messages the child threads stall

                    User32.TranslateMessage(msg);

                    User32.DispatchMessageA(msg);
                }
            }
        }

        public int Shutdown()
        {
            unsafe
            {
                VccState* vccState = Library.Vcc.GetVccState();
                EmuState* emuState = Library.Emu.GetEmuState();

                Kernel.CloseHandle(vccState->hEventThread);
                Kernel.CloseHandle(vccState->hEmuThread);

                _pakInterface.UnloadDll(emuState);
                _audio.SoundDeInit();

                _config.WriteIniFile(emuState); //Save any changes to ini File

                int code = (int)vccState->msg.wParam;

                Kernel.FreeLibrary(_hResources);

                return code;
            }
        }
    }
}
