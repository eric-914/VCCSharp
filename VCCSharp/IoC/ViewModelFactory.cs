using System;
using VCCSharp.Configuration;
using VCCSharp.Main;
using VCCSharp.Menu;
using VCCSharp.Modules;

namespace VCCSharp.IoC
{
    public interface IViewModelFactory
    {
        MainWindowViewModel CreateMainWindowViewModel(IMainMenu menuItems);
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

        public MainWindowViewModel CreateMainWindowViewModel(IMainMenu menuItems)
        {
            return new MainWindowViewModel
            {
                MenuItems = menuItems,
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
                Model = config.Model,
                LeftModel = config.GetLeftJoystick(), 
                RightModel = config.GetRightJoystick()
            };
        }
    }
}
