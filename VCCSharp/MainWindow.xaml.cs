using System.Threading.Tasks;
using VCCSharp.IoC;
using VCCSharp.Menu;

namespace VCCSharp
{
    public partial class MainWindow
    {
        public MainMenu MenuItems { get; }

        private readonly IFactory _factory = Factory.Instance;

        public MainWindow()
        {
            InitializeComponent();

            MenuItems = _factory.MenuItems;

            DataContext = this;

            Task.Run(_factory.Get<IVccThread>().Run);
        }
    }
}
