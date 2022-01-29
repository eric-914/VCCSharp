using System.Windows;
using VCCSharp.IoC;
using VCCSharp.Menu;

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

            var commands = _factory.Get<MainWindowCommands>();

            ViewModel = _factory.Get<IViewModelFactory>().CreateMainWindowViewModel(commands.MenuItems);

            _factory.Get<IModules>().Emu.TestIt = () =>
            {
                ViewModel.WindowHeight += 20;
                ViewModel.WindowWidth += 30;
            };

            _factory.Get<IWindowEvents>().Bind(this, commands);

            _factory.Get<IVccThread>().Run(this);
        }

        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ViewModel.SurfaceSize = Surface.RenderSize;
        }
    }
}
