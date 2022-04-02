namespace DX8.Tester;

public partial class TestWindow
{
    private readonly TestWindowViewModel _vm = new();

    public TestWindow()
    {
        InitializeComponent();
            
        DataContext = _vm;
    }
}
