using System;
using System.Threading.Tasks;
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
        public MainWindow()
        {
            InitializeComponent();

            Initialize(Factory.Instance);
        }

        private void Initialize(IFactory factory)
        {
            DataContext = factory.Get<IViewModelFactory>().CreateMainWindowViewModel(this);

            IntPtr hWnd = new WindowInteropHelper(this).EnsureHandle(); //--Note: This is UI thread

            //TODO: Figure out how to pass in "Surface" control instead of Main Window

            Task.Run(() => factory.Get<IVccThread>().Run(hWnd));
        }
    }
}
