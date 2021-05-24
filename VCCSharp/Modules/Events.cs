using System;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using HWND = System.IntPtr;

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
        void ProcessMessage(HWND hWnd, uint message, IntPtr wParam, IntPtr lParam);
    }

    public class Events : IEvents
    {
        private readonly IModules _modules;
        private IGraphics _graphics => _modules.Graphics;

        public Events(IModules modules)
        {
            _modules = modules;
        }

        public void EmuRun()
        {
            _modules.Emu.SetEmuRunning(true);

            _graphics.InvalidateBorder();
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
            byte monType = _graphics.MonType == Define.FALSE ? Define.TRUE : Define.FALSE;

            _modules.Graphics.SetMonitorType(monType);
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

        public void ProcessMessage(HWND hWnd, uint message, IntPtr wParam, IntPtr lParam)
        {
            switch (message)
            {
                case Define.WM_CLOSE:
                    //GetVccState()->BinaryRunning = false;     
                    break;  //TODO: This is Events.EmuExit()

                case Define.WM_COMMAND:
                    //ProcessCommandMessage(hWnd, wParam);
                    break;
                case Define.WM_CREATE: 
                    //CreateMainMenu(hWnd); 
                    break;

                case Define.WM_KEYDOWN: 
                    //ProcessKeyDownMessage(wParam, lParam); 
                    break;

                case Define.WM_KEYUP: 
                    //KeyUp(wParam, lParam); 
                    break;

                case Define.WM_KILLFOCUS: 
                    //SendSavedKeyEvents(); 
                    break;

                case Define.WM_LBUTTONDOWN: 
                    //SetButtonStatus(0, 1); 
                    break;

                case Define.WM_LBUTTONUP: 
                    //SetButtonStatus(0, 0); 
                    break;

                case Define.WM_MOUSEMOVE: 
                    //MouseMove(lParam); 
                    break;

                case Define.WM_RBUTTONDOWN: 
                    //SetButtonStatus(1, 1); 
                    break;

                case Define.WM_RBUTTONUP: 
                    //SetButtonStatus(1, 0); 
                    break;

                case Define.WM_SYSCOMMAND: 
                    //ProcessSysCommandMessage(hWnd, wParam); 
                    break;

                case Define.WM_SYSKEYDOWN: 
                    //ProcessSysKeyDownMessage(wParam, lParam); 
                    break;

                case Define.WM_SYSKEYUP: 
                    //KeyUp(wParam, lParam); 
                    break;
            }

            Library.Events.ProcessMessage(hWnd, message, wParam, lParam);
        }
    }
}
