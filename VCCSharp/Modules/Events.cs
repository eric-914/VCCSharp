using System.Diagnostics;
using System.Threading.Tasks;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using KeyStates = VCCSharp.Enums.KeyStates;

namespace VCCSharp.Modules
{
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
        void ProcessMessage(int message, long wParam, long lParam);
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

        //----------------------------------------------------------------------------------------
        //	lParam bits
        //	  0-15	The repeat count for the current message. The value is the number of times
        //			the keystroke is auto repeated as a result of the user holding down the key.
        //			If the keystroke is held long enough, multiple messages are sent. However,
        //			the repeat count is not cumulative.
        //	 16-23	The scan code. The value depends on the OEM.
        //	    24	Indicates whether the key is an extended key, such as the right-hand ALT and
        //			CTRL keys that appear on an enhanced 101- or 102-key keyboard. The value is
        //			one if it is an extended key; otherwise, it is zero.
        //	 25-28	Reserved; do not use.
        //	    29	The context code. The value is always zero for a WM_KEYDOWN message.
        //	    30	The previous key state. The value is one if the key is down before the
        //	   		message is sent, or it is zero if the key is up.
        //	    31	The transition state. The value is always zero for a WM_KEYDOWN message.
        //----------------------------------------------------------------------------------------
        public void ProcessMessage(int message, long wParam, long lParam)
        {
            switch (message)
            {
                case Define.WM_COMMAND:
                    ProcessCommandMessage(wParam);
                    break;

                case Define.WM_KEYDOWN:
                    KeyDown(lParam);
                    break;

                case Define.WM_KEYUP:
                    KeyUp(lParam);
                    break;

                case Define.WM_SYSCOMMAND:
                    ProcessSysCommandMessage(wParam);
                    break;

                case Define.WM_SYSKEYDOWN:
                    // Ignore repeated system keys
                    if (lParam >> 30 == 0) return;
                    KeyDown(lParam);
                    break;

                case Define.WM_SYSKEYUP:
                    KeyUp(lParam);
                    break;
            }
        }

        private void ProcessSysCommandMessage(long wParam)
        {
            //-------------------------------------------------------------
            // Control ATL key menu access.
            // Here left ALT is hardcoded off and right ALT on
            // TODO: Add config check boxes to control them
            //-------------------------------------------------------------

            bool notKeyMenu = wParam != Define.SC_KEYMENU;
            short keyState = (short)(_user32.GetKeyState((int)Define.VK_LMENU) & 0x8000);

            if (notKeyMenu || (keyState != 0))
            {
                ProcessCommandMessage(wParam);
            }
        }

        private void ProcessCommandMessage(long wParam)
        {
            // Force all keys up to prevent key repeats
            _modules.Keyboard.SendSavedKeyEvents();

            int wmId = (int)(wParam & 0xFFFF); //LOWORD(wParam);

            // Parse the menu selections:
            // Added for Dynamic menu system
            if (wmId >= Define.ID_DYNAMENU_START && wmId <= Define.ID_DYNAMENU_END)
            {
                Task.Run(() =>
                {
                    _modules.MenuCallbacks.CartridgeMenuItemClicked(wmId); 
                });
            }
        }

        private void KeyUp(long lParam)
        {
            // send emulator key up event to the emulator
            // TODO: Key up checks whether the emulation is running, this does not

            byte oemScan = (byte)((lParam & 0x00FF0000) >> 16);

            KeyboardHandleKey(oemScan, KeyStates.Up);
        }

        private void KeyDown(long lParam)
        {
            byte oemScan = (byte)((lParam & 0x00FF0000) >> 16); // just get the scan code

            // send other keystrokes to the emulator if it is active
            if (_modules.Emu.EmulationRunning)
            {
                KeyboardHandleKey(oemScan, KeyStates.Down);
            }
        }

        private void KeyboardHandleKey(byte oemScan, KeyStates keyState)
        {
            //_modules.Keyboard.KeyboardHandleKey(oemScan, keyState);
        }

    }
}
