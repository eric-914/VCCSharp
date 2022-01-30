using System.Diagnostics;
using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Main;

namespace VCCSharp.Modules
{
    /*
     * TODO: Invoke Cartridge Menu Item click
     * // Parse the menu selections:
     * // Added for Dynamic menu system
     * if (wmId >= Define.ID_DYNAMENU_START && wmId <= Define.ID_DYNAMENU_END)
     * {
     *      Task.Run(() => { _modules.MenuCallbacks.CartridgeMenuItemClicked(wmId);});
     * }
     */
    public interface IEvents
    {
        void EmuRun();
        void EmuReset(ResetPendingStates state);
        void EmuExit();
        void Shutdown();
        void ToggleOnOff();
        void SlowDown();
        void SpeedUp();
        void ToggleMonitorType();
        void ToggleThrottle();
        void ToggleFullScreen();
        void ToggleInfoBand();
        void Cancel();
        void ResetWindow(IMainWindow window);
    }

    public class Events : IEvents
    {
        private readonly IModules _modules;
        private IGraphics Graphics => _modules.Graphics;

        public Events(IModules modules)
        {
            _modules = modules;
        }

        public void EmuRun()
        {
            _modules.Emu.EmulationRunning = true;

            Graphics.InvalidateBorder();
        }

        public void EmuReset(ResetPendingStates state)
        {
            if (_modules.Emu.EmulationRunning)
            {
                _modules.Clipboard.Abort = true; //--Abort Pasting if happening

                _modules.Emu.ResetPending = (byte)state;
            }
        }

        public void EmuExit()
        {
            Debug.WriteLine("Exiting...");
            _modules.Config.Save(); //Save any changes to ini File

            _modules.Vcc.BinaryRunning = false;
        }

        public void Shutdown()
        {
            _modules.PAKInterface.UnloadDll(false);
            _modules.Audio.SoundDeInit();
        }

        public void SlowDown() //F3
        {
            _modules.Config.DecreaseOverclockSpeed();
        }

        public void SpeedUp() //F4
        {
            _modules.Config.IncreaseOverclockSpeed();
        }

        public void ToggleMonitorType() //F6
        {
            MonitorTypes monType = Graphics.MonitorType == MonitorTypes.Composite ? MonitorTypes.RGB : MonitorTypes.Composite;

            _modules.Graphics.SetMonitorType(monType);
        }

        public void ToggleThrottle() //F8
        {
            _modules.Vcc.Throttle = !_modules.Vcc.Throttle;
        }

        public void ToggleOnOff() //F9
        {
            _modules.Emu.EmulationRunning = !_modules.Emu.EmulationRunning;

            EmuReset(ResetPendingStates.Hard);

            _modules.Draw.SetStatusBarText("");
        }

        public void ToggleInfoBand() //F10
        {
            _modules.Draw.InfoBand = !_modules.Draw.InfoBand;

            _modules.Graphics.InvalidateBorder();
        }

        public void ToggleFullScreen() //F11
        {
            if (_modules.Vcc.RunState == (byte)EmuRunStates.Running)
            {
                _modules.Vcc.RunState = (byte)EmuRunStates.ReqWait;
                _modules.Emu.FullScreen = !_modules.Emu.FullScreen;
            }
        }

        public void Cancel() //ESC
        {
            _modules.Clipboard.Abort = true; //--Abort clipboard (paste) actions
        }

        /// <summary>
        /// Resize main window such that the view size is exactly 640x480
        /// </summary>
        /// <param name="window"></param>
        public void ResetWindow(IMainWindow window)
        {
            Size wSize = window.Window.RenderSize;
            Size vSize = window.View.RenderSize;
            Size size = new Size(wSize.Width + 640 - vSize.Width, wSize.Height + 480 - vSize.Height);

            window.ViewModel.WindowHeight = size.Height;
            window.ViewModel.WindowWidth = size.Width;
        }
    }
}
