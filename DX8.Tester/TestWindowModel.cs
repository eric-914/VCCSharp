using DX8.Tester.Model;
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
        _joystick.EnumerateDevices(true);

        var list = _joystick.Devices.Select(x => x.Device).ToList();

        Count = list.Count;

        if (Count > 0)
        {
            LeftJoystick = new JoystickStateViewModel(_joystick, 0);
        }

        if (Count > 1)
        {
            RightJoystick = new JoystickStateViewModel(_joystick, 1);
        }

        return list.Select(x => x.InstanceName).ToList();
    }

    public void Refresh()
    {
        LeftJoystick.Refresh();
        RightJoystick.Refresh();
    }
}
