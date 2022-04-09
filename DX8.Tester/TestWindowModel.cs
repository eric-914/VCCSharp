using VCCSharp.Shared.Dx;

namespace DX8.Tester;

public delegate void DeviceLostEventHandler(object? sender, EventArgs e);

internal class TestWindowModel
{
    public event DeviceLostEventHandler? DeviceLostEvent;

    private readonly IDxManager _manager;

    public int Count { get; set; }

    public TestWindowModel(IDxManager manager)
    {
        _manager = manager;
        _manager.DeviceLostEvent += (_, _) => DeviceLostEvent?.Invoke(this, EventArgs.Empty);
        _manager.Initialize();
    }

    public List<string> FindJoysticks()
    {
        _manager.EnumerateDevices();

        var list = _manager.Devices.Select(x => x.Device).ToList();

        Count = list.Count;

        return list.Select(x => x.InstanceName).ToList();
    }

    public int Interval
    {
        get => _manager.Interval;
        set => _manager.Interval = value;
    }
}
