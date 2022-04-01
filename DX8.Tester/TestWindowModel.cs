using DX8.Tester.Model;
using System;
using System.Collections.Generic;
using VCCSharp.Libraries;

namespace DX8.Tester;

internal class TestWindowModel
{
    private readonly Dx _dx = new();
    private readonly IDxInput _input;

    public IJoystickStateViewModel LeftJoystick { get; private set; } 
    public IJoystickStateViewModel RightJoystick { get; private set; } 

    public int Count { get; set; }

    public TestWindowModel()
    {
        _input = _dx.Input;
        LeftJoystick = new JoystickStateViewModel();
        RightJoystick = new JoystickStateViewModel();
    }

    public List<string> FindJoysticks()
    {
        var handle = KernelDll.GetModuleHandleA(IntPtr.Zero);

        _input.CreateDirectInput(handle);

        _input.EnumerateDevices();

        var list = _input.JoystickList();

        Count = list.Count;

        if (Count > 0)
        {
            LeftJoystick = new JoystickStateViewModel(_input, 0);
        }

        if (Count > 1)
        {
            RightJoystick = new JoystickStateViewModel(_input, 1);
        }

        return list;
    }

    public void Refresh()
    {
        LeftJoystick.Refresh();
        RightJoystick.Refresh();
    }
}
