using DX8;
using DX8.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using VCCSharp.Libraries;

namespace VCCSharp.DX8;

internal class DxJoystick
{
    private readonly IDxInput _input;

    private List<IDxDevice> _devices = new();

    public DxJoystick(IDxInput input)
    {
        _input = input;
    }

    public void Initialize()
    {
        var handle = KernelDll.GetModuleHandleA(IntPtr.Zero);

        _input.CreateDirectInput(handle);
    }

    public void EnumerateDevices(bool refresh)
    {
        if (!refresh && _devices.Any()) return;

        _input.EnumerateDevices();

        _devices = _input.JoystickList().ToList();
    }

    public List<string> JoystickList() => _devices.Select(x => x.InstanceName).ToList();

    public IDxJoystickState JoystickPoll(IDxDevice index)
    {
        return _input.JoystickPoll(index);
    }
}
