using DX8;
using DX8.Models;
using System.ComponentModel;
using VCCSharp.Shared.Dx;

namespace VCCSharp.Shared.ViewModels;

public interface IJoystickStateViewModel : INotifyPropertyChanged
{
    IDxJoystickState State { get; }
}

public class JoystickStateViewModel : NotifyViewModel, IJoystickStateViewModel
{
    private IDxJoystickState _state = new NullDxJoystickState();

    public JoystickStateViewModel() { }

    public JoystickStateViewModel(IDxManager manager, int index)
    {
        manager.PollEvent += (_, _) => State = manager.State(index);
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
