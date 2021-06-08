using System.Threading.Tasks;
using VCCSharp.IoC;
using VCCSharp.Menu;

namespace VCCSharp
{
    public partial class MainWindow
    {
        public MenuItems MenuItems { get; }

        private readonly IFactory _factory = Factory.Instance;

        public MainWindow()
        {
            InitializeComponent();

            var bindings = _factory.MainWindowCommands;

            MenuItems = (MenuItems)bindings.MenuItems;
            CommandBindings.AddRange(bindings.CommandBindings);
            InputBindings.AddRange(bindings.InputBindings);

            DataContext = this;

            //Window window = GetWindow(this);
            //IntPtr hWnd = new WindowInteropHelper(window).EnsureHandle();

            Task.Run(_factory.Get<IVccThread>().Run);
        }
    }
}
