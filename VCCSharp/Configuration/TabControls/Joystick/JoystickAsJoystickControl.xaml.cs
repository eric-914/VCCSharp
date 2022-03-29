using System.Threading.Tasks;
using System.Windows;
using VCCSharp.Models;

namespace VCCSharp.Configuration.TabControls.Joystick;

public partial class JoystickAsJoystickControl 
{
    private JoystickViewModel ViewModel => (JoystickViewModel)DataContext;

    private readonly ThreadRunner _runner = new();

    public JoystickAsJoystickControl()
    {
        InitializeComponent();
    }

    private void RefreshList_OnClick(object sender, RoutedEventArgs e)
    {
        ViewModel.RefreshList();
    }

    private void TestJoystick_OnClick(object sender, RoutedEventArgs e)
    {
        //_runner.IsRunning = !_runner.IsRunning;

        if (!_runner.IsRunning)
        {
            var vm = ViewModel;
            Task.Run(() => _runner.Run(() => vm.Refresh()));
        }
    }
}
