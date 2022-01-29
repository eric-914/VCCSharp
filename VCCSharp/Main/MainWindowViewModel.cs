using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using VCCSharp.Annotations;
using VCCSharp.Menu;

namespace VCCSharp.Main
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public IMainMenu MenuItems { get; set; }
        public IStatus Status { get; set; }

        private double _windowWidth = 654;
        public double WindowWidth
        {
            get => _windowWidth;
            set
            {
                if ((int)value == (int)_windowWidth) return;

                _windowWidth = value;
                OnPropertyChanged();
            }
        }

        private double _windowHeight = 575;
        public double WindowHeight
        {
            get => _windowHeight;
            set
            {
                if ((int)value == (int)_windowHeight) return;

                _windowHeight = value;
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