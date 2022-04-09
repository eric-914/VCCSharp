using VCCSharp.Shared.Enums;

namespace VCCSharp.Shared.Models;

public interface IDxConfiguration : IDeviceIndex, IInterval { }

public class NullDxConfiguration : IDxConfiguration
{
    private readonly IDeviceIndex _deviceIndex = new NullDeviceIndex();
    private readonly IInterval _interval = new NullInterval();

    public int GetDeviceIndex(JoystickSides side) => _deviceIndex.GetDeviceIndex(side);
    public void SetDeviceIndex(JoystickSides side, int value) => _deviceIndex.SetDeviceIndex(side, value);
    public int Interval { get => _interval.Interval; set => _interval.Interval = value; }
}
