﻿using System;
using System.Threading.Tasks;
using System.Windows.Interop;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using static System.IntPtr;

namespace VCCSharp
{
    public interface IVccApp
    {
        void LoadConfiguration(string iniFile);
        void Startup(string qLoadFile);
        void Startup();
        void Threading();
        void Run(string qLoadFile);

        void SetWindow(IntPtr hWnd);
    }

    public class VccApp : IVccApp
    {
        private readonly IModules _modules;
        private readonly IUser32 _user32;

        public VccApp(IModules modules, IUser32 user32)
        {
            _modules = modules;
            _user32 = user32;
        }

        public void LoadConfiguration(string iniFile)
        {
            _modules.Config.InitConfig(iniFile);
        }

        public void Startup(string qLoadFile)
        {
            if (!string.IsNullOrEmpty(qLoadFile))
            {
                if (_modules.QuickLoad.QuickStart(qLoadFile) == (int)QuickStartStatuses.Ok)
                {
                    _modules.Vcc.SetAppTitle(qLoadFile); //TODO: No app title if no quick load
                }

                _modules.Emu.EmulationRunning = true;
            }
        }

        public void Startup()
        {
            _modules.CoCo.SetAudioEventAudioOut();

            _modules.Keyboard.SetKeyTranslations();

            _modules.CoCo.OverClock = 1;  //Default clock speed .89 MHZ	

            _modules.Vcc.CreatePrimaryWindow();

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

        public void Run(string qLoadFile)
        {
            Startup(qLoadFile);

            while (_modules.Vcc.BinaryRunning)
            {
                _modules.Vcc.CheckScreenModeChange();

                MSG msg;

                _user32.GetMessageA(ref msg, Zero, 0, 0);   //Seems if the main loop stops polling for Messages the child threads stall

                _user32.TranslateMessage(ref msg);

                _user32.DispatchMessageA(ref msg);
            }
        }

        public void SetWindow(IntPtr hWnd)
        {
            _modules.Emu.WindowHandle = hWnd;
        }
    }
}
