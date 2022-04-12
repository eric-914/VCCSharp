﻿using Ninject;
using VCCSharp.Enums;
using VCCSharp.Models.Configuration;
using VCCSharp.Shared.ViewModels;

namespace VCCSharp.Configuration.TabControls.Display;

public class DisplayTabViewModel : NotifyViewModel
{
    private readonly ICPUConfiguration _cpu = new CPU();
    private readonly IVideoConfiguration _video = new Video();
    private readonly IWindowConfiguration _window = new Window();

    public DisplayTabViewModel() { }

    [Inject]
    public DisplayTabViewModel(ICPUConfiguration cpu, IVideoConfiguration video, IWindowConfiguration window)
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
