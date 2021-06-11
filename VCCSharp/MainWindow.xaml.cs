using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
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
        private MainWindowViewModel ViewModel { get; } = new MainWindowViewModel();

        private readonly IFactory _factory = Factory.Instance;

        public MainWindow()
        {
            InitializeComponent();

            var bindings = _factory.MainWindowCommands;

            ViewModel.MenuItems = (MenuItems)bindings.MenuItems;
            CommandBindings.AddRange(bindings.CommandBindings);
            InputBindings.AddRange(bindings.InputBindings);

            ViewModel.Status = _factory.Get<IStatus>();

            DataContext = ViewModel;

            //TODO: Seems to get parent window, not surface container
            Window window = GetWindow(Surface);
            
            if (window == null)
            {
                throw new Exception("Failed to get window object?");
            }

            IntPtr hWnd = new WindowInteropHelper(window).EnsureHandle();

            IVccThread thread = _factory.Get<IVccThread>();

            Task.Run(() => thread.Run(hWnd));
        }
    }
}
