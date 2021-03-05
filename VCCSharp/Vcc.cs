using System;
using VCCSharp.Enums;
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
                _hResources = Library.LoadLibrary("resources.dll");

                EmuState* emuState = Library.Emu.GetEmuState();
                VccState* vccState = Library.Vcc.GetVccState();

                emuState->Resources = _hResources;

                //TODO: Redundant at the moment
                Library.Emu.SetEmuState(emuState);

                Library.DirectDraw.InitDirectDraw(hInstance, _hResources);

                Library.CoCo.SetClockSpeed(1);  //Default clock speed .89 MHZ	

                if (!string.IsNullOrEmpty(cmdLineArgs.QLoadFile))
                {
                    if (Library.QuickLoad.QuickStart(emuState, cmdLineArgs.QLoadFile) == 0)
                    {
                        Library.Vcc.SetAppTitle(_hResources, cmdLineArgs.QLoadFile); //TODO: No app title if no quick load
                    }

                    emuState->EmulationRunning = 1; //true
                }

                Library.Vcc.CreatePrimaryWindow();

                //NOTE: Sound is lost if this isn't done after CreatePrimaryWindow();
                //Loads the default config file Vcc.ini from the exec directory
                Library.Config.InitConfig(emuState, ref cmdLineArgs);

                Library.DirectDraw.ClearScreen();

                emuState->ResetPending = (byte)ResetPendingStates.Cls;

                Library.MenuCallbacks.DynamicMenuCallback(emuState, null, (int)MenuActions.Refresh, 0);

                emuState->ResetPending = (byte)ResetPendingStates.Hard;

                emuState->EmulationRunning = vccState->AutoStart;

                vccState->BinaryRunning = 1; //true
            }

            Library.Vcc.VccStartupThreading();
        }

        public void Run()
        {
            unsafe
            {
                VccState* vccState = Library.Vcc.GetVccState();

                while (vccState->BinaryRunning != 0)
                {
                    Library.Vcc.CheckScreenModeChange();

                    Library.Vcc.VccRun();
                }
            }
        }

        public int Shutdown()
        {
            var code = Library.Vcc.VccShutdown();

            Library.FreeLibrary(_hResources);

            return code;
        }
    }
}
