using VCCSharp.Configuration.TabControls.Joystick;
using VCCSharp.Shared.ViewModels;

namespace DX8.Tester;

internal class TestWindowViewModel : NotifyViewModel
{
    public JoystickPairViewModel Joysticks { get; } = new();

    public TestWindowViewModel() { }

    public TestWindowViewModel(JoystickPairViewModel joysticks)
    {
        Joysticks = joysticks;
    }
}
