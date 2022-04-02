using System;
using System.ComponentModel;
using DX8.Models;

namespace DX8.Tester.Model;

public interface IJoystickStateViewModel : INotifyPropertyChanged
{
    IDxJoystickState Joystick { get; }
    Action Refresh { get; }
}

public class JoystickStateViewModel : NotifyViewModel, IJoystickStateViewModel
{
    private IDxJoystickState _joystick = new NullDxJoystickState();

    public Action Refresh { get; } = () => { };

    public JoystickStateViewModel() { }

    public JoystickStateViewModel(DxManager manager, int index)
    {
        Refresh = () => Joystick = manager.Devices[index].State;
    }

    public IDxJoystickState Joystick
    {
        get => _joystick;
        set
        {
            if (_joystick == value) return;

            _joystick = value;
            OnPropertyChanged();
        }
    }
}
