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
            unsafe
            {
                _modules.Emu.SetEmuRunning(true);

                _modules.Graphics.InvalidateBorder();
            }
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

                if (state == ResetPendingStates.Hard)
                {
                    _modules.Emu.SetEmuRunning(emuState->EmulationRunning != Define.TRUE);
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
    }
}
