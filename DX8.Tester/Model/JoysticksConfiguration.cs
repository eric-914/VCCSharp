using DX8.Tester.Mock;
using VCCSharp.Shared.Configuration;
using VCCSharp.Shared.Enums;
using VCCSharp.Shared.Models;

namespace DX8.Tester.Model;

public class JoysticksConfiguration : IJoysticksConfiguration
{
    private int _leftDeviceIndex;
    private int _rightDeviceIndex = 1;

    public int Interval { get; set; } = 100;

    public IJoystickConfiguration Left { get; } = new MockJoystickConfiguration();
    public IJoystickConfiguration Right { get; } = new MockJoystickConfiguration();

    public int GetDeviceIndex(JoystickSides side)
    {
        var lookup = new Dictionary<JoystickSides, Func<int>>
        {
            { JoystickSides.Left, () => _leftDeviceIndex },
            { JoystickSides.Right, () => _rightDeviceIndex }
        };

        return lookup[side]();
    }

    public void SetDeviceIndex(JoystickSides side, int value)
    {
        var lookup = new Dictionary<JoystickSides, Action<int>>
        {
            { JoystickSides.Left, x => _leftDeviceIndex = x },
            { JoystickSides.Right, x => _rightDeviceIndex = x}
        };

        lookup[side](value);
    }

}
