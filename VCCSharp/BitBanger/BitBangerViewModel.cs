using System;
using VCCSharp.Main.ViewModels;
using VCCSharp.Modules;

namespace VCCSharp.BitBanger;

public class BitBangerViewModel : NotifyViewModel
{
    private const string NoFile = "No Capture File";

    //TODO: Remove STATIC once safe
    private static IConfigurationModule? _config;

    private string _serialCaptureFile = NoFile;

    public IConfigurationModule? Config
    {
        get => _config;
        set
        {
            if (_config != null) return;

            _config = value;
        }
    }

    public string? SerialCaptureFile
    {
        get
        {
            if (Config == null) return string.Empty;

            string? file = Config.SerialCaptureFile;

            _serialCaptureFile = string.IsNullOrEmpty(file) ? NoFile : file;

            return _serialCaptureFile;
        }
        set
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            if (value == _serialCaptureFile) return;

            _serialCaptureFile = value;

            if (Config == null)
            {
                throw new Exception("Configuration is missing");
            }
            Config.SerialCaptureFile = value;

            OnPropertyChanged();
        }
    }

    public bool AddLineFeed
    {
        get => Config is { TextMode: true };
        set
        {
            if (Config == null)
            {
                throw new Exception("Configuration is missing");
            }
            if (value == Config.TextMode) return;

            Config.TextMode = value;
            OnPropertyChanged();
        }
    }

    public bool Print
    {
        get => Config is { PrintMonitorWindow: true };
        set
        {
            if (Config == null)
            {
                throw new Exception("Configuration is missing");
            }
            if (value == Config.PrintMonitorWindow) return;

            Config.PrintMonitorWindow = value;
            OnPropertyChanged();
        }
    }
}