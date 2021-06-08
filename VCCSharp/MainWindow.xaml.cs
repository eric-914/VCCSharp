using System.Threading.Tasks;
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

            //Window window = GetWindow(this);
            //IntPtr hWnd = new WindowInteropHelper(window).EnsureHandle();

            Task.Run(_factory.Get<IVccThread>().Run);
        }
    }
}
