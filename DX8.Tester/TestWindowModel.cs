using System;
using DX8.Tester.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using VCCSharp.Shared;

namespace DX8.Tester;

public delegate void DeviceLostEventHandler(object? sender, EventArgs e);

internal class TestWindowModel
{
    public event DeviceLostEventHandler? DeviceLostEvent;

    private readonly DxManager _joystick;

    public IJoystickStateViewModel LeftJoystick { get; private set; } = new JoystickStateViewModel();
    public IJoystickStateViewModel RightJoystick { get; private set; } = new JoystickStateViewModel();

    public int Count { get; set; }

    public TestWindowModel()
    {
        var factory = new Factory();

        var dispatcher = new DispatcherWrapper(Application.Current.Dispatcher);
        var runner = factory.CreateThreadRunner(dispatcher);
        Application.Current.Exit += (_, _) => runner.Stop();

        _joystick = factory.CreateManager(runner);
        _joystick.PollEvent += (_, _) => Refresh();
        _joystick.DeviceLostEvent += (_, _) => DeviceLostEvent?.Invoke(this, EventArgs.Empty);
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

    public int Interval
    {
        get => _joystick.Interval;
        set => _joystick.Interval = value;
    }

    internal class DispatcherWrapper : IDispatcher
    {
        private readonly Dispatcher _source;

        public DispatcherWrapper(Dispatcher source)
        {
            _source = source;
        }
        public void Invoke(Action callback)
        {
            _source.Invoke(callback);
        }
    }
}
