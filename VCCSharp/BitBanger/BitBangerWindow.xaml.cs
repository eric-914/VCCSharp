namespace VCCSharp.BitBanger
{
    public partial class BitBangerWindow
    {
        public BitBangerWindow(BitBangerViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
