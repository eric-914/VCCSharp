using VCCSharp.Configuration.TabControls.Audio;
using VCCSharp.Configuration.TabControls.Cpu;
using VCCSharp.Configuration.TabControls.Display;
using VCCSharp.Configuration.TabControls.Keyboard;
using VCCSharp.Configuration.TabControls.Miscellaneous;
using VCCSharp.Configuration.ViewModel;
using VCCSharp.Main;
using VCCSharp.Menu;
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
        var audio = _factory.Get<AudioTabViewModel>();
        var cpu = _factory.Get<CpuTabViewModel>();
        var display = _factory.Get<DisplayTabViewModel>();
        var keyboard = _factory.Get<KeyboardTabViewModel>();
        var miscellaneous = _factory.Get<MiscellaneousTabViewModel>();
        var joysticks = _factory.Get<JoystickPairViewModel>();

        return new ConfigurationViewModel(audio, cpu, display, keyboard, joysticks, miscellaneous);

        //return _factory.Get<ConfigurationViewModel>();
    }
}
