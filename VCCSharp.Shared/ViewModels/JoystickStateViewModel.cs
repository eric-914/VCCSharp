using DX8;
using DX8.Models;
using System.ComponentModel;
using VCCSharp.Shared.Dx;

namespace VCCSharp.Shared.ViewModels;

public interface IJoystickStateViewModel : INotifyPropertyChanged
{
    IDxJoystickState State { get; }
    Action Refresh { get; }
}

public class JoystickStateViewModel : NotifyViewModel, IJoystickStateViewModel
{
    private IDxJoystickState _state = new NullDxJoystickState();

    public Action Refresh { get; } = () => { };

    public JoystickStateViewModel() { }

    public JoystickStateViewModel(IDxManager manager, int index)
    {
        Refresh = () => State = manager.State(index);
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
