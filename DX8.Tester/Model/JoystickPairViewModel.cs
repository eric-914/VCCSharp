using VCCSharp.Shared.ViewModels;

namespace DX8.Tester.Model;

public class JoystickPairViewModel : NotifyViewModel
{
    //[LeftJoyStick]
    public JoystickConfigurationViewModel Left { get; } = new();

    //[RightJoyStick]
    public JoystickConfigurationViewModel Right { get; } = new();

    public JoystickPairViewModel() { }

    public JoystickPairViewModel(JoystickConfigurationViewModel left, JoystickConfigurationViewModel right)
    {
        Left = left;
        Right = right;
    }
}
