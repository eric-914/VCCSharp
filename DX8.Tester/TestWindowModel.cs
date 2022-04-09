﻿using DX8.Tester.Controls;
using VCCSharp.Shared.Dx;
using VCCSharp.Shared.ViewModels;

namespace DX8.Tester;

public delegate void DeviceLostEventHandler(object? sender, EventArgs e);

internal class TestWindowModel
{
    public event DeviceLostEventHandler? DeviceLostEvent;

    private readonly IDxManager _manager;

    public IJoystickStateViewModel LeftJoystick { get; private set; } = new JoystickStateViewModel();
    public IJoystickStateViewModel RightJoystick { get; private set; } = new JoystickStateViewModel();

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

        //--First (if found) will be left joystick
        LeftJoystick = Count > 0
            ? new JoystickStateViewModel(_manager, 0)
            : new JoystickStateViewModel();

        //--Second (if found) will be right joystick
        RightJoystick = Count > 1
            ? new JoystickStateViewModel(_manager, 1)
            : new JoystickStateViewModel();

        return list.Select(x => x.InstanceName).ToList();
    }

    public int Interval
    {
        get => _manager.Interval;
        set => _manager.Interval = value;
    }
}
