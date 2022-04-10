using DX8.Tester.Model;

namespace DX8.Tester;

public partial class TestWindow
{
    public TestWindow()
    {
        InitializeComponent();

        DataContext = Factory.Instance.CreateViewModel(new JoysticksConfiguration());
    }
}
