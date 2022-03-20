using System;
using System.ComponentModel;
using VCCSharp.Main.ViewModels;

namespace VCCSharp.Main;

public interface IStatus : INotifyPropertyChanged
{
    int FrameSkip { get; set; }
    string? CpuName { get; set; }
    double Mhz { get; set; }
    string? Status { get; set; }
    float Fps { get; set; }
}

public class StatusViewModel : NotifyViewModel, IStatus
{
    private int _frameSkip = 5;
    private string? _cpuName;
    private double _mhz = 1.234;
    private string? _status;
    private float _fps = 60.01f;

    public int FrameSkip
    {
        get => _frameSkip;
        set
        {
            if (_frameSkip == value) return;

            _frameSkip = value;
            OnPropertyChanged();
        }
    }

    public string? CpuName
    {
        get => _cpuName;
        set
        {
            if (_cpuName == value) return;

            _cpuName = value;
            OnPropertyChanged();
        }
    }

    public double Mhz
    {
        get => _mhz;
        set
        {
            if (Math.Abs(_mhz - value) < .001) return;

            _mhz = value;
            OnPropertyChanged();
        }
    }

    public string? Status
    {
        get => _status;
        set
        {
            if (_status == value) return;

            _status = value;
            OnPropertyChanged();
        }
    }

    public float Fps
    {
        get => _fps;
        set
        {
            if (Math.Abs(_fps - value) < .001) return;

            _fps = value;
            OnPropertyChanged();
        }
    }
}