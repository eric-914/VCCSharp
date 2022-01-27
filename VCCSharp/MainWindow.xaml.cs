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
        public MainWindow()
        {
            InitializeComponent();

            Initialize(Factory.Instance);
        }

        private void Initialize(IFactory factory)
        {
            DataContext = factory.Get<IViewModelFactory>().CreateMainWindowViewModel(this);

            factory.Get<IVccThread>().Run(this);
        }
    }
}
