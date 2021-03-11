using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;

namespace VCCSharp
{
    public class Actions
    {
        private readonly IModules _modules;

        public Actions(IModules modules)
        {
            _modules = modules;
        }

        public void ApplicationExit()
        {
            _modules.Events.EmuExit();
            Application.Current.MainWindow?.Close();
        }

        public void AboutVcc()
        {
            new AboutWindow().ShowDialog();
        }

        public void SaveConfiguration()
        {
            _modules.Events.SaveConfig();
        }

        public void LoadConfiguration()
        {
            _modules.Events.LoadIniFile();
        }

        public void HardReset() //F9
        {
            //_modules.Events.EmuReset(ResetPendingStates.Hard);
            _modules.Events.ToggleOnOff();
        }

        public void SoftReset() //F5
        {
            _modules.Events.EmuReset(ResetPendingStates.Soft);
        }

        public void CopyText()
        {
            _modules.Clipboard.CopyText();
        }

        public void PasteText()
        {
            _modules.Clipboard.PasteText();
        }

        public void PasteBasicCodeMerge()
        {
            _modules.Clipboard.PasteBASIC();
        }

        public void PasteBasicCodeNew()
        {
            _modules.Clipboard.PasteBASICWithNew();
        }

        public void FlipArtifactColors() //F7
        {
            _modules.Graphics.FlipArtifacts();
        }

        public void OpenConfiguration()
        {
            _modules.Events.ShowConfiguration();
        }

        public void LoadCartridge()
        {
            MessageBox.Show("Load Cartridge");
        }

        public void EjectCartridge()
        {
            MessageBox.Show("Eject Cartridge");
        }

        public void Run()
        {
            _modules.Events.EmuRun();
        }

        public void SlowDown() //F3
        {
            _modules.Events.SlowDown();
        }

        public void SpeedUp() //F4
        {
            _modules.Events.SpeedUp();
        }

        public void ToggleMonitorType() //F6
        {
            _modules.Events.ToggleMonitorType();
        }

        public void ToggleThrottle() //F8
        {
            _modules.Events.ToggleThrottle();
        }

        public void ToggleInfoBand() //F10
        {
            _modules.Events.ToggleInfoBand();
        }

        public void ToggleFullScreen() //F11
        {
            _modules.Events.ToggleFullScreen();
        }
    }
}
