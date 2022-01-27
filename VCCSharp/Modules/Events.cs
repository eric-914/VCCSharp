using System.Diagnostics;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;

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
        void ToggleOnOff();
        void SlowDown();
        void SpeedUp();
        void ToggleMonitorType();
        void ToggleThrottle();
        void ToggleFullScreen();
        void ToggleInfoBand();
        void Cancel();
    }

    public class Events : IEvents
    {
        private readonly IModules _modules;
        private readonly IUser32 _user32;
        private IGraphics Graphics => _modules.Graphics;

        public Events(IModules modules, IUser32 user32)
        {
            _modules = modules;
            _user32 = user32;
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

            _modules.Vcc.BinaryRunning = false;
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
    }
}
