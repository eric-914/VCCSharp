using VCCSharp.Enums;
using VCCSharp.Main.ViewModels;
using VCCSharp.Models.Configuration;
using VCCSharp.Modules;

namespace VCCSharp.TapePlayer;

public class TapePlayerViewModel : NotifyViewModel
{
    private const string NoFile = "EMPTY";

    private string _filePath = "Sample Browse File Text";
    private TapeModes _mode = TapeModes.Stop;
    private int _counter;

    public CassetteRecorder Model { get; set; } = new();

    public IConfigurationManager ConfigurationManager
    {
        set => Model = value.Model.CassetteRecorder;
    }

    public string FilePath
    {
        get
        {
            string? file = Model.TapeFileName;

            _filePath = string.IsNullOrEmpty(file) ? NoFile : file;

            return _filePath;
        }
        set
        {
            if (value == _filePath) return;

            _filePath = value;

            Model.TapeFileName = value;

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

            Model.TapeCounter = value;

            OnPropertyChanged();
        }
    }
}
