using System;
using System.Windows;
using VCCSharp.Configuration;
using VCCSharp.Menu;
using VCCSharp.Modules;

namespace VCCSharp.IoC
{
    public interface IViewModelFactory
    {
        MainWindowViewModel CreateMainWindowViewModel(Window window);
        CommandViewModel CreateCommandViewModel(Action action = null);
        MenuItemViewModel CreateMenuItemViewModel(string header, Action action);
        ConfigurationViewModel CreateConfigurationViewModel(IConfig config);
    }

    public class ViewModelFactory : IViewModelFactory
    {
        private readonly IFactory _factory;

        public ViewModelFactory(IFactory factory)
        {
            _factory = factory;
        }

        public MainWindowViewModel CreateMainWindowViewModel(Window window)
        {
            return new MainWindowViewModel
            {
                MenuItems = _factory.Get<IWindowEvents>().Bind(window),
                Status = _factory.Get<IStatus>()
            };
        }

        public CommandViewModel CreateCommandViewModel(Action action = null)
        {
            return new CommandViewModel(action ?? (() => { }));
        }

        public MenuItemViewModel CreateMenuItemViewModel(string header, Action action)
        {
            return new MenuItemViewModel { Header = header, Action = action };
        }

        public ConfigurationViewModel CreateConfigurationViewModel(IConfig config)
        {
            return new ConfigurationViewModel
            {
                Config = config,
                Model = config.ConfigModel,
                LeftModel = config.GetLeftJoystick(), 
                RightModel = config.GetRightJoystick()
            };
        }
    }
}
