namespace VCCSharp.Shared.ViewModels;

public class JoystickPairViewModel : NotifyViewModel
{
    //[LeftJoyStick]
    public JoystickConfigurationViewModel Left { get; } = new();

    //[RightJoyStick]
    public JoystickConfigurationViewModel Right { get; } = new();

    public JoystickPairViewModel() { }

    public JoystickPairViewModel(ILeftJoystickConfigurationViewModel left, IRightJoystickConfigurationViewModel right)
    {
        Left = (JoystickConfigurationViewModel)left;
        Right = (JoystickConfigurationViewModel)right;
    }
}
