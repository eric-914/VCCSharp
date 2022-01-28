using System.Windows;
using VCCSharp.IoC;

namespace VCCSharp.Main
{
    public partial class MainWindow
    {
        private readonly IFactory _factory = Factory.Instance;

        private MainWindowViewModel ViewModel
        {
            get => DataContext as MainWindowViewModel;
            set => DataContext = value;
        }

        public MainWindow()
        {
            InitializeComponent();

            ViewModel = _factory.Get<IViewModelFactory>().CreateMainWindowViewModel(this);
        }

        //--This occurs some time after the menu has a proper render size giving us a valid surface height
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            _factory.Get<IVccThread>().Run(this);
        }

        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ViewModel.Status.WindowSize = e.NewSize;
            ViewModel.Status.SurfaceSize = Surface.RenderSize;
        }
    }
}
