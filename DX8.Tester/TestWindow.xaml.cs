namespace DX8.Tester;

public partial class TestWindow
{
    public TestWindow()
    {
        InitializeComponent();

        DataContext = new TestWindowViewModel(new TestWindowModel());
    }
}
