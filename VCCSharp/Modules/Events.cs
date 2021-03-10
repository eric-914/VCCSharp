using VCCSharp.Enums;
using VCCSharp.Libraries;

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
        public void EmuRun()
        {
            Library.Events.EmuRun();
        }

        public void EmuReset(ResetPendingStates state)
        {
            Library.Events.EmuReset((byte)state);
        }

        public void EmuExit()
        {
            Library.Events.EmuExit();
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
