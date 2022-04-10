using System.Windows.Input;
using VCCSharp.Shared.Commands;
using VCCSharp.Shared.Models;

namespace VCCSharp.Shared.ViewModels;

public class JoystickSourceViewModel : NotifyViewModel
{
    private readonly IJoystickSourceModel? _model;

    public ICommand RefreshListCommand { get; } = new ActionCommand(() => throw new NotImplementedException());

    public IJoystickStateViewModel State { get; } = new JoystickStateViewModel();
    public JoystickIntervalViewModel Interval { get; } = new();

    public int Count => _model?.Count ?? 0;
    
    public JoystickSourceViewModel() { }

    public JoystickSourceViewModel(IJoystickSourceModel model, IJoystickStateViewModel state, JoystickIntervalViewModel interval)
    {
        _model = model;
        State = state;
        Interval = interval;

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
        _model?.RefreshList();

        AvailableJoysticks = _model?.Joysticks ?? new List<string>();
        OnPropertyChanged(nameof(Count));
    }
}
