using VCCSharp.Configuration.Models;
using VCCSharp.Configuration.Options;

namespace VCCSharp.Shared.Models;

public class NullDeviceIndex : IDeviceIndex
{
    private int _deviceIndex = -1;

    public int GetDeviceIndex(JoystickSides side) => _deviceIndex;
    public void SetDeviceIndex(JoystickSides side, int value) => _deviceIndex = value;
}