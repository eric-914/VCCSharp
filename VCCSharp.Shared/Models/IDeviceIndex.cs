namespace VCCSharp.Shared.Models;

public interface IDeviceIndex
{
    int DeviceIndex { get; set; }
}

public class NullDeviceIndex : IDeviceIndex
{
    public int DeviceIndex { get; set; }
}