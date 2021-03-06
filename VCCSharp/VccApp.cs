using System;
using System.Windows.Interop;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using VCCSharp.Modules;
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
        private readonly IKernel _kernel;
        private readonly IUser32 _user32;

        private HINSTANCE _hResources;
        //private EmuState _emuState;

        private readonly IVcc _vcc;
        private readonly IEmu _emu;
        private readonly IDirectDraw _directDraw;
        private readonly ICoCo _coco;
        private readonly IConfig _config;
        private readonly IMenuCallbacks _menuCallbacks;
        private readonly IQuickLoad _quickLoad;
        private readonly IPAKInterface _pakInterface;
        private readonly IAudio _audio;

        public VccApp(IModules modules, IKernel kernel, IUser32 user32)
        {
            _kernel = kernel;
            _user32 = user32;

            _vcc = modules.Vcc;
            _emu = modules.Emu;
            _directDraw = modules.DirectDraw;
            _coco = modules.CoCo;
            _config = modules.Config;
            _menuCallbacks = modules.MenuCallbacks;
            _quickLoad = modules.QuickLoad;
            _pakInterface = modules.PAKInterface;
            _audio = modules.Audio;
        }

        public void Startup(HINSTANCE hInstance, CmdLineArguments cmdLineArgs)
        {
            unsafe
            {
                _hResources = _kernel.LoadLibrary("resources.dll");

                EmuState* emuState = _emu.GetEmuState();
                VccState* vccState = _vcc.GetVccState();

                emuState->Resources = _hResources;

                //TODO: Redundant at the moment
                _emu.SetEmuState(emuState);

                _directDraw.InitDirectDraw(hInstance, _hResources);

                _coco.SetClockSpeed(1);  //Default clock speed .89 MHZ	

                if (!string.IsNullOrEmpty(cmdLineArgs.QLoadFile))
                {
                    if (_quickLoad.QuickStart(emuState, cmdLineArgs.QLoadFile) == (int)QuickStartStatuses.Ok)
                    {
                        _vcc.SetAppTitle(_hResources, cmdLineArgs.QLoadFile); //TODO: No app title if no quick load
                    }

                    emuState->EmulationRunning = Define.TRUE;
                }

                _vcc.CreatePrimaryWindow();

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
                VccState* vccState = _vcc.GetVccState();

                vccState->hEventThread = _vcc.CreateEventHandle();
                vccState->hEmuThread = _vcc.CreateThreadHandle(vccState->hEventThread);

                _kernel.WaitForSingleObject(vccState->hEventThread, Define.INFINITE);
                _kernel.SetThreadPriority(vccState->hEmuThread, Define.THREAD_PRIORITY_NORMAL);
            }
        }

        public void Run()
        {
            unsafe
            {
                VccState* vccState = _vcc.GetVccState();

                while (vccState->BinaryRunning == Define.TRUE)
                {
                    _vcc.CheckScreenModeChange();

                    MSG* msg = &(vccState->msg);

                    _user32.GetMessageA(msg, IntPtr.Zero, 0, 0);   //Seems if the main loop stops polling for Messages the child threads stall

                    _user32.TranslateMessage(msg);

                    _user32.DispatchMessageA(msg);
                }
            }
        }

        public int Shutdown()
        {
            unsafe
            {
                VccState* vccState = _vcc.GetVccState();
                EmuState* emuState = _emu.GetEmuState();

                _kernel.CloseHandle(vccState->hEventThread);
                _kernel.CloseHandle(vccState->hEmuThread);

                _pakInterface.UnloadDll(emuState);
                _audio.SoundDeInit();

                _config.WriteIniFile(emuState); //Save any changes to ini File

                int code = (int)vccState->msg.wParam;

                _kernel.FreeLibrary(_hResources);

                return code;
            }
        }
    }
}
