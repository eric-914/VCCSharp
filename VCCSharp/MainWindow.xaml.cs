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

            MenuItems = bindings.MenuItems;
            CommandBindings.AddRange(bindings.CommandBindings);
            InputBindings.AddRange(bindings.InputBindings);

            DataContext = this;

            Task.Run(_factory.Get<IVccThread>().Run);
        }
    }
}
