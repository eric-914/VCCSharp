using System.Windows;
using VCCSharp.IoC;

namespace VCCSharp.Main;

public partial class MainWindow : IMainWindow
{
    public Window Window => this;
    public FrameworkElement View => MainView;

    public MainWindowViewModel ViewModel
    {
        get => (MainWindowViewModel)DataContext;
        set => DataContext = value;
    }
        
    public MainWindow()
    {
        InitializeComponent();

        Factory.Instance.Get<IVccMainWindow>().Run(this);
    }
}