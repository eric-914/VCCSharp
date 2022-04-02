using System;
using System.Collections.Generic;
using System.Linq;
using VCCSharp.Libraries;

namespace DX8.Tester.Model;

public class DxManager
{
    public event ThreadRunnerEventHandler? PollEvent;

    private readonly IDxInput _input;

    private readonly ThreadRunner _runner = new();

    public List<DxJoystick> Devices { get; private set; } = new();

    public DxManager(IDxInput input)
    {
        _input = input;
        _runner.Tick += (_, _) => JoystickPoll();
    }

    public void Initialize()
    {
        var handle = KernelDll.GetModuleHandleA(IntPtr.Zero);

        _input.CreateDirectInput(handle);

        EnumerateDevices();

        _runner.Start();
    }

    public void EnumerateDevices()
    {
        _runner.Stop();

        _input.EnumerateDevices();

        var devices = 
            from each in _input.JoystickList()
            select new DxJoystick(each);

        Devices = devices.ToList();

        _runner.Start();
    }

    public void JoystickPoll()
    {
        foreach (var device in Devices)
        {
            device.State = _input.JoystickPoll(device.Device);
        }

        PollEvent?.Invoke(this, EventArgs.Empty);
    }
}
