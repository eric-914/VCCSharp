using VCCSharp.Configuration.Options;

namespace VCCSharp.Configuration.Support;

//TODO: This isn't really configuration related
public interface IDeviceIndex
{
    int GetDeviceIndex(JoystickSides side);
    void SetDeviceIndex(JoystickSides side, int value);
}
