using System.Windows.Input;
using VCCSharp.Shared.Commands;
using VCCSharp.Shared.Models;

namespace VCCSharp.Shared.ViewModels;

public class JoystickSourceViewModel : NotifyViewModel, ILeft<JoystickSourceViewModel>, IRight<JoystickSourceViewModel>
{
    private readonly JoystickSourceModel? _model;

    public ICommand RefreshListCommand { get; } = new ActionCommand(() => throw new NotImplementedException());

    public JoystickStateViewModel State { get; } = new();
    public JoystickIntervalViewModel Interval { get; } = new();

    public int Count => _model?.Count ?? 0;
    
    public JoystickSourceViewModel() { }

    protected JoystickSourceViewModel(JoystickSourceModel model, JoystickStateViewModel state, JoystickIntervalViewModel interval)
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

public class LeftJoystickSourceViewModel : JoystickSourceViewModel
{
    public LeftJoystickSourceViewModel(ILeft<JoystickSourceModel> model, ILeft<JoystickStateViewModel> state, JoystickIntervalViewModel interval)
        : base((JoystickSourceModel)model, (JoystickStateViewModel)state, interval) { }
}

public class RightJoystickSourceViewModel : JoystickSourceViewModel
{
    public RightJoystickSourceViewModel(IRight<JoystickSourceModel> model, IRight<JoystickStateViewModel> state, JoystickIntervalViewModel interval)
        : base((JoystickSourceModel)model, (JoystickStateViewModel)state, interval) { }
}
