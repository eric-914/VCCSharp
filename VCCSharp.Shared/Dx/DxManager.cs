using DX8;
using DX8.Models;
using System.Windows;
using VCCSharp.Libraries;
using VCCSharp.Shared.Threading;

namespace VCCSharp.Shared.Dx;

public delegate void PollEventHandler(object? sender, EventArgs e);
public delegate void DeviceRefreshListEventHandler(object? sender, EventArgs e);
public delegate void DeviceLostEventHandler(object? sender, EventArgs e);

public interface IDxManager
{
    event PollEventHandler? PollEvent;
    event DeviceRefreshListEventHandler? DeviceRefreshListEvent;
    event DeviceLostEventHandler? DeviceLostEvent;

    List<DxJoystick> Devices { get; }
    List<string> DeviceNames { get; }
    int Interval { get; set; }

    IDxManager Initialize();
    IDxManager EnumerateDevices();

    IDxJoystickState State(int index);
}

public class DxManager : IDxManager
{
    public event PollEventHandler? PollEvent;
    public event DeviceRefreshListEventHandler? DeviceRefreshListEvent;
    public event DeviceLostEventHandler? DeviceLostEvent;

    private readonly IDxInput _input;

    private readonly IThreadRunner _runner;

    public List<DxJoystick> Devices { get; private set; } = new();
    public List<string> DeviceNames => Devices.Select(x => x.Device.InstanceName).ToList();

    private static readonly IDxJoystickState NullState = new NullDxJoystickState();

    public DxManager(IDxInput input, IThreadRunner runner)
    {
        _input = input;
        _runner = runner;
    }

    public IDxManager Initialize()
    {
        var handle = KernelDll.GetModuleHandleA(IntPtr.Zero);

        _input.CreateDirectInput(handle);
        _runner.Tick += (_, _) => JoystickPoll();

        return this;
    }

    public IDxManager EnumerateDevices()
    {
        _runner.Stop();

        _input.EnumerateDevices();

        var devices =
            from each in _input.JoystickList()
            select new DxJoystick(each);

        Devices = devices.ToList();

        _runner.Start();

        DeviceRefreshListEvent?.Invoke(this, EventArgs.Empty);

        return this;
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
                DeviceLost();
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

    public IDxJoystickState State(int index)
    {
        return index != -1 && index < Devices.Count ? Devices[index].State : NullState;
    }

    private void DeviceLost()
    {
        const string message = @"
A joystick connection has been lost.
Choose OK to refresh the joystick list.
";

        _runner.Stop(); //--Stop further polling until this is addressed

        DeviceLostEvent?.Invoke(this, EventArgs.Empty);

        MessageBox.Show(message, "Device Lost");

        EnumerateDevices();
    }
}
