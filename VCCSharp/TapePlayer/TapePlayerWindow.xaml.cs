namespace VCCSharp.TapePlayer
{
    public partial class TapePlayerWindow
    {
        public TapePlayerWindow(TapePlayerViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
