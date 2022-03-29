using System.Windows;

namespace VCCSharp.Configuration.TabControls.Joystick;

public partial class JoystickAsJoystickControl 
{
    private JoystickViewModel ViewModel => (JoystickViewModel)DataContext;

    public JoystickAsJoystickControl()
    {
        InitializeComponent();
    }

    private void RefreshList_OnClick(object sender, RoutedEventArgs e)
    {
        ViewModel.RefreshList();
    }
}
