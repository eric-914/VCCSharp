using System;
using System.IO;
using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Models;
using VCCSharp.Modules;

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

        public void ShowDialog(IConfig state)
        {
            _viewModel.Config = _modules.Config;

            _view ??= new TapePlayerWindow(_viewModel, this);

            _view.Show();
        }

        public void Browse()
        {
            LoadTape();
        }

        public void Record()
        {
            _viewModel.Mode = TapeModes.Record;

            _modules.Config.TapeMode = TapeModes.Record;

            SetTapeMode(TapeModes.Record);
        }

        public void Play()
        {
            _viewModel.Mode = TapeModes.Play;

            _modules.Config.TapeMode = TapeModes.Play;

            SetTapeMode(TapeModes.Play);
        }

        public void Stop()
        {
            _viewModel.Mode = TapeModes.Stop;

            _modules.Config.TapeMode = TapeModes.Stop;

            SetTapeMode(TapeModes.Stop);
        }

        public void Eject()
        {
            _viewModel.Mode = TapeModes.Eject;

            _modules.Config.TapeMode = TapeModes.Eject;

            SetTapeMode(TapeModes.Eject);
        }

        public void Rewind()
        {
            _viewModel.Counter = 0;

            _modules.Config.TapeCounter = 0;

            SetTapeCounter(0);
        }

        public void SetTapeMode(TapeModes mode)
        {
            _modules.Cassette.TapeMode = mode;

            switch (_modules.Cassette.TapeMode)
            {
                case TapeModes.Stop:
                    break;

                case TapeModes.Play:
                    if (_modules.Cassette.TapeHandle == IntPtr.Zero)
                    {
                        _modules.Cassette.TapeMode = LoadTape() == 0 ? TapeModes.Stop : TapeModes.Play;
                    }

                    if (_modules.Cassette.MotorState != 0)
                    {
                        _modules.Cassette.Motor(1);
                    }

                    break;

                case TapeModes.Record:
                    if (_modules.Cassette.TapeHandle == IntPtr.Zero)
                    {
                        _modules.Cassette.TapeMode = LoadTape() == 0 ? TapeModes.Stop : TapeModes.Record;
                    }
                    break;

                case TapeModes.Eject:
                    _modules.Cassette.CloseTapeFile();
                    _modules.Cassette.TapeFileName = "EMPTY";

                    break;
            }

            SetTapeCounter((int)_modules.Cassette.TapeOffset);

            //_viewModel.FilePath = Path.GetFileName(Converter.ToString(instance->TapeFileName));
        }

        public int LoadTape()
        {
            var configModel = _modules.Config.Model;

            string szFileName = _modules.Config.TapeFileName;
            string appPath = configModel.FilePaths.Cassette ?? "C:\\";

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

                configModel.FilePaths.Cassette = Path.GetDirectoryName(file);

                _modules.Config.TapeCounter = 0;

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
