namespace VCCSharp.BitBanger
{
    public class BitBangerManager : IBitBanger
    {
        public void ShowDialog()
        {
            var viewModel = new BitBangerViewModel();
            var view = new BitBangerWindow(viewModel);

            view.Show();
        }
    }
}
