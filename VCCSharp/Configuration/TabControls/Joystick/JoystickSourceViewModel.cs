using System.Windows.Input;
using VCCSharp.Shared.Commands;
using VCCSharp.Shared.Models;
using VCCSharp.Shared.ViewModels;

namespace VCCSharp.Configuration.TabControls.Joystick;

public class JoystickSourceViewModel : NotifyViewModel
{
    private readonly JoystickSourceModel? _model;

    public ICommand RefreshListCommand { get; } = new ActionCommand(() => throw new NotImplementedException());

    //--TODO: Getting the IDxJoystickState requires State.State
    public JoystickStateViewModel State { get; } = new();

    public int Count => 0;

    public JoystickSourceViewModel() { }

    public JoystickSourceViewModel(JoystickSourceModel model, JoystickStateViewModel state)
    {
        _model = model;
        State = state;

        RefreshListCommand = new ActionCommand(RefreshList);
        RefreshList();
    }

    // Index of which Joystick is selected
    public int DeviceIndex
    {
        get => _model?.DeviceIndex ?? -1;
        set
        {
            if (_model != null) _model.DeviceIndex = value;
        }
    }

    private List<string> _availableJoysticks = new();
    public List<string> AvailableJoysticks
    {
        get => _availableJoysticks;
        private set
        {
            _availableJoysticks = value;
            OnPropertyChanged();
        }
    }

    public void RefreshList()
    {
        AvailableJoysticks = _model?.FindJoysticks() ?? new List<string>();
        OnPropertyChanged(nameof(Count));
    }
}
