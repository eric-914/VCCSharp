using System.Threading.Tasks;
using System.Windows;
using VCCSharp.IoC;

namespace VCCSharp
{
    public partial class MainWindow : Window
    {
        private readonly IFactory _factory = Factory.Instance;

        public MainWindow()
        {
            InitializeComponent();

            Task.Run(_factory.Get<IVccThread>().Run);
        }
    }
}
