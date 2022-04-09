using VCCSharp.Shared.Enums;

namespace VCCSharp.Shared.Models;

public interface IDeviceIndex
{
    int GetDeviceIndex(JoystickSides side);
    void SetDeviceIndex(JoystickSides side, int value);
}

public class NullDeviceIndex : IDeviceIndex
{
    private int _deviceIndex = -1;

    public int GetDeviceIndex(JoystickSides side) => _deviceIndex;
    public void SetDeviceIndex(JoystickSides side, int value) => _deviceIndex = value;
}