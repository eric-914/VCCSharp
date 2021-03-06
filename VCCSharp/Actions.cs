﻿using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Models;

namespace VCCSharp
{
    public class Actions
    {
        private readonly IModules _modules;
        private readonly IOptions _options;

        public Actions(IModules modules, IOptions options)
        {
            _modules = modules;
            _options = options;
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
            _modules.Config.SaveConfig();
        }

        public void LoadConfiguration()
        {
            _modules.Config.LoadIniFile();
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
            _options.TapePlayer.ShowDialog(_modules.Config);
        }

        public void BitBanger()
        {
            _options.BitBanger.ShowDialog();
        }

        public void OpenConfiguration()
        {
            ConfigModel configModel = _modules.Config.ConfigModel;
            JoystickModel leftJoystickModel = _modules.Config.GetLeftJoystick();
            JoystickModel rightJoystickModel = _modules.Config.GetRightJoystick();

            _options.Configuration.ShowDialog(_modules.Config, configModel, leftJoystickModel, rightJoystickModel);
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

        public void OpenBitBanger()
        {
            throw new System.NotImplementedException();
        }

        public void CloseBitBanger()
        {
            throw new System.NotImplementedException();
        }
    }
}
