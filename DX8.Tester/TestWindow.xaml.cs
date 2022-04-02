using System.Windows;

namespace DX8.Tester;

public partial class TestWindow
{
    public TestWindow()
    {
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        ((TestWindowViewModel)DataContext).RefreshList();
    }
}
