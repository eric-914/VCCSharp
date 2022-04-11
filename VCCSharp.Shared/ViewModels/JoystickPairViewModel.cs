namespace VCCSharp.Shared.ViewModels;

public class JoystickPairViewModel : NotifyViewModel
{
    //[LeftJoyStick]
    public IJoystickConfigurationViewModel Left { get; } = new JoystickConfigurationViewModel();

    //[RightJoyStick]
    public IJoystickConfigurationViewModel Right { get; } = new JoystickConfigurationViewModel();

    public JoystickPairViewModel() { }

    public JoystickPairViewModel(ILeftJoystickConfigurationViewModel left, IRightJoystickConfigurationViewModel right)
    {
        Left = left;
        Right = right;
    }
}
