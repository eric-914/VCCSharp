using VCCSharp.IoC;
using VCCSharp.Modules;

namespace VCCSharp.Configuration
{
    public class ConfigurationManager : IConfigurationWindow
    {
        private readonly IModules _modules;
        private readonly IViewModelFactory _factory;

        public ConfigurationManager(IModules modules, IViewModelFactory factory)
        {
            _modules = modules;
            _factory = factory;
        }

        public void ShowDialog(IConfig config)
        {
            var viewModel = _factory.CreateConfigurationViewModel(config);

            var view = new ConfigurationWindow(viewModel) { Apply = ApplyChanges };

            view.Closing += (_, _) => _modules.Audio.Spectrum = null;
            view.Show();

            _modules.Audio.Spectrum = viewModel.Spectrum;
        }

        public void ApplyChanges()
        {
            _modules.Vcc.ApplyConfigurationChanges();
        }
    }
}
