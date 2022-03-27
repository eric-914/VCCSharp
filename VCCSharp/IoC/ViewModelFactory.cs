using System;
using VCCSharp.Configuration.TabControls.Audio;
using VCCSharp.Configuration.TabControls.Cpu;
using VCCSharp.Configuration.TabControls.Joystick;
using VCCSharp.Configuration.TabControls.Miscellaneous;
using VCCSharp.Configuration.ViewModel;
using VCCSharp.Enums;
using VCCSharp.Main;
using VCCSharp.Menu;
using VCCSharp.Models.Configuration;
using VCCSharp.Models.Joystick;

namespace VCCSharp.IoC;

public interface IViewModelFactory
{
    MainWindowViewModel CreateMainWindowViewModel(IMainMenu menuItems);
    CommandViewModel CreateCommandViewModel(Action? action = null);
    MenuItemViewModel CreateMenuItemViewModel(string header, Action action);
    ConfigurationViewModel CreateConfigurationViewModel(IConfiguration model);
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

    public ConfigurationViewModel CreateConfigurationViewModel(IConfiguration model)
    {
        var modules = _factory.Get<IModules>();
        var services = _factory.Get<IJoystickServices>();

        var left = new JoystickViewModel(JoystickSides.Left, model.Joysticks.Left, services);
        var right = new JoystickViewModel(JoystickSides.Right, model.Joysticks.Right, services);

        var audio = new AudioTabViewModel(model.Audio, modules.Audio);
        var cpu = new CpuTabViewModel(model.CPU, model.Memory);

        var miscellaneous = new MiscellaneousTabViewModel(model.Startup);

        return new ConfigurationViewModel(model, audio, cpu, left, right, miscellaneous);
    }
}
