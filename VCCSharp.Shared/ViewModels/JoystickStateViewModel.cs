using DX8;
using DX8.Models;
using System.ComponentModel;
using VCCSharp.Shared.Dx;
using VCCSharp.Shared.Enums;
using VCCSharp.Shared.Models;

namespace VCCSharp.Shared.ViewModels;

public interface IJoystickStateViewModel : INotifyPropertyChanged
{
    IDxJoystickState State { get; }
}

public class JoystickStateViewModel : NotifyViewModel, IJoystickStateViewModel
{
    private IDxJoystickState _state = new NullDxJoystickState();

    public JoystickStateViewModel() { }

    public JoystickStateViewModel(IDxManager manager, IDeviceIndex configuration, JoystickSides side)
    {
        manager.PollEvent += (_, _) => State = manager.State(configuration.GetDeviceIndex(side));
    }

    public IDxJoystickState State
    {
        get => _state;
        set
        {
            if (_state == value) return;

            _state = value;
            OnPropertyChanged();
        }
    }
}
