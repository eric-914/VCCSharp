using System;
using System.Threading.Tasks;
using System.Windows.Interop;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using static System.IntPtr;

namespace VCCSharp
{
    public interface IVccApp
    {
        void Startup(CmdLineArguments cmdLineArgs);
        void Threading();
        void Run();

        void SetWindow(IntPtr hWnd);
    }

    public class VccApp : IVccApp
    {
        private readonly IModules _modules;
        private readonly IUser32 _user32;

        public MSG Msg;

        public VccApp(IModules modules, IUser32 user32)
        {
            _modules = modules;
            _user32 = user32;
        }

        public void Startup(CmdLineArguments cmdLineArgs)
        {
            _modules.CoCo.SetAudioEventAudioOut();

            _modules.Keyboard.SetKeyTranslations();

            _modules.CoCo.OverClock = 1;  //Default clock speed .89 MHZ	

            _modules.Config.InitConfig(ref cmdLineArgs);

            _modules.Vcc.CreatePrimaryWindow();

            if (!string.IsNullOrEmpty(cmdLineArgs.QLoadFile))
            {
                if (_modules.QuickLoad.QuickStart(cmdLineArgs.QLoadFile) == (int)QuickStartStatuses.Ok)
                {
                    _modules.Vcc.SetAppTitle(cmdLineArgs.QLoadFile); //TODO: No app title if no quick load
                }

                _modules.Emu.EmulationRunning = true;
            }

            _modules.Draw.ClearScreen();

            _modules.Emu.ResetPending = (byte)ResetPendingStates.Cls;

            _modules.MenuCallbacks.RefreshCartridgeMenu();

            _modules.Emu.ResetPending = (byte)ResetPendingStates.Hard;

            _modules.Emu.EmulationRunning = _modules.Vcc.AutoStart;

            _modules.Vcc.BinaryRunning = true;

            _modules.Throttle.CalibrateThrottle();
        }

        public void Threading()
        {
            Task.Run(_modules.Vcc.EmuLoop);
        }

        public void Run()
        {
            while (_modules.Vcc.BinaryRunning)
            {
                _modules.Vcc.CheckScreenModeChange();

                _user32.GetMessageA(ref Msg, Zero, 0, 0);   //Seems if the main loop stops polling for Messages the child threads stall

                _user32.TranslateMessage(ref Msg);

                _user32.DispatchMessageA(ref Msg);
            }
        }

        public void SetWindow(IntPtr hWnd)
        {
            _modules.Emu.WindowHandle = hWnd;
        }
    }
}
