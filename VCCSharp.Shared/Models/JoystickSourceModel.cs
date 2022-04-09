using VCCSharp.Shared.Dx;

namespace VCCSharp.Shared.Models;

public class JoystickSourceModel
{
    private readonly IDxManager _manager;

    public int Count { get; set; }

    public JoystickSourceModel(IDxManager manager)
    {
        _manager = manager;
    }

    public List<string> FindJoysticks()
    {
        _manager.EnumerateDevices();

        var list = _manager.Devices.Select(x => x.Device).ToList();

        Count = list.Count;

        return list.Select(x => x.InstanceName).ToList();
    }
}
