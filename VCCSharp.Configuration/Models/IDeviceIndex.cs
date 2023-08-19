using VCCSharp.Configuration.Options;

namespace VCCSharp.Configuration.Models;

public interface IDeviceIndex
{
    int GetDeviceIndex(JoystickSides side);
    void SetDeviceIndex(JoystickSides side, int value);
}
