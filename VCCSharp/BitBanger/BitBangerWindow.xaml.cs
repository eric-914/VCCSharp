using System.Windows;

namespace VCCSharp.BitBanger
{
    public partial class BitBangerWindow
    {
        private readonly IBitBanger _manager;

        public BitBangerWindow(BitBangerViewModel viewModel, IBitBanger manager)
        {
            _manager = manager;

            InitializeComponent();
            DataContext = viewModel;
        }

        private void Open(object sender, RoutedEventArgs e)
        {
            _manager.Open();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            _manager.Close();
        }
    }
}
