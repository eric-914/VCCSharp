using System.Windows.Input;
using VCCSharp.Modules;
using VCCSharp.Shared.Commands;
using VCCSharp.Shared.ViewModels;

namespace VCCSharp.Configuration.TabControls.Joystick;

public class JoystickSourceViewModel : NotifyViewModel
{
    private readonly Models.Configuration.Joystick _model = new();
    private readonly IJoysticks? _module;

    public ICommand RefreshListCommand { get; } = new ActionCommand(() => throw new NotImplementedException());

    //--TODO: Getting the IDxJoystickState requires State.State
    public JoystickStateViewModel State { get; } = new();


    public JoystickSourceViewModel() { }

    public JoystickSourceViewModel(Models.Configuration.Joystick model, IJoysticks module, JoystickStateViewModel state)
    {
        _model = model;
        _module = module;
        State = state;

        //TODO: I don't think this belongs here.
        _module.FindJoysticks();

        RefreshListCommand = new ActionCommand(RefreshList);
        RefreshList();
    }

    // Index of which Joystick is selected
    public int DeviceIndex
    {
        get => _model.DeviceIndex;
        set => _model.DeviceIndex = value;
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
        if (_module != null)
        {
            _module.FindJoysticks();

            AvailableJoysticks = _module.JoystickList.Select(x => x.Device.InstanceName).ToList();
        }
    }
}
