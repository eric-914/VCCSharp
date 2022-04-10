namespace VCCSharp.Shared.ViewModels;

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
