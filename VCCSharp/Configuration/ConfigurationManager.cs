using VCCSharp.Models;

namespace VCCSharp.Configuration
{
    public class ConfigurationManager : IConfiguration
    {
        public unsafe void ShowDialog(ConfigState* state, ConfigModel* model)
        {
            var viewModel = new ConfigurationViewModel { State = state, Model = model };
            var view = new ConfigurationWindow(viewModel);

            view.Show();
        }
    }
}
