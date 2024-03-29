﻿using VCCSharp.Configuration.Models;
using VCCSharp.Configuration.Options;
using VCCSharp.Shared.ViewModels;

namespace VCCSharp.Configuration.TabControls.Display;

public interface IDisplayTabViewModel
{
    bool ForceAspect { get; set; }
    int FrameSkip { get; set; }
    MonitorTypes? MonitorType { get; set; }
    PaletteTypes? PaletteType { get; set; }
    bool RememberSize { get; set; }
    bool ScanLines { get; set; }
    bool SpeedThrottle { get; set; }
}

public abstract class DisplayTabViewModelBase : NotifyViewModel, IDisplayTabViewModel
{
    private readonly ICPUConfiguration _cpu = ConfigurationFactory.CPUConfiguration();
    private readonly IVideoConfiguration _video = ConfigurationFactory.VideoConfiguration();
    private readonly IWindowConfiguration _window = ConfigurationFactory.WindowConfiguration();

    protected DisplayTabViewModelBase(ICPUConfiguration cpu, IVideoConfiguration video, IWindowConfiguration window)
    {
        _cpu = cpu;
        _video = video;
        _window = window;
    }

    public int FrameSkip
    {
        get => _cpu.FrameSkip;
        set
        {
            if (_cpu.FrameSkip == (byte)value) return;

            _cpu.FrameSkip = (byte)value;
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
        get => _cpu.ThrottleSpeed;
        set
        {
            if (value == _cpu.ThrottleSpeed) return;

            _cpu.ThrottleSpeed = value;
            OnPropertyChanged();
        }
    }
}

public class DisplayTabViewModelStub : DisplayTabViewModelBase
{
    public DisplayTabViewModelStub() : base(ConfigurationFactory.CPUConfiguration(), ConfigurationFactory.VideoConfiguration(), ConfigurationFactory.WindowConfiguration()) { }
}

public class DisplayTabViewModel : DisplayTabViewModelBase
{
    public DisplayTabViewModel(ICPUConfiguration cpu, IVideoConfiguration video, IWindowConfiguration window) : base(cpu, video, window)
    {
    }
}
