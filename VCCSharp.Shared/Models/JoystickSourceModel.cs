﻿using VCCSharp.Shared.Dx;
using VCCSharp.Shared.Enums;

namespace VCCSharp.Shared.Models;

public interface IJoystickSourceModel
{
    int Count { get; set; }
    List<string> Joysticks { get; }
    int DeviceIndex { get; set; }
    void RefreshList();
}

public class JoystickSourceModel : IJoystickSourceModel
{
    private readonly IDxManager _manager;
    private readonly JoystickSides _side;
    private readonly IDeviceIndex _deviceIndex;

    public int Count { get; set; }

    public List<string> Joysticks { get; private set; } = new();

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

        _manager.DeviceRefreshListEvent += (_, _) => Refresh();
    }

    public void RefreshList() => _manager.EnumerateDevices();

    private void Refresh()
    {
        Joysticks = _manager.DeviceNames;
        Count = Joysticks.Count;
    }
}

public interface ILeftJoystickSourceModel : IJoystickSourceModel {}
public interface IRightJoystickSourceModel : IJoystickSourceModel {}

public class LeftJoystickSourceModel : JoystickSourceModel, ILeftJoystickSourceModel
{
    public LeftJoystickSourceModel(IDxManager manager, IJoysticksConfiguration configuration) 
        : base(manager, configuration, JoystickSides.Left) { }
}

public class RightJoystickSourceModel : JoystickSourceModel, IRightJoystickSourceModel
{
    public RightJoystickSourceModel(IDxManager manager, IJoysticksConfiguration configuration) 
        : base(manager, configuration, JoystickSides.Right) { }
}
