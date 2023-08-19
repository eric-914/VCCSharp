using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Modules;

namespace VCCSharp
{
    public class Actions
    {
        private readonly IModules _modules;
        private readonly IConfigurationManager _configurationManager;
        private readonly IEvents _events;
        private readonly IOptions _options;

        public Actions(IModules modules, IConfigurationManager configurationManager, IEvents events, IOptions options)
        {
            _modules = modules;
            _configurationManager = configurationManager;
            _events = events;
            _options = options;
        }

        public void ApplicationExit()
        {
            _events.EmuExit();
            Application.Current.MainWindow?.Close();
        }

        public void AboutVcc()
        {
            new AboutWindow().ShowDialog();
        }

        public void SaveConfiguration()
        {
            _configurationManager.SaveAs();
        }

        public void LoadConfiguration()
        {
            _configurationManager.LoadFrom();
            _modules.Emu.ResetPending = ResetPendingStates.Hard;
        }

        public void HardReset() //F9
        {
            _events.ToggleOnOff();
        }

        public void SoftReset() //F5
        {
            _events.EmuReset(ResetPendingStates.Soft);
        }

        public void CopyText()
        {
            _modules.Clipboard.CopyText();
        }

        public void PasteText()
        {
            _modules.Clipboard.PasteClipboard();
        }

        public void PasteBasicCodeMerge()
        {
            _modules.Clipboard.PasteBasic();
        }

        public void PasteBasicCodeNew()
        {
            _modules.Clipboard.PasteBasicWithNew();
        }

        public void FlipArtifactColors() //F7
        {
            _modules.Graphics.FlipArtifacts();
        }

        public void TapeRecorder()
        {
            _options.TapePlayer.ShowDialog(_configurationManager);
        }

        public void BitBanger()
        {
            _options.BitBanger.ShowDialog();
        }

        public void OpenConfiguration()
        {
            _options.Configuration.ShowDialog();
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
            _events.EmuRun();
        }

        public void SlowDown() //F3
        {
            _events.SlowDown();
        }

        public void SpeedUp() //F4
        {
            _events.SpeedUp();
        }

        public void ToggleMonitorType() //F6
        {
            _events.ToggleMonitorType();
        }

        public void ToggleThrottle() //F8
        {
            _events.ToggleThrottle();
        }

        public void ToggleInfoBand() //F10
        {
            _events.ToggleInfoBand();
        }

        public void ToggleFullScreen() //F11
        {
            _events.ToggleFullScreen();
        }

        public void TestIt() //Ctrl-F12 -- Might keep this around
        {
            TestItAction();
        }

        //--Just a way to be able to trigger some elsewhere defined action by hitting Ctrl-F12
        public static Action TestItAction { get; set; } = () => { System.Diagnostics.Debug.WriteLine("Ctrl-F12"); };

        public void Cancel() //ESC
        {
            _events.Cancel();
        }

        public void OpenBitBanger()
        {
            throw new NotImplementedException();
        }

        public void CloseBitBanger()
        {
            throw new NotImplementedException();
        }
    }
}
