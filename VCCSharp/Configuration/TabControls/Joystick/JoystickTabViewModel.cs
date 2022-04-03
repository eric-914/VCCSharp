using VCCSharp.Shared.ViewModels;

namespace VCCSharp.Configuration.TabControls.Joystick;

public class JoystickTabViewModel : NotifyViewModel
{
    //[LeftJoyStick]
    public JoystickViewModel Left { get; } = new();

    //[RightJoyStick]
    public JoystickViewModel Right { get; } = new();

    public JoystickTabViewModel() { }

    public JoystickTabViewModel(JoystickViewModel left, JoystickViewModel right)
    {
        Left = left;
        Right = right;
    }
}
