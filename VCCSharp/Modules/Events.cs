using System;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using HWND = System.IntPtr;
using HINSTANCE = System.IntPtr;
using static System.IntPtr;

namespace VCCSharp.Modules
{
    public interface IEvents
    {
        void EmuRun();
        void EmuReset(ResetPendingStates state);
        void EmuExit();
        void ShowConfiguration();
        void ToggleOnOff();
        void SlowDown();
        void SpeedUp();
        void ToggleMonitorType();
        void ToggleThrottle();
        void ToggleFullScreen();
        void ToggleInfoBand();

        void ProcessMessage(HWND hWnd, uint message, IntPtr wParam, IntPtr lParam);
    }

    public class Events : IEvents
    {
        private readonly IModules _modules;
        private readonly IUser32 _user32;

        public Events(IModules modules, IUser32 user32)
        {
            _modules = modules;
            _user32 = user32;
        }

        public void EmuRun()
        {
            _modules.Emu.SetEmuRunning(true);

            _modules.Graphics.InvalidateBorder();
        }

        public void EmuReset(ResetPendingStates state)
        {
            unsafe
            {
                EmuState* emuState = _modules.Emu.GetEmuState();

                if (emuState->EmulationRunning == Define.TRUE)
                {
                    emuState->ResetPending = (byte)state;
                }
            }
        }

        public void EmuExit()
        {
            unsafe
            {
                _modules.Vcc.GetVccState()->BinaryRunning = Define.FALSE;
            }
        }

        public void SlowDown() //F3
        {
            unsafe
            {
                _modules.Config.DecreaseOverclockSpeed(_modules.Emu.GetEmuState());
            }
        }

        public void SpeedUp() //F4
        {
            unsafe
            {
                _modules.Config.IncreaseOverclockSpeed(_modules.Emu.GetEmuState());
            }
        }

        public void ToggleMonitorType() //F6
        {
            unsafe
            {
                GraphicsState* graphicsState = _modules.Graphics.GetGraphicsState();

                byte monType = graphicsState->MonType == Define.FALSE ? Define.TRUE : Define.FALSE;

                _modules.Graphics.SetMonitorType(monType);
            }
        }

        public void ToggleThrottle() //F8
        {
            unsafe
            {
                VccState* vccState = _modules.Vcc.GetVccState();

                vccState->Throttle = vccState->Throttle == Define.TRUE ? Define.FALSE : Define.TRUE;
            }
        }

        public void ToggleOnOff() //F9
        {
            unsafe
            {
                EmuState* emuState = _modules.Emu.GetEmuState();

                emuState->EmulationRunning = emuState->EmulationRunning == Define.TRUE ? Define.FALSE : Define.TRUE;

                if (emuState->EmulationRunning == Define.TRUE)
                {
                    emuState->ResetPending = (byte)ResetPendingStates.Hard;
                }
                else
                {
                    _modules.DirectDraw.SetStatusBarText("", emuState);
                }

            }
        }

        public void ToggleInfoBand() //F10
        {
            unsafe
            {
                DirectDrawState* directDrawState = _modules.DirectDraw.GetDirectDrawState();

                directDrawState->InfoBand = directDrawState->InfoBand == Define.TRUE ? Define.FALSE : Define.TRUE;
            }

            _modules.Graphics.InvalidateBorder();
        }

        public void ToggleFullScreen() //F11
        {
            unsafe
            {
                VccState* vccState = _modules.Vcc.GetVccState();
                EmuState* emuState = _modules.Emu.GetEmuState();

                if (vccState->RunState == (byte)EmuRunStates.Running)
                {
                    vccState->RunState = (byte)EmuRunStates.ReqWait;
                    emuState->FullScreen = emuState->FullScreen == Define.TRUE ? Define.FALSE : Define.TRUE;
                }
            }
        }

        public void ShowConfiguration()
        {
            unsafe
            {
                EmuState* emuState = _modules.Emu.GetEmuState();

                // open config dialog if not already open
                // opens modeless so you can control the cassette
                // while emulator is still running (assumed)
                if (emuState->ConfigDialog == Zero)
                {
                    emuState->ConfigDialog = CreateConfigurationDialog(emuState->Resources, emuState->WindowHandle);

                    // open modeless
                    _user32.ShowWindow(emuState->ConfigDialog, (int)ShowWindowCommands.Normal);
                }

            }
        }

        public HWND CreateConfigurationDialog(HINSTANCE resources, HWND windowHandle)
        {
            return Library.Events.CreateConfigurationDialog(resources, windowHandle);
        }

        public void ProcessMessage(HWND hWnd, uint message, IntPtr wParam, IntPtr lParam)
        {
            Library.Events.ProcessMessage(hWnd, message, wParam, lParam);
        }
    }
}
