using System;
using System.Collections.Generic;
using System.Linq;
using VCCSharp.Libraries;
using VCCSharp.Shared;

namespace DX8.Tester.Model;

public delegate void PollEventHandler(object? sender, EventArgs e);
public delegate void DeviceLostEventHandler(object? sender, EventArgs e);

public class DxManager
{
    public event PollEventHandler? PollEvent;
    public event DeviceLostEventHandler? DeviceLostEvent;

    private readonly IDxInput _input;

    private readonly IThreadRunner _runner;

    public List<DxJoystick> Devices { get; private set; } = new();

    public DxManager(IDxInput input, IThreadRunner runner)
    {
        _input = input;
        _runner = runner;
        _runner.Tick += (_, _) => JoystickPoll();
    }

    public void Initialize()
    {
        var handle = KernelDll.GetModuleHandleA(IntPtr.Zero);

        _input.CreateDirectInput(handle);

        //EnumerateDevices();
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

    private void JoystickPoll()
    {
        foreach (var device in Devices)
        {
            device.State = _input.JoystickPoll(device.Device);
        }

        //--This will happen if any joysticks get disconnected
        if (Devices.Any(x => x.State.ErrorCode == JoystickStateErrorCodes.COMException))
        {
            if (_runner.IsRunning)
            {
                _runner.Stop(); //--Stop further polling until this is addressed
                DeviceLostEvent?.Invoke(this, EventArgs.Empty);
            }
        }
        else
        {
            PollEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    public int Interval
    {
        get => _runner.Interval;
        set => _runner.Interval = value;
    }
}
