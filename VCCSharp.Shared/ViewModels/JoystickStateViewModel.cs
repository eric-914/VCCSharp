using DX8;
using DX8.Models;
using VCCSharp.Configuration.Options;
using VCCSharp.Shared.Dx;
using VCCSharp.Shared.Models;

namespace VCCSharp.Shared.ViewModels;

public class JoystickStateViewModel : NotifyViewModel, ILeft<JoystickStateViewModel>, IRight<JoystickStateViewModel>
{
    private IDxJoystickState _state = new NullDxJoystickState();

    public JoystickStateViewModel() { }

    protected JoystickStateViewModel(IDxManager manager, IDeviceIndex configuration, JoystickSides side)
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

public class LeftJoystickStateViewModel : JoystickStateViewModel
{
    public LeftJoystickStateViewModel(IDxManager manager, IJoysticksConfiguration configuration)
        : base(manager, configuration, JoystickSides.Left) { }
}

public class RightJoystickStateViewModel : JoystickStateViewModel
{
    public RightJoystickStateViewModel(IDxManager manager, IJoysticksConfiguration configuration)
        : base(manager, configuration, JoystickSides.Right) { }
}
