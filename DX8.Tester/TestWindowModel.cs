﻿using DX8.Tester.Model;
using System.Collections.Generic;
using System.Linq;

namespace DX8.Tester;

internal class TestWindowModel
{
    private readonly DxManager _joystick;

    public IJoystickStateViewModel LeftJoystick { get; private set; } = new JoystickStateViewModel();
    public IJoystickStateViewModel RightJoystick { get; private set; } = new JoystickStateViewModel();

    public int Count { get; set; }

    public TestWindowModel()
    {
        _joystick = new Dx().Joystick;
        _joystick.PollEvent += (_, _) => Refresh();
        _joystick.Initialize();
    }

    public List<string> FindJoysticks()
    {
        _joystick.EnumerateDevices();

        var list = _joystick.Devices.Select(x => x.Device).ToList();

        Count = list.Count;

        //--First (if found) will be left joystick
        LeftJoystick = Count > 0
            ? new JoystickStateViewModel(_joystick, 0)
            : new JoystickStateViewModel();

        //--Second (if found) will be right joystick
        RightJoystick = Count > 1
            ? new JoystickStateViewModel(_joystick, 1)
            : new JoystickStateViewModel();

        return list.Select(x => x.InstanceName).ToList();
    }

    private void Refresh()
    {
        LeftJoystick.Refresh();
        RightJoystick.Refresh();
    }
}
