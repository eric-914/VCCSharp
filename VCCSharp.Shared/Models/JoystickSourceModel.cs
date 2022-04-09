using VCCSharp.Shared.Dx;

namespace VCCSharp.Shared.Models;

public class JoystickSourceModel
{
    private readonly IDxManager _manager;

    public int Count { get; set; }

    private readonly IDeviceIndex _deviceIndex;
    public int DeviceIndex
    {
        get => _deviceIndex.DeviceIndex;
        set => _deviceIndex.DeviceIndex = value;
    }

    public JoystickSourceModel(IDxManager manager, IDeviceIndex deviceIndex)
    {
        _manager = manager;
        _deviceIndex = deviceIndex;
    }

    public List<string> FindJoysticks()
    {
        _manager.EnumerateDevices();

        var list = _manager.Devices.Select(x => x.Device).ToList();

        Count = list.Count;

        return list.Select(x => x.InstanceName).ToList();
    }
}
