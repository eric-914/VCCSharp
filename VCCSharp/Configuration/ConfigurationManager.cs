﻿using VCCSharp.IoC;
using VCCSharp.Models;
using VCCSharp.Modules;

namespace VCCSharp.Configuration
{
    public class ConfigurationManager : IConfiguration
    {
        private readonly IModules _modules;

        public ConfigurationManager(IModules modules)
        {
            _modules = modules;
        }

        public void ShowDialog(IConfig state, ConfigModel model, JoystickModel left, JoystickModel right)
        {
            var viewModel = new ConfigurationViewModel
            {
                Config = _modules.Config,
                State = state, 
                Model = model,
                LeftModel = left, 
                RightModel = right
            };

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
