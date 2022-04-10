using VCCSharp.Configuration.TabControls.Audio;
using VCCSharp.Configuration.TabControls.Cpu;
using VCCSharp.Configuration.TabControls.Display;
using VCCSharp.Configuration.TabControls.Keyboard;
using VCCSharp.Configuration.TabControls.Miscellaneous;
using VCCSharp.Configuration.ViewModel;
using VCCSharp.Main;
using VCCSharp.Menu;
using VCCSharp.Models.Configuration;
using VCCSharp.Shared.Configuration;
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
        var manager = _factory.Get<IDxManager>();
        var configuration = _factory.Get<IConfiguration>();

        var audio = _factory.Get<AudioTabViewModel>();
        var cpu = _factory.Get<CpuTabViewModel>();
        var display = _factory.Get<DisplayTabViewModel>();
        var keyboard = _factory.Get<KeyboardTabViewModel>();

        var interval = new JoystickIntervalViewModel(configuration.Joysticks, manager);

        var left = CreateJoystickConfigurationViewModel(manager, JoystickSides.Left, configuration.Joysticks.Left, configuration.Joysticks, interval);
        var right = CreateJoystickConfigurationViewModel(manager, JoystickSides.Right, configuration.Joysticks.Right, configuration.Joysticks, interval);

        var joysticks = new JoystickPairViewModel(left, right);

        var miscellaneous = new MiscellaneousTabViewModel(configuration.Startup);

        return new ConfigurationViewModel(audio, cpu, display, keyboard, joysticks, miscellaneous);
    }

    private static JoystickConfigurationViewModel CreateJoystickConfigurationViewModel(IDxManager manager, JoystickSides side, IJoystickConfiguration configuration, IDeviceIndex joysticks, JoystickIntervalViewModel interval)
    {
        var state = new JoystickStateViewModel(manager, joysticks, side);
        var model = new JoystickSourceModel(manager, joysticks, side);
        var joystickSource = new JoystickSourceViewModel(model, state, interval);
        var keyboardSource = new KeyboardSourceViewModel(configuration.KeyMap);

        return new JoystickConfigurationViewModel(side, configuration, joystickSource, keyboardSource);
    }
}
