namespace VCCSharp.Configuration.TabControls.Joystick;

public class JoystickTabViewModel
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
