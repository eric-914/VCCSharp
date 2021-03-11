using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IEvents
    {
        void EmuRun();
        void EmuReset(ResetPendingStates state);
        void EmuExit();
        void LoadIniFile();
        void SaveConfig();
        void ShowConfiguration();
        void ToggleOnOff();
        void SlowDown();
        void SpeedUp();
        void ToggleMonitorType();
        void ToggleThrottle();
        void ToggleFullScreen();
        void ToggleInfoBand();
    }

    public class Events : IEvents
    {
        private readonly IModules _modules;

        public Events(IModules modules)
        {
            _modules = modules;
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

        public void LoadIniFile()
        {
            Library.Events.LoadIniFile();
        }

        public void SaveConfig()
        {
            Library.Events.SaveConfig();
        }

        public void ShowConfiguration()
        {
            Library.Events.ShowConfiguration();
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
            Library.Events.ToggleMonitorType();
        }

        public void ToggleThrottle() //F8
        {
            unsafe
            {
                VccState* vccState = _modules.Vcc.GetVccState();

                vccState->Throttle = vccState->Throttle == Define.TRUE ? Define.FALSE : Define.TRUE;
            }
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

        public void ToggleInfoBand() //F10
        {
            unsafe
            {
                DirectDrawState* directDrawState = _modules.DirectDraw.GetDirectDrawState();

                directDrawState->InfoBand = directDrawState->InfoBand == Define.TRUE ? Define.FALSE : Define.TRUE;
            }

            _modules.Graphics.InvalidateBorder();
        }
    }
}
