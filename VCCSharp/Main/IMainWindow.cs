using System.Windows;

namespace VCCSharp.Main;

public interface IMainWindow
{
    Window Window { get; }
    FrameworkElement View { get; }

    MainWindowViewModel ViewModel { get; set; }
}
