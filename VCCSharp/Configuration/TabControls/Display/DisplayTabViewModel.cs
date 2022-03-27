using VCCSharp.Enums;
using VCCSharp.Main.ViewModels;
using VCCSharp.Models.Configuration;

namespace VCCSharp.Configuration.TabControls.Display;

public class DisplayTabViewModel : NotifyViewModel
{
    private readonly CPU _model = new();
    private readonly Video _video = new();
    private readonly Window _window = new();

    public DisplayTabViewModel() { }

    public DisplayTabViewModel(CPU model, Video video, Window window)
    {
        _model = model;
        _video = video;
        _window = window;
    }

    public int FrameSkip
    {
        get => _model.FrameSkip;
        set
        {
            if (_model.FrameSkip == (byte)value) return;

            _model.FrameSkip = (byte)value;
            OnPropertyChanged();
        }
    }

    public MonitorTypes? MonitorType
    {
        get => _video.Monitor.Value;
        set
        {
            if (!value.HasValue || _video.Monitor.Value == value.Value) return;

            _video.Monitor.Value = value.Value;
            OnPropertyChanged();
        }
    }

    public PaletteTypes? PaletteType
    {
        get => _video.Palette.Value;
        set
        {
            if (!value.HasValue || _video.Palette.Value == value.Value) return;

            _video.Palette.Value = value.Value;
            OnPropertyChanged();
        }
    }

    public bool ScanLines
    {
        get => _video.ScanLines;
        set
        {
            if (value == _video.ScanLines) return;

            _video.ScanLines = value;
            OnPropertyChanged();
        }
    }

    public bool RememberSize
    {
        get => _window.RememberSize;
        set
        {
            if (value == _window.RememberSize) return;

            _window.RememberSize = value;
            OnPropertyChanged();
        }
    }

    public bool ForceAspect
    {
        get => _video.ForceAspect;
        set
        {
            if (value == _video.ForceAspect) return;

            _video.ForceAspect = value;
            OnPropertyChanged();
        }
    }

    public bool SpeedThrottle
    {
        get => _model.ThrottleSpeed;
        set
        {
            if (value == _model.ThrottleSpeed) return;

            _model.ThrottleSpeed = value;
            OnPropertyChanged();
        }
    }
}
