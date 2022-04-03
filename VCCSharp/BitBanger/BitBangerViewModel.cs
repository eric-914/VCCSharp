using System;
using VCCSharp.Models.Configuration;
using VCCSharp.Modules;
using VCCSharp.Shared.ViewModels;

namespace VCCSharp.BitBanger;

public class BitBangerViewModel : NotifyViewModel
{
    private const string NoFile = "No Capture File";

    private string _serialCaptureFile = NoFile;

    public SerialPort Model { get; set; } = new();

    public IConfigurationManager ConfigurationManager
    {
        set => Model = value.Model.SerialPort;
    }

    public string? SerialCaptureFile
    {
        get
        {
            string? file = Model.SerialCaptureFile;

            _serialCaptureFile = string.IsNullOrEmpty(file) ? NoFile : file;

            return _serialCaptureFile;
        }
        set
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (value == _serialCaptureFile) return;

            _serialCaptureFile = value;

            Model.SerialCaptureFile = value;

            OnPropertyChanged();
        }
    }

    public bool AddLineFeed
    {
        get => Model is { TextMode: true };
        set
        {
            if (value == Model.TextMode) return;

            Model.TextMode = value;
            OnPropertyChanged();
        }
    }

    public bool Print
    {
        get => Model is { PrintMonitorWindow: true };
        set
        {
            if (value == Model.PrintMonitorWindow) return;

            Model.PrintMonitorWindow = value;
            OnPropertyChanged();
        }
    }
}
