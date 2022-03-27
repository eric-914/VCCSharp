using VCCSharp.Enums;
using VCCSharp.Models.Configuration;
using VCCSharp.Models.Joystick;

namespace VCCSharp.Configuration.TabControls.Joystick;

public class JoystickTabViewModel
{
    //[LeftJoyStick]
    public JoystickViewModel Left { get; } = new();

    //[RightJoyStick]
    public JoystickViewModel Right { get; } = new();

    public JoystickTabViewModel() { }

    public JoystickTabViewModel(Joysticks model, IJoystickServices services)
    {
        Left = new JoystickViewModel(JoystickSides.Left, model.Left, services);
        Right = new JoystickViewModel(JoystickSides.Right, model.Right, services);
    }
}
