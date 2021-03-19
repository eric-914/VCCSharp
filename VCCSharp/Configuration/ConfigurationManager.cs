using VCCSharp.Models;

namespace VCCSharp.Configuration
{
    public class ConfigurationManager : IConfiguration
    {
        public void ShowDialog(ConfigModel model)
        {
            var viewModel = new ConfigurationViewModel { Model = model };
            var view = new ConfigurationWindow(viewModel);

            view.Show();
        }
    }
}
