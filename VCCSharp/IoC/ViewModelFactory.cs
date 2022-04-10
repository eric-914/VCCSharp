using VCCSharp.Configuration.TabControls.Audio;
using VCCSharp.Configuration.TabControls.Cpu;
using VCCSharp.Configuration.TabControls.Display;
using VCCSharp.Configuration.TabControls.Keyboard;
using VCCSharp.Configuration.TabControls.Miscellaneous;
using VCCSharp.Configuration.ViewModel;
using VCCSharp.Main;
using VCCSharp.Menu;
using VCCSharp.Models.Configuration;
using VCCSharp.Shared.Enums;
using VCCSharp.Shared.Models;
using VCCSharp.Shared.ViewModels;

namespace VCCSharp.IoC;

public interface IViewModelFactory
{
    MainWindowViewModel CreateMainWindowViewModel(IMainMenu menuItems);
    CommandViewModel CreateCommandViewModel(Action? action = null);
    MenuItemViewModel CreateMenuItemViewModel(string header, Action action);
    ConfigurationViewModel CreateConfigurationViewModel();
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

    public ConfigurationViewModel CreateConfigurationViewModel()
    {
        var configuration = _factory.Get<IConfiguration>();

        var audio = _factory.Get<AudioTabViewModel>();
        var cpu = _factory.Get<CpuTabViewModel>();
        var display = _factory.Get<DisplayTabViewModel>();
        var keyboard = _factory.Get<KeyboardTabViewModel>();
        var miscellaneous = new MiscellaneousTabViewModel(configuration.Startup);

        var left = CreateJoystickConfigurationViewModel(JoystickSides.Left);
        var right = CreateJoystickConfigurationViewModel(JoystickSides.Right);

        var joysticks = new JoystickPairViewModel(left, right);

        return new ConfigurationViewModel(audio, cpu, display, keyboard, joysticks, miscellaneous);
    }

    private JoystickConfigurationViewModel CreateJoystickConfigurationViewModel(JoystickSides side)
    {
        var joysticks = _factory.Get<IJoysticksConfiguration>();

        var interval = _factory.Get<JoystickIntervalViewModel>();

        var joystick = side == JoystickSides.Left ? joysticks.Left : joysticks.Right;

        var state = side == JoystickSides.Left
            ? (IJoystickStateViewModel)_factory.Get<ILeftJoystickStateViewModel>()
            : _factory.Get<IRightJoystickStateViewModel>();

        var model = side == JoystickSides.Left
            ? (IJoystickSourceModel)_factory.Get<ILeftJoystickSourceModel>()
            : _factory.Get<IRightJoystickSourceModel>();

        var joystickSource = new JoystickSourceViewModel(model, state, interval);
        var keyboardSource = new KeyboardSourceViewModel(joystick.KeyMap);

        return new JoystickConfigurationViewModel(side, joystick, joystickSource, keyboardSource);
    }
}
