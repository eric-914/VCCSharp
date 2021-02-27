using System.Threading.Tasks;
using System.Windows;

namespace VCCSharp
{
    public partial class MainWindow : Window
    {
        private readonly VccThread _vcc = new VccThread();

        public MainWindow()
        {
            InitializeComponent();

            Task.Run(_vcc.Run);
        }
    }
}
