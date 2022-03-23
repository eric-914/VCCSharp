using System.Collections.Generic;
using System.Windows.Input;
using VCCSharp.Enums;
using VCCSharp.Main.ViewModels;
using VCCSharp.Models.Keyboard;

namespace VCCSharp.Configuration.TabControls.Joystick;

public class JoystickViewModel : NotifyViewModel
{
    private readonly IJoystickServices? _services;

    public Models.Configuration.Joystick Model { get; } = new();

    public JoystickViewModel() { }

    //--TODO: Holding a local copy of the correct JoystickModel* ends up with bad pointers for reason unknown
    public JoystickViewModel(JoystickSides side, Models.Configuration.Joystick model, IJoystickServices services) : this()
    {
        Side = side;
        Model = model;
        _services = services;
    }

    #region Constants

    public IEnumerable<string> KeyNames => KeyScanMapper.KeyText;

    public List<string> JoystickNames => _services?.FindJoysticks() ?? new List<string>();

    public string SideText => Side == JoystickSides.Left ? "Left" : "Right";

    #endregion
    
    public JoystickSides Side { get; set; }

    public JoystickDevices Device
    {
        get => Model.InputSource.Value;
        set
        {
            if (Model.InputSource.Value == value) return;
            
            Model.InputSource.Value = value;
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

    // Index of which Joystick is selected
    public int DeviceIndex
    {
        get => Model.DeviceIndex;
        set => Model.DeviceIndex = value;
    }
}
