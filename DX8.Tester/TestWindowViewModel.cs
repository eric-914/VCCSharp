using VCCSharp.Shared.ViewModels;

namespace DX8.Tester;

internal class TestWindowViewModel : NotifyViewModel
{
    public JoystickPairViewModel Joysticks { get; } = new JoystickPairViewModel(new JoystickConfigurationViewModel(), new JoystickConfigurationViewModel());

    public TestWindowViewModel() { }

    public TestWindowViewModel(JoystickPairViewModel joysticks)
    {
        Joysticks = joysticks;
    }
}
