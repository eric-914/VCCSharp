using VCCSharp.Shared.Dx;
using VCCSharp.Shared.Enums;

namespace VCCSharp.Shared.Models;

public class JoystickSourceModel
{
    private readonly IDxManager _manager;
    private readonly JoystickSides _side;

    public int Count { get; set; }

    private readonly IDeviceIndex _deviceIndex;
    public int DeviceIndex
    {
        get => _deviceIndex.GetDeviceIndex(_side);
        set => _deviceIndex.SetDeviceIndex(_side, value);
    }

    public JoystickSourceModel(IDxManager manager, IDeviceIndex deviceIndex, JoystickSides side)
    {
        _manager = manager;
        _deviceIndex = deviceIndex;
        _side = side;
    }

    public List<string> FindJoysticks()
    {
        _manager.EnumerateDevices();

        var list = _manager.Devices.Select(x => x.Device).ToList();

        Count = list.Count;

        return list.Select(x => x.InstanceName).ToList();
    }
}
