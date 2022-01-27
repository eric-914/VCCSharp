using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using VCCSharp.Annotations;

namespace VCCSharp.Main
{
    public interface IStatus : INotifyPropertyChanged
    {
        int FrameSkip { get; set; }
        string CpuName { get; set; }
        double Mhz { get; set; }
        string Status { get; set; }
        float Fps { get; set; }
        Size WindowSize { get; set; }
        Size SurfaceSize { get; set; }
    }

    public class StatusViewModel : IStatus
    {
        private int _frameSkip = 5;
        private string _cpuName;
        private double _mhz = 1.234;
        private string _status;
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

        public string CpuName
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

        public string Status
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

        private Size _windowSize;
        public Size WindowSize
        {
            get => _windowSize;
            set
            {
                if ((int)_windowSize.Width == (int)value.Width && (int)_windowSize.Height == (int)value.Height) return;

                _windowSize = value;
                OnPropertyChanged();
            }
        }

        private Size _surfaceSize;
        public Size SurfaceSize
        {
            get => _surfaceSize;
            set
            {
                if ((int)_surfaceSize.Width == (int)value.Width && (int)_surfaceSize.Height == (int)value.Height) return;

                _surfaceSize = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
