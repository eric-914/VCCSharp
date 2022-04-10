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

#region Left/Right implementations

public interface ILeftJoystickStateViewModel : IJoystickStateViewModel { }
public interface IRightJoystickStateViewModel : IJoystickStateViewModel { }

public class LeftJoystickStateViewModel : JoystickStateViewModel, ILeftJoystickStateViewModel
{
    public LeftJoystickStateViewModel(IDxManager manager, IJoysticksConfiguration configuration)
        : base(manager, configuration, JoystickSides.Left) { }
}

public class RightJoystickStateViewModel : JoystickStateViewModel, IRightJoystickStateViewModel
{
    public RightJoystickStateViewModel(IDxManager manager, IJoysticksConfiguration configuration)
        : base(manager, configuration, JoystickSides.Left) { }
}

#endregion
