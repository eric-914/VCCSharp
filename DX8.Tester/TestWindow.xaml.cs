using System.Threading.Tasks;
using System.Windows;

namespace DX8.Tester;

public partial class TestWindow
{
    private readonly ThreadRunner _runner = new();
    private readonly TestWindowViewModel _vm = new();

    public TestWindow()
    {
        Application.Current.Exit += (_, _) => _runner.IsRunning = false;

        InitializeComponent();
            
        DataContext = _vm;
            
        Task.Run(() => _runner.Run(_vm.Refresh));
    }
}
