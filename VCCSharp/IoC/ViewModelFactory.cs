using VCCSharp.Configuration.TabControls.Audio;
using VCCSharp.Configuration.TabControls.Cpu;
using VCCSharp.Configuration.TabControls.Display;
using VCCSharp.Configuration.TabControls.Joystick;
using VCCSharp.Configuration.TabControls.Keyboard;
using VCCSharp.Configuration.TabControls.Miscellaneous;
using VCCSharp.Configuration.ViewModel;
using VCCSharp.Main;
using VCCSharp.Menu;
using VCCSharp.Models.Configuration;
using VCCSharp.Shared.Dx;
using VCCSharp.Shared.Enums;
using VCCSharp.Shared.Models;
using VCCSharp.Shared.ViewModels;

namespace VCCSharp.IoC;

public interface IViewModelFactory
{
    MainWindowViewModel CreateMainWindowViewModel(IMainMenu menuItems);
    CommandViewModel CreateCommandViewModel(Action? action = null);
    MenuItemViewModel CreateMenuItemViewModel(string header, Action action);
    ConfigurationViewModel CreateConfigurationViewModel(IConfiguration configuration, IDxManager manager);
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

    public ConfigurationViewModel CreateConfigurationViewModel(IConfiguration configuration, IDxManager manager)
    {
        var modules = _factory.Get<IModules>();

        var audio = new AudioTabViewModel(configuration.Audio, modules.Audio);
        var cpu = new CpuTabViewModel(configuration.CPU, configuration.Memory);
        var display = new DisplayTabViewModel(configuration.CPU, configuration.Video, configuration.Window);
        var keyboard = new KeyboardTabViewModel(configuration.Keyboard);

        var leftState = new JoystickStateViewModel(manager, configuration.Joysticks, JoystickSides.Left);
        var rightState = new JoystickStateViewModel(manager, configuration.Joysticks, JoystickSides.Right);

        var leftModel = new JoystickSourceModel(manager, configuration.Joysticks, JoystickSides.Left);
        var rightModel = new JoystickSourceModel(manager, configuration.Joysticks, JoystickSides.Right);

        var interval = new JoystickIntervalViewModel(configuration.Joysticks, manager);

        var leftJoystickSource = new JoystickSourceViewModel(leftModel, leftState, interval);
        var rightJoystickSource = new JoystickSourceViewModel(rightModel, rightState, interval);

        var leftKeyboardSource = new KeyboardSourceViewModel(configuration.Joysticks.Left.KeyMap);
        var rightKeyboardSource = new KeyboardSourceViewModel(configuration.Joysticks.Right.KeyMap);

        var left = new JoystickConfigurationViewModel(JoystickSides.Left, configuration.Joysticks.Left, leftJoystickSource, leftKeyboardSource);
        var right = new JoystickConfigurationViewModel(JoystickSides.Right, configuration.Joysticks.Right, rightJoystickSource, rightKeyboardSource);

        var joysticks = new JoystickPairViewModel(left, right);

        var miscellaneous = new MiscellaneousTabViewModel(configuration.Startup);

        return new ConfigurationViewModel(audio, cpu, display, keyboard, joysticks, miscellaneous);
    }
}
