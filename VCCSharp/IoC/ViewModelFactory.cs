using System;
using VCCSharp.Configuration.TabControls.Joystick;
using VCCSharp.Configuration.ViewModel;
using VCCSharp.Enums;
using VCCSharp.Main;
using VCCSharp.Menu;
using VCCSharp.Models;
using VCCSharp.Models.Audio;
using VCCSharp.Models.Joystick;
using VCCSharp.Modules;

namespace VCCSharp.IoC;

public interface IViewModelFactory
{
    MainWindowViewModel CreateMainWindowViewModel(IMainMenu menuItems);
    CommandViewModel CreateCommandViewModel(Action? action = null);
    MenuItemViewModel CreateMenuItemViewModel(string header, Action action);
    ConfigurationViewModel CreateConfigurationViewModel(IConfigurationManager manager);
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

    public CommandViewModel CreateCommandViewModel(Action? action = null)
    {
        return new CommandViewModel(action ?? (() => { }));
    }

    public MenuItemViewModel CreateMenuItemViewModel(string header, Action action)
    {
        return new MenuItemViewModel { Header = header, Action = action };
    }

    public ConfigurationViewModel CreateConfigurationViewModel(IConfigurationManager manager)
    {
        var services = _factory.Get<IJoystickServices>();
        var left = new JoystickViewModel(JoystickSides.Left, manager.Model.Joysticks.Left, services);
        var right = new JoystickViewModel(JoystickSides.Right, manager.Model.Joysticks.Right, services);
        var spectrum = new AudioSpectrum();

        var soundDevices = _factory.Get<IModules>().Audio.FindSoundDevices();

        return new ConfigurationViewModel(manager, left, right, spectrum, soundDevices);
    }
}
