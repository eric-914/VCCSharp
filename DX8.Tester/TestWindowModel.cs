using DX8.Tester.Model;
using System;
using System.Collections.Generic;
using VCCSharp.Libraries;

namespace DX8.Tester;

internal class TestWindowModel
{
    private readonly Dx _dx = new();
    private readonly IDxInput _input;

    public DPadModel DPad { get; } = new();

    public ButtonModel Button { get; } = new();

    public TestWindowModel()
    {
        _input = _dx.Input;
    }

    public List<string> FindJoysticks()
    {
        var handle = KernelDll.GetModuleHandleA(IntPtr.Zero);

        _input.CreateDirectInput(handle);

        return _input.EnumerateDevices();
    }

    public void Refresh()
    {
        var state = _input.JoystickPoll(0);
    }
}
