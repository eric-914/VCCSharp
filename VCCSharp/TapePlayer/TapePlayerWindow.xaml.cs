using System.Windows;

namespace VCCSharp.TapePlayer
{
    public partial class TapePlayerWindow
    {
        private readonly ITapePlayer _manager;

        public TapePlayerWindow(TapePlayerViewModel viewModel, ITapePlayer manager)
        {
            _manager = manager;

            InitializeComponent();
            DataContext = viewModel;
        }

        private void Browse(object sender, RoutedEventArgs e)
        {
            _manager.Browse();
        }

        private void Record(object sender, RoutedEventArgs e)
        {
            _manager.Record();
        }

        private void Play(object sender, RoutedEventArgs e)
        {
            _manager.Play();
        }

        private void Stop(object sender, RoutedEventArgs e)
        {
            _manager.Stop();
        }

        private void Eject(object sender, RoutedEventArgs e)
        {
            _manager.Eject();
        }

        private void Rewind(object sender, RoutedEventArgs e)
        {
            _manager.Rewind();
        }
    }
}
