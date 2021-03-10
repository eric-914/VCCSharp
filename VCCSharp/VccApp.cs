﻿using System.Threading.Tasks;
using System.Windows.Interop;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using static System.IntPtr;
using HINSTANCE = System.IntPtr;

namespace VCCSharp
{
    public interface IVccApp
    {
        void Startup(HINSTANCE hInstance, CmdLineArguments cmdLineArgs);
        void Threading();
        void Run();
        int Shutdown();
    }

    public class VccApp : IVccApp
    {
        private readonly IModules _modules;
        private readonly IKernel _kernel;
        private readonly IUser32 _user32;

        private HINSTANCE _hResources;
        //private EmuState _emuState;

        public VccApp(IModules modules, IKernel kernel, IUser32 user32)
        {
            _modules = modules;
            _kernel = kernel;
            _user32 = user32;
        }

        public void Startup(HINSTANCE hInstance, CmdLineArguments cmdLineArgs)
        {
            unsafe
            {
                //AudioState* audioState = _modules.Audio.GetAudioState();

                _hResources = _kernel.LoadLibrary("resources.dll");

                EmuState* emuState = _modules.Emu.GetEmuState();
                VccState* vccState = _modules.Vcc.GetVccState();

                emuState->Resources = _hResources;

                //TODO: Redundant at the moment
                _modules.Emu.SetEmuState(emuState);

                _modules.DirectDraw.InitDirectDraw(hInstance, _hResources);

                _modules.CoCo.SetClockSpeed(1);  //Default clock speed .89 MHZ	

                if (!string.IsNullOrEmpty(cmdLineArgs.QLoadFile))
                {
                    if (_modules.QuickLoad.QuickStart(emuState, cmdLineArgs.QLoadFile) == (int)QuickStartStatuses.Ok)
                    {
                        _modules.Vcc.SetAppTitle(_hResources, cmdLineArgs.QLoadFile); //TODO: No app title if no quick load
                    }

                    emuState->EmulationRunning = Define.TRUE;
                }

                _modules.Vcc.CreatePrimaryWindow();

                //NOTE: Sound is lost if this isn't done after CreatePrimaryWindow();
                //Loads the default config file Vcc.ini from the exec directory
                _modules.Config.InitConfig(emuState, ref cmdLineArgs);

                _modules.DirectDraw.ClearScreen();

                emuState->ResetPending = (byte)ResetPendingStates.Cls;

                _modules.MenuCallbacks.DynamicMenuCallback(emuState, null, MenuActions.Refresh, Define.IGNORE);

                emuState->ResetPending = (byte)ResetPendingStates.Hard;

                emuState->EmulationRunning = vccState->AutoStart;

                vccState->BinaryRunning = Define.TRUE;

                _modules.Throttle.CalibrateThrottle();
            }
        }

        public void Threading()
        {
            Task.Run(_modules.Vcc.EmuLoop);
        }

        public void Run()
        {
            unsafe
            {
                VccState* vccState = _modules.Vcc.GetVccState();

                while (vccState->BinaryRunning == Define.TRUE)
                {
                    _modules.Vcc.CheckScreenModeChange();

                    MSG* msg = &(vccState->msg);

                    _user32.GetMessageA(msg, Zero, 0, 0);   //Seems if the main loop stops polling for Messages the child threads stall

                    _user32.TranslateMessage(msg);

                    _user32.DispatchMessageA(msg);
                }
            }
        }

        public int Shutdown()
        {
            unsafe
            {
                VccState* vccState = _modules.Vcc.GetVccState();
                EmuState* emuState = _modules.Emu.GetEmuState();

                _modules.PAKInterface.UnloadDll(emuState);
                _modules.Audio.SoundDeInit();

                _modules.Config.WriteIniFile(emuState); //Save any changes to ini File

                int code = (int)vccState->msg.wParam;

                _kernel.FreeLibrary(_hResources);

                return code;
            }
        }
    }
}
