using VCCSharp.IoC;
using VCCSharp.Models;

namespace VCCSharp.Configuration
{
    public class ConfigurationManager : IConfiguration
    {
        private readonly IModules _modules;

        public ConfigurationManager(IModules modules)
        {
            _modules = modules;
        }

        public unsafe void ShowDialog(ConfigState* state)
        {
            var viewModel = new ConfigurationViewModel { State = state };
            var view = new ConfigurationWindow(viewModel) { Apply = ApplyChanges };

            view.Closing += (sender, args) => _modules.Audio.Spectrum = null;
            view.Show();

            _modules.Audio.Spectrum = viewModel.Spectrum;
        }

        public void ApplyChanges()
        {
            _modules.Vcc.ApplyConfigurationChanges();
        }
    }
}
