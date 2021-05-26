using System;
using System.IO;
using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Models;

namespace VCCSharp.TapePlayer
{
    public class TapePlayerManager : ITapePlayer
    {
        private readonly IModules _modules;

        private readonly TapePlayerViewModel _viewModel = new TapePlayerViewModel();
        private TapePlayerWindow _view;

        public TapePlayerManager(IModules modules)
        {
            _modules = modules;

            _viewModel.PropertyChanged += (sender, args) =>
            {
                switch (args.PropertyName)
                {
                    case "FilePath":
                        break;

                    case "Mode":
                        break;

                    case "Counter":
                        break;
                }
            };

            _modules.CoCo.UpdateTapeDialog = SetTapeCounter;
            _modules.Cassette.UpdateTapeDialog = SetTapeCounter;
        }

        public unsafe void ShowDialog(ConfigState* state)
        {
            _viewModel.State = state;

            _view ??= new TapePlayerWindow(_viewModel, this);

            _view.Show();
        }

        public void Browse()
        {
            LoadTape();
        }

        public void Record()
        {
            _viewModel.Mode = TapeModes.REC;

            unsafe
            {
                ConfigState* configState = _modules.Config.GetConfigState();

                configState->TapeMode = Define.REC;
            }

            SetTapeMode(Define.REC);
        }

        public void Play()
        {
            _viewModel.Mode = TapeModes.PLAY;

            unsafe
            {
                ConfigState* configState = _modules.Config.GetConfigState();

                configState->TapeMode = Define.PLAY;
            }

            SetTapeMode(Define.PLAY);
        }

        public void Stop()
        {
            _viewModel.Mode = TapeModes.STOP;

            unsafe
            {
                ConfigState* configState = _modules.Config.GetConfigState();

                configState->TapeMode = Define.STOP;
            }

            SetTapeMode(Define.STOP);
        }

        public void Eject()
        {
            _viewModel.Mode = TapeModes.EJECT;

            unsafe
            {
                ConfigState* configState = _modules.Config.GetConfigState();

                configState->TapeMode = Define.EJECT;
            }

            SetTapeMode(Define.EJECT);
        }

        public void Rewind()
        {
            _viewModel.Counter = 0;

            unsafe
            {
                ConfigState* configState = _modules.Config.GetConfigState();

                configState->TapeCounter = 0;

                SetTapeCounter(0);
            }
        }

        public void SetTapeMode(byte mode)
        {
            _modules.Cassette.TapeMode = mode;

            switch (_modules.Cassette.TapeMode)
            {
                case Define.STOP:
                    break;

                case Define.PLAY:
                    if (_modules.Cassette.TapeHandle == IntPtr.Zero)
                    {
                        _modules.Cassette.TapeMode = LoadTape() == 0 ? Define.STOP : Define.PLAY;
                    }

                    if (_modules.Cassette.MotorState != 0)
                    {
                        _modules.Cassette.Motor(1);
                    }

                    break;

                case Define.REC:
                    if (_modules.Cassette.TapeHandle == IntPtr.Zero)
                    {
                        _modules.Cassette.TapeMode = LoadTape() == 0 ? Define.STOP : Define.REC;
                    }
                    break;

                case Define.EJECT:
                    _modules.Cassette.CloseTapeFile();
                    _modules.Cassette.TapeFileName = "EMPTY";

                    break;
            }

            SetTapeCounter((int)_modules.Cassette.TapeOffset);

            //_viewModel.FilePath = Path.GetFileName(Converter.ToString(instance->TapeFileName));
        }

        public unsafe int LoadTape()
        {
            ConfigState* configState = _modules.Config.GetConfigState();
            ConfigModel* configModel = _modules.Config.GetConfigModel();

            string szFileName = Converter.ToString(configState->TapeFileName);
            string appPath = Converter.ToString(configModel->CassPath) ?? "C:\\";

            var openFileDlg = new Microsoft.Win32.OpenFileDialog
            {
                FileName = szFileName,
                DefaultExt = ".cas",
                Filter = "Cassette Files (*.cas)|*.cas|Wave Files (*.wav)|*.wav",
                InitialDirectory = appPath,
                CheckFileExists = true,
                ShowReadOnly = false,
                Title = "Insert Tape Image"
            };

            if (openFileDlg.ShowDialog() == true)
            {
                var file = openFileDlg.FileName;

                _viewModel.FilePath = Path.GetFileName(file);

                _modules.Cassette.TapeFileName = file;

                if (_modules.Cassette.MountTape(file) == 0)
                {
                    MessageBox.Show("Can't open file", "Error");
                }

                Converter.ToByteArray(Path.GetDirectoryName(file), configModel->CassPath);

                configState->TapeCounter = 0;

                SetTapeCounter(0);

                return 1;
            }

            return 0;
        }

        private void SetTapeCounter(int counter)
        {
            //_modules.Cassette.SetTapeCounter((uint)counter);
            //_modules.Config.UpdateTapeDialog((uint) counter);
            _viewModel.Counter = counter;
        }
    }
}
