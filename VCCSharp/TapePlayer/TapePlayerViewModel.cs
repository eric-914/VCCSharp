using System;
using VCCSharp.Enums;
using VCCSharp.Main.ViewModels;
using VCCSharp.Modules;

namespace VCCSharp.TapePlayer;

public class TapePlayerViewModel : NotifyViewModel
{
    private const string NoFile = "EMPTY";

    //TODO: Remove STATIC once safe
    private static IConfigurationModule? _config;

    private string _filePath = "Sample Browse File Text";
    private TapeModes _mode = TapeModes.Stop;
    private int _counter;

    public IConfigurationModule? Config
    {
        get => _config;
        set
        {
            if (_config != null) return;

            _config = value;
        }
    }

    public string FilePath
    {
        get
        {
            if (Config == null) return string.Empty;

            string? file = Config.TapeFileName;

            _filePath = string.IsNullOrEmpty(file) ? NoFile : file;

            return _filePath;
        }
        set
        {
            if (value == _filePath) return;

            _filePath = value;

            if (Config == null)
            {
                throw new Exception("Configuration is undefined");
            }

            Config.TapeFileName = value;

            OnPropertyChanged();
        }
    }

    public TapeModes Mode
    {
        get => _mode;
        set
        {
            if (value == _mode) return;

            _mode = value;
            OnPropertyChanged();
        }
    }

    public int Counter
    {
        get => _counter;
        set
        {
            if (value == _counter) return;
            _counter = value;

            if (Config == null)
            {
                throw new Exception("Configuration is undefined");
            }

            Config.TapeCounter = value;

            OnPropertyChanged();
        }
    }
}