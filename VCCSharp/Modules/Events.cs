using System.Threading.Tasks;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using HWND = System.IntPtr;
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
        void ProcessMessage(HWND hWnd, uint message, long wParam, long lParam);
    }

    public class Events : IEvents
    {
        private readonly IModules _modules;
        private readonly IUser32 _user32;
        private IGraphics Graphics => _modules.Graphics;

        //--------------------------------------------------------------------------
        // When the main window is about to lose keyboard focus there are one
        // or two keys down in the emulation that must be raised.  These routines
        // track the last two key down events so they can be raised when needed.
        //--------------------------------------------------------------------------
        private byte SC_save1;
        private byte SC_save2;
        private byte KB_save1;
        private byte KB_save2;

        private bool KeySaveToggle;

        public Events(IModules modules, IUser32 user32)
        {
            _modules = modules;
            _user32 = user32;
        }

        public void EmuRun()
        {
            _modules.Emu.EmulationRunning = Define.TRUE;

            Graphics.InvalidateBorder();
        }

        public void EmuReset(ResetPendingStates state)
        {
            if (_modules.Emu.EmulationRunning == Define.TRUE)
            {
                _modules.Emu.ResetPending = (byte)state;
            }
        }

        public void EmuExit()
        {
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
            byte monType = Graphics.MonType == Define.FALSE ? Define.TRUE : Define.FALSE;

            _modules.Graphics.SetMonitorType(monType);
        }

        public void ToggleThrottle() //F8
        {
            _modules.Vcc.Throttle = _modules.Vcc.Throttle == Define.TRUE ? Define.FALSE : Define.TRUE;
        }

        public void ToggleOnOff() //F9
        {
            _modules.Emu.EmulationRunning = _modules.Emu.EmulationRunning == Define.TRUE ? Define.FALSE : Define.TRUE;

            if (_modules.Emu.EmulationRunning == Define.TRUE)
            {
                _modules.Emu.ResetPending = (byte)ResetPendingStates.Hard;
            }
            else
            {
                _modules.DirectDraw.SetStatusBarText("");
            }
        }

        public void ToggleInfoBand() //F10
        {
            _modules.DirectDraw.InfoBand = _modules.DirectDraw.InfoBand == Define.TRUE ? Define.FALSE : Define.TRUE;

            _modules.Graphics.InvalidateBorder();
        }

        public void ToggleFullScreen() //F11
        {
            if (_modules.Vcc.RunState == (byte)EmuRunStates.Running)
            {
                _modules.Vcc.RunState = (byte)EmuRunStates.ReqWait;
                _modules.Emu.FullScreen = _modules.Emu.FullScreen == Define.TRUE ? Define.FALSE : Define.TRUE;
            }
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
        public void ProcessMessage(HWND hWnd, uint message, long wParam, long lParam)
        {
            switch (message)
            {
                //TODO: This is Events.EmuExit()
                case Define.WM_CLOSE:
                    _modules.Vcc.BinaryRunning = false;
                    break;

                case Define.WM_COMMAND:
                    ProcessCommandMessage(hWnd, wParam);
                    break;

                case Define.WM_CREATE:
                    CreateMainMenu(hWnd);
                    break;

                case Define.WM_KEYDOWN:
                    ProcessKeyDownMessage(wParam, lParam);
                    break;

                case Define.WM_KEYUP:
                    KeyUp(wParam, lParam);
                    break;

                case Define.WM_KILLFOCUS:
                    SendSavedKeyEvents();
                    break;

                case Define.WM_LBUTTONDOWN:
                    _modules.Joystick.SetButtonStatus(0, 1);
                    break;

                case Define.WM_LBUTTONUP:
                    _modules.Joystick.SetButtonStatus(0, 0);
                    break;

                case Define.WM_MOUSEMOVE:
                    MouseMove(lParam);
                    break;

                case Define.WM_RBUTTONDOWN:
                    _modules.Joystick.SetButtonStatus(1, 1);
                    break;

                case Define.WM_RBUTTONUP:
                    _modules.Joystick.SetButtonStatus(1, 0);
                    break;

                case Define.WM_SYSCOMMAND:
                    ProcessSysCommandMessage(hWnd, wParam);
                    break;

                case Define.WM_SYSKEYDOWN:
                    ProcessSysKeyDownMessage(wParam, lParam);
                    break;

                case Define.WM_SYSKEYUP:
                    KeyUp(wParam, lParam);
                    break;
            }
        }

        public void ProcessSysCommandMessage(HWND hWnd, long wParam)
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
                ProcessCommandMessage(hWnd, wParam);
            }
        }

        public void ProcessCommandMessage(HWND hWnd, long wParam)
        {
            // Force all keys up to prevent key repeats
            SendSavedKeyEvents();

            int wmId = (int)(wParam & 0xFFFF); //LOWORD(wParam);
            //int wmEvent = (int)((wParam >> 16) & 0xFFFF); //HIWORD(wParam);

            // Parse the menu selections:
            // Added for Dynamic menu system
            if (wmId >= Define.ID_DYNAMENU_START && wmId <= Define.ID_DYNAMENU_END)
            {
                Task.Run(() =>
                {
                    //Calls to the loaded DLL so it can do the right thing
                    int result = _modules.MenuCallbacks.DynamicMenuActivated(_modules.Emu.EmulationRunning, wmId); 

                    if (result != 0)
                    {
                        _modules.Emu.ResetPending = Define.RESET_HARD;
                    }
                });
            }
        }

        public void KeyUp(long wParam, long lParam)
        {
            // send emulator key up event to the emulator
            // TODO: Key up checks whether the emulation is running, this does not

            byte oemScan = (byte)((lParam & 0x00FF0000) >> 16);

            _modules.Keyboard.KeyboardHandleKey((byte)wParam, oemScan, KeyStates.kEventKeyUp);
        }

        public void MouseMove(long lParam)
        {
            unsafe
            {
                if (_modules.Emu.EmulationRunning != 0)
                {
                    uint x = (uint)(lParam & 0xFFFF); // LOWORD(lParam);
                    uint y = (uint)((lParam >> 16) & 0xFFFF); // HIWORD(lParam);

                    RECT clientSize;
                    _modules.GDI.GDIGetClientRect(_modules.Emu.WindowHandle, &clientSize);

                    x /= (uint)((clientSize.right - clientSize.left) >> 6);
                    y /= (uint)(((clientSize.bottom - clientSize.top) - 20) >> 6);

                    _modules.Joystick.SetJoystick((ushort)x, (ushort)y);
                }
            }
        }

        public void ProcessSysKeyDownMessage(long wParam, long lParam)
        {
            // Ignore repeated system keys
            if ((lParam >> 30) != 0)
            {
                KeyDown(wParam, lParam);
            }
        }

        public void ProcessKeyDownMessage(long wParam, long lParam)
        {
            // get key scan code for emulator control keys
            byte oemScan = (byte)((lParam & 0x00FF0000) >> 16); // just get the scan code

            switch (oemScan)
            {
                case Define.DIK_F11:
                    ToggleFullScreenState();
                    break;

                default:
                    KeyDown(wParam, lParam);
                    break;
            }
        }

        public void ToggleFullScreenState()
        {
            if (_modules.Vcc.RunState == 0)
            {
                _modules.Vcc.RunState = 1;
                _modules.Emu.FullScreen = _modules.Emu.FullScreen != 0 ? Define.FALSE : Define.TRUE;
            }
        }

        public void KeyDown(long wParam, long lParam)
        {
            byte oemScan = (byte)((lParam & 0x00FF0000) >> 16); // just get the scan code

            // send other keystrokes to the emulator if it is active
            if (_modules.Emu.EmulationRunning != 0)
            {
                _modules.Keyboard.KeyboardHandleKey((byte)wParam, oemScan, KeyStates.kEventKeyDown);

                // Save key down in case focus is lost
                SaveLastTwoKeyDownEvents((byte)wParam, oemScan);
            }
        }

        //--------------------------------------------------------------------------
        // When the main window is about to lose keyboard focus there are one
        // or two keys down in the emulation that must be raised.  These routines
        // track the last two key down events so they can be raised when needed.
        //--------------------------------------------------------------------------

        // Save last two key down events
        public void SaveLastTwoKeyDownEvents(byte kbChar, byte oemScan)
        {
            // Ignore zero scan code
            if (oemScan == 0)
            {
                return;
            }

            // Remember it
            KeySaveToggle = !KeySaveToggle;

            if (KeySaveToggle)
            {
                KB_save1 = kbChar;
                SC_save1 = oemScan;
            }
            else
            {
                KB_save2 = kbChar;
                SC_save2 = oemScan;
            }
        }

        // Force keys up if main widow keyboard focus is lost.  Otherwise
        // down keys will cause issues with OS-9 on return
        // Send key up events to keyboard handler for saved keys
        public void SendSavedKeyEvents()
        {
            if (SC_save1 != 0)
            {
                _modules.Keyboard.KeyboardHandleKey(KB_save1, SC_save1, KeyStates.kEventKeyUp);
            }

            if (SC_save2 != 0)
            {
                _modules.Keyboard.KeyboardHandleKey(KB_save2, SC_save2, KeyStates.kEventKeyUp);
            }

            SC_save1 = 0;
            SC_save2 = 0;
        }

        public void CreateMainMenu(HWND hWnd)
        {
            if (_modules.Emu.FullScreen == 0)
            {
                _modules.GDI.CreateMainMenuWindowed(hWnd, _modules.Emu.Resources);
            }
            else
            {
                _modules.GDI.CreateMainMenuFullScreen(hWnd);
            }
        }

    }
}
