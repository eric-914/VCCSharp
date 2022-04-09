using VCCSharp.Enums;
using VCCSharp.Shared.Enums;

namespace DX8.Tester.Model;

public class JoystickConfigurationViewModel
{
    public JoystickSourceViewModel JoystickSource { get; } = new();

    //TODO: Other sources here...

    public JoystickSides Side { get; }

    public int InputSource = (int)JoystickDevices.Joystick;

    public JoystickConfigurationViewModel() { }

    public JoystickConfigurationViewModel(JoystickSides side, JoystickSourceViewModel source)
    {
        Side = side;
        JoystickSource = source;
    }
}
