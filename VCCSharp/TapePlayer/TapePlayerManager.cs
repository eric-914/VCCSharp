using VCCSharp.IoC;

namespace VCCSharp.TapePlayer
{
    public class TapePlayerManager : ITapePlayer
    {
        public void ShowDialog()
        {
            var viewModel = new TapePlayerViewModel();
            var view = new TapePlayerWindow(viewModel);

            view.Show();
        }
    }
}
