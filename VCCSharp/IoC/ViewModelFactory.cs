﻿using System;
using VCCSharp.Configuration.TabControls.Joystick;
using VCCSharp.Configuration.ViewModel;
using VCCSharp.Enums;
using VCCSharp.Main;
using VCCSharp.Menu;
using VCCSharp.Models;
using VCCSharp.Modules;

namespace VCCSharp.IoC;

public interface IViewModelFactory
{
    MainWindowViewModel CreateMainWindowViewModel(IMainMenu menuItems);
    CommandViewModel CreateCommandViewModel(Action? action = null);
    MenuItemViewModel CreateMenuItemViewModel(string header, Action action);
    ConfigurationViewModel CreateConfigurationViewModel(IConfigurationModule configurationModule);
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

    public ConfigurationViewModel CreateConfigurationViewModel(IConfigurationModule configurationModule)
    {
        var services = _factory.Get<IJoystickServices>();
        var left = new JoystickViewModel(JoystickSides.Left, configurationModule.Model.Joysticks.Left, services);
        var right = new JoystickViewModel(JoystickSides.Right, configurationModule.Model.Joysticks.Right, services);
        var spectrum = new AudioSpectrum();

        return new ConfigurationViewModel(configurationModule, left, right, spectrum);
    }
}
