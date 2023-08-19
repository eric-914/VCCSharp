using System.IO;
using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Models.Configuration;
using VCCSharp.Modules;

namespace VCCSharp.TapePlayer;

public class TapePlayerManager : ITapePlayer
{
    private readonly IModules _modules;
    private readonly TapePlayerViewModel _viewModel = new();
    private TapePlayerWindow? _view;

    private IConfiguration Model => _modules.Configuration;

    public TapePlayerManager(IModules modules)
    {
        _modules = modules;

        _viewModel.PropertyChanged += (_, args) =>
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

    public void ShowDialog(IConfigurationManager state)
    {
        _viewModel.ConfigurationManager = state;

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

        Model.CassetteRecorder.TapeMode = TapeModes.Record;

        SetTapeMode(TapeModes.Record);
    }

    public void Play()
    {
        _viewModel.Mode = TapeModes.Play;

        Model.CassetteRecorder.TapeMode = TapeModes.Play;

        SetTapeMode(TapeModes.Play);
    }

    public void Stop()
    {
        _viewModel.Mode = TapeModes.Stop;

        Model.CassetteRecorder.TapeMode = TapeModes.Stop;

        SetTapeMode(TapeModes.Stop);
    }

    public void Eject()
    {
        _viewModel.Mode = TapeModes.Eject;

        Model.CassetteRecorder.TapeMode = TapeModes.Eject;

        SetTapeMode(TapeModes.Eject);
    }

    public void Rewind()
    {
        _viewModel.Counter = 0;

        Model.CassetteRecorder.TapeCounter = 0;

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
        var configuration = _modules.Configuration;

        string? szFileName = Model.CassetteRecorder.TapeFileName;
        string appPath = configuration.FilePaths.Cassette ?? "C:\\";

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

            var filepath = Path.GetFileName(file);

            if (file == null || filepath == null)
            {
                throw new Exception($"Invalid file/path: {file}");
            }

            _viewModel.FilePath = filepath;

            _modules.Cassette.TapeFileName = file;

            if (_modules.Cassette.MountTape(file) == 0)
            {
                MessageBox.Show("Can't open file", "Error");
            }

            var path = Path.GetDirectoryName(file);
            if (path == null)
            {
                MessageBox.Show("Can't open file", "Error");
            }

            configuration.FilePaths.Cassette = path;

            Model.CassetteRecorder.TapeCounter = 0;

            SetTapeCounter(0);

            return 1;
        }

        return 0;
    }

    private void SetTapeCounter(int counter)
    {
        _viewModel.Counter = counter;
    }
}