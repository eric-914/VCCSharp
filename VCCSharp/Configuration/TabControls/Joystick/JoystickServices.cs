using DX8;
using System;
using System.Collections.Generic;
using VCCSharp.Libraries;

namespace VCCSharp.Configuration.TabControls.Joystick;

internal interface IJoystickServices
{
    List<string> FindJoysticks();
    IDxJoystickState Poll(int index);
}

internal class JoystickServices : IJoystickServices
{
    private readonly IDxInput _input;

    public JoystickServices(IDxInput input)
    {
        _input = input;
    }

    public List<string> FindJoysticks()
    {
        var handle = KernelDll.GetModuleHandleA(IntPtr.Zero);

        _input.CreateDirectInput(handle);

        return _input.EnumerateDevices();
    }

    public IDxJoystickState Poll(int index)
    {
        return _input.JoystickPoll(index);
    }
}
