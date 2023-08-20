using VCCSharp.Configuration.Options;

namespace VCCSharp.Configuration.Models.Implementation;

internal class JoysticksConfiguration : IJoysticksConfiguration
{
    public IJoystickConfiguration Left { get; } = new JoystickConfiguration();
    public IJoystickConfiguration Right { get; } = new JoystickConfiguration();

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
