using System;
using VCCSharp.Enums;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp
{
    public class Vcc
    {
        private IntPtr _hResources;
        //private EmuState _emuState;

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

                Library.DirectDraw.InitDirectDraw(hInstance, _hResources);

                Library.CoCo.SetClockSpeed(1);  //Default clock speed .89 MHZ	

                if (!string.IsNullOrEmpty(cmdLineArgs.QLoadFile))
                {
                    if (Library.QuickLoad.QuickStart(emuState, cmdLineArgs.QLoadFile) == (int)QuickStartStatuses.Ok)
                    {
                        Library.Vcc.SetAppTitle(_hResources, cmdLineArgs.QLoadFile); //TODO: No app title if no quick load
                    }

                    emuState->EmulationRunning = Define.TRUE;
                }

                Library.Vcc.CreatePrimaryWindow();

                //NOTE: Sound is lost if this isn't done after CreatePrimaryWindow();
                //Loads the default config file Vcc.ini from the exec directory
                Library.Config.InitConfig(emuState, ref cmdLineArgs);

                Library.DirectDraw.ClearScreen();

                emuState->ResetPending = (byte)ResetPendingStates.Cls;

                Library.MenuCallbacks.DynamicMenuCallback(emuState, null, (int)MenuActions.Refresh, Define.IGNORE);

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

                    Library.Vcc.VccRun();
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

                Library.PAKInterface.UnloadDll(emuState);
                Library.Audio.SoundDeInit();

                Library.Config.WriteIniFile(emuState); //Save any changes to ini File

                int code = Library.Vcc.VccShutdown();

                Kernel.FreeLibrary(_hResources);

                return code;
            }
        }
    }
}
