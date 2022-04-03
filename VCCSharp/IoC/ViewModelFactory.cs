using VCCSharp.Configuration.TabControls.Audio;
using VCCSharp.Configuration.TabControls.Cpu;
using VCCSharp.Configuration.TabControls.Display;
using VCCSharp.Configuration.TabControls.Joystick;
using VCCSharp.Configuration.TabControls.Keyboard;
using VCCSharp.Configuration.TabControls.Miscellaneous;
using VCCSharp.Configuration.ViewModel;
using VCCSharp.Enums;
using VCCSharp.Main;
using VCCSharp.Menu;
using VCCSharp.Models.Configuration;
using VCCSharp.Shared.Dx;
using VCCSharp.Shared.ViewModels;

namespace VCCSharp.IoC;

public interface IViewModelFactory
{
    MainWindowViewModel CreateMainWindowViewModel(IMainMenu menuItems);
    CommandViewModel CreateCommandViewModel(Action? action = null);
    MenuItemViewModel CreateMenuItemViewModel(string header, Action action);
    ConfigurationViewModel CreateConfigurationViewModel(IConfiguration model, IDxManager manager);
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

    public ConfigurationViewModel CreateConfigurationViewModel(IConfiguration model, IDxManager manager)
    {
        var modules = _factory.Get<IModules>();

        var audio = new AudioTabViewModel(model.Audio, modules.Audio);
        var cpu = new CpuTabViewModel(model.CPU, model.Memory);
        var display = new DisplayTabViewModel(model.CPU, model.Video, model.Window);
        var keyboard = new KeyboardTabViewModel(model.Keyboard);

        var leftState = new JoystickStateViewModel(manager, (int)JoystickSides.Left);
        var rightState = new JoystickStateViewModel(manager, (int)JoystickSides.Right);
        var left = new JoystickViewModel(JoystickSides.Left, model.Joysticks.Left, modules.Joysticks, leftState);
        var right = new JoystickViewModel(JoystickSides.Right, model.Joysticks.Right, modules.Joysticks, rightState);
        var joysticks = new JoystickTabViewModel(left, right);

        var miscellaneous = new MiscellaneousTabViewModel(model.Startup);

        return new ConfigurationViewModel(audio, cpu, display, keyboard, joysticks, miscellaneous);
    }
}
