using System.Windows.Input;
using VCCSharp.Enums;
using VCCSharp.Models.Keyboard;
using VCCSharp.Modules;
using VCCSharp.Shared.Commands;
using VCCSharp.Shared.ViewModels;

namespace VCCSharp.Configuration.TabControls.Joystick;

public class JoystickViewModel : NotifyViewModel
{
    private readonly IJoysticks? _module;

    public Models.Configuration.Joystick Model { get; } = new();

    //--TODO: Getting the IDxJoystickState requires State.State
    public JoystickStateViewModel State { get; } = new();

    public ICommand RefreshListCommand { get; } = new ActionCommand(() => throw new NotImplementedException());

    public JoystickViewModel() { }

    public JoystickViewModel(JoystickSides side, Models.Configuration.Joystick model, IJoysticks module, JoystickStateViewModel state)
    {
        _module = module;
        Side = side;
        Model = model;
        State = state;

        _module.FindJoysticks();

        RefreshListCommand = new ActionCommand(RefreshList);
        RefreshList();
    }

    #region Constants

    public IEnumerable<string> KeyNames => KeyScanMapper.KeyText;

    public string SideText => Side == JoystickSides.Left ? "Left" : "Right";

    #endregion

    public JoystickSides Side { get; set; }

    public int Device
    {
        get => (int)Model.InputSource.Value;
        set
        {
            if ((int)Model.InputSource.Value == value) return;

            Model.InputSource.Value = (JoystickDevices)value;
            OnPropertyChanged();
        }
    }

    public JoystickEmulations Emulation
    {
        get => Model.Type.Value;
        set
        {
            if (Model.Type.Value == value) return;

            Model.Type.Value = value;
            OnPropertyChanged();
        }
    }

    #region Key Mapping

    public Key Up
    {
        get => Model.KeyMap.Up.Value;
        set
        {
            if (Model.KeyMap.Up.Value == value) return;

            Model.KeyMap.Up.Value = value;
            OnPropertyChanged();
        }
    }

    public Key Down
    {
        get => Model.KeyMap.Down.Value;
        set
        {
            if (Model.KeyMap.Down.Value == value) return;

            Model.KeyMap.Down.Value = value;
            OnPropertyChanged();
        }
    }

    public Key Left
    {
        get => Model.KeyMap.Left.Value;
        set
        {
            if (Model.KeyMap.Left.Value == value) return;

            Model.KeyMap.Left.Value = value;
            OnPropertyChanged();
        }
    }

    public Key Right
    {
        get => Model.KeyMap.Right.Value;
        set
        {
            if (Model.KeyMap.Right.Value == value) return;

            Model.KeyMap.Right.Value = value;
            OnPropertyChanged();
        }
    }

    public Key Fire1
    {
        get => Model.KeyMap.Buttons[0].Value;
        set
        {
            if (Model.KeyMap.Buttons[0].Value == value) return;

            Model.KeyMap.Buttons[0].Value = value;
            OnPropertyChanged();
        }
    }

    public Key Fire2
    {
        get => Model.KeyMap.Buttons[1].Value;
        set
        {
            if (Model.KeyMap.Buttons[1].Value == value) return;

            Model.KeyMap.Buttons[1].Value = value;
            OnPropertyChanged();
        }
    }

    #endregion

    // Index of which Joystick is selected
    public int DeviceIndex
    {
        get => Model.DeviceIndex;
        set => Model.DeviceIndex = value;
    }

    public List<string> JoystickNames => _module?.JoystickList.Select(x => x.Device.InstanceName).ToList() ?? new List<string>();

    public void RefreshList()
    {
        if (_module != null)
        {
            _module.FindJoysticks();

            OnPropertyChanged(nameof(JoystickNames));
        }
    }
}
