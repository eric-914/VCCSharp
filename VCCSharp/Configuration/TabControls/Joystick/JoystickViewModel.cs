using DX8;
using DX8.Models;
using System.Collections.Generic;
using System.Windows.Input;
using VCCSharp.Enums;
using VCCSharp.Main.ViewModels;
using VCCSharp.Models.Keyboard;
using VCCSharp.Modules;

namespace VCCSharp.Configuration.TabControls.Joystick;

public class JoystickViewModel : NotifyViewModel
{
    private readonly IJoysticks? _module;

    public Models.Configuration.Joystick Model { get; } = new();

    public JoystickViewModel() { }

    public JoystickViewModel(JoystickSides side, Models.Configuration.Joystick model, IJoysticks module)
    {
        _module = module;
        Side = side;
        Model = model;

        _module.FindJoysticks(false);

        RefreshList(false);
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

    private IDxJoystickState _state = new NullDxJoystickState();
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

    public List<string> JoystickNames => _module?.JoystickList ?? new List<string>();

    public void Refresh()
    {
        if (_module != null)
        {
            State = _module.JoystickPoll(DeviceIndex);
        }
    }

    public void RefreshList(bool refresh)
    {
        if (_module != null)
        {
            _module.FindJoysticks(refresh);

            OnPropertyChanged(nameof(JoystickNames));
        }
    }
}
