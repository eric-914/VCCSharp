using System.Windows;
using VCCSharp.Main.ViewModels;
using VCCSharp.Menu;
using VCCSharp.Properties;

namespace VCCSharp.Main;

public class MainWindowViewModel : NotifyViewModel
{
    public string Title { get; set; } = Resources.ResourceManager.GetString("AppTitle") ?? "VCCSharp";
    public IMainMenu MenuItems { get; set; } = default!;
    public IStatus Status { get; set; } = default!;

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
}
