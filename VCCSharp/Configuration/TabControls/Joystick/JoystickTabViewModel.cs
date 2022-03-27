using VCCSharp.Enums;
using VCCSharp.Modules;
using Joysticks = VCCSharp.Models.Configuration.Joysticks;

namespace VCCSharp.Configuration.TabControls.Joystick;

public class JoystickTabViewModel
{
    //[LeftJoyStick]
    public JoystickViewModel Left { get; } = new();

    //[RightJoyStick]
    public JoystickViewModel Right { get; } = new();

    public JoystickTabViewModel() { }

    public JoystickTabViewModel(Joysticks model, IJoysticks module)
    {
        Left = new JoystickViewModel(JoystickSides.Left, model.Left, module);
        Right = new JoystickViewModel(JoystickSides.Right, model.Right, module);
    }
}
