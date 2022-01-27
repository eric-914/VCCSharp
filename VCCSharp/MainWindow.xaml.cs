using System.Windows;
using VCCSharp.IoC;
using VCCSharp.Menu;

namespace VCCSharp
{
    public class MainWindowViewModel
    {
        public MenuItems MenuItems { get; set; }
        public IStatus Status { get; set; }
    }

    public partial class MainWindow
    {
        private readonly IFactory _factory = Factory.Instance;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = _factory.Get<IViewModelFactory>().CreateMainWindowViewModel(this);
        }

        //--This occurs some time after the menu has a proper render size giving us a valid surface height
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            _factory.Get<IVccThread>().Run(this, (int)Surface.ActualHeight);
        }
    }
}
