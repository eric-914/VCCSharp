using VCCSharp.Shared.Configuration;
using VCCSharp.Shared.Enums;
using VCCSharp.Shared.Models;

namespace VCCSharp.Models.Configuration;

public class Joysticks : IJoysticksConfiguration
{
    public IJoystickConfiguration Left { get; } = new Joystick();
    public IJoystickConfiguration Right { get; } = new Joystick();

    public int GetDeviceIndex(JoystickSides side)
    {
        var lookup = new Dictionary<JoystickSides, Func<int>>
        {
            { JoystickSides.Left, () => Left.DeviceIndex },
            { JoystickSides.Right, () => Right.DeviceIndex }
        };

        return lookup[side]();
    }

    public void SetDeviceIndex(JoystickSides side, int value)
    {
        var lookup = new Dictionary<JoystickSides, Action<int>>
        {
            { JoystickSides.Left, x => Left.DeviceIndex = x },
            { JoystickSides.Right, x => Right.DeviceIndex = x}
        };

        lookup[side](value);
    }

    public int Interval { get; set; } = 100;
}
