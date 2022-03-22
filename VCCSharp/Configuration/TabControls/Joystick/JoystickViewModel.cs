using System;
using System.Collections.Generic;
using System.Windows.Input;
using VCCSharp.Enums;
using VCCSharp.Main.ViewModels;
using VCCSharp.Models.Configuration;
using VCCSharp.Models.Keyboard;

namespace VCCSharp.Configuration.TabControls.Joystick;

public class JoystickViewModel : NotifyViewModel
{
    private readonly Joysticks? _joysticks;
    private readonly IJoystickServices? _services;

    public JoystickViewModel() { }

    //--TODO: Holding a local copy of the correct JoystickModel* ends up with bad pointers for reason unknown
    public JoystickViewModel(JoystickSides side, Joysticks joysticks, IJoystickServices services) : this()
    {
        Side = side;
        _joysticks = joysticks;
        _services = services;
    }

    public Models.Configuration.Joystick? Model => _joysticks == null ? null : Side == JoystickSides.Left ? _joysticks.Left : _joysticks.Right;

    #region Constants

    public IEnumerable<string> KeyNames => KeyScanMapper.KeyText;

    public List<string> JoystickNames => _services?.FindJoysticks() ?? new List<string>();

    #endregion

    public string SideText => Side == JoystickSides.Left ? "Left" : "Right";

    public JoystickSides Side { get; set; }

    public JoystickDevices? Device
    {
        get => UseMouse;
        set
        {
            if (value.HasValue)
            {
                UseMouse = value.Value;
                OnPropertyChanged();
            }
        }
    }

    public JoystickDevices UseMouse
    {
        get => Model?.InputSource.Value ?? JoystickDevices.Mouse;
        set
        {
            if (Model == null) { throw new Exception("Model not defined"); }
            Model.InputSource.Value = value;
        }
    }

    public JoystickEmulations? Emulation
    {
        get => HiRes;
        set
        {
            if (value.HasValue)
            {
                HiRes = value.Value;
                OnPropertyChanged();
            }
        }
    }

    public JoystickEmulations HiRes
    {
        get => Model?.Type.Value ?? JoystickEmulations.Standard;
        set
        {
            if (Model == null) { throw new Exception("Model not defined"); }
            Model.Type.Value = value;
        }
    }

    // Index of which Joystick is selected
    public int DiDevice { get; set; } = 0;

    public Key Up
    {
        get => Model?.KeyMap.Up.Value ?? Key.None;
        set
        {
            if (Model == null) { throw new Exception("Model not defined"); }
            if (Model.KeyMap.Up.Value == value) return;

            Model.KeyMap.Up.Value = value;
            OnPropertyChanged();
        }
    }

    public Key Down
    {
        get => Model?.KeyMap.Down.Value ?? Key.None;
        set
        {
            if (Model == null) { throw new Exception("Model not defined"); }
            if (Model.KeyMap.Down.Value == value) return;

            Model.KeyMap.Down.Value = value;
            OnPropertyChanged();
        }
    }

    public Key Left
    {
        get => Model?.KeyMap.Left.Value ?? Key.None;
        set
        {
            if (Model == null) { throw new Exception("Model not defined"); }
            if (Model.KeyMap.Left.Value == value) return;

            Model.KeyMap.Left.Value = value;
            OnPropertyChanged();
        }
    }

    public Key Right
    {
        get => Model?.KeyMap.Right.Value ?? Key.None;
        set
        {
            if (Model == null) { throw new Exception("Model not defined"); }
            if (Model.KeyMap.Right.Value == value) return;

            Model.KeyMap.Right.Value = value;
            OnPropertyChanged();
        }
    }

    public Key Fire1
    {
        get => Model?.KeyMap.Buttons[0].Value ?? Key.None;
        set
        {
            if (Model == null) { throw new Exception("Model not defined"); }
            if (Model.KeyMap.Buttons[0].Value == value) return;

            Model.KeyMap.Buttons[0].Value = value;
            OnPropertyChanged();
        }
    }

    public Key Fire2
    {
        get => Model?.KeyMap.Buttons[1].Value ?? Key.None;
        set
        {
            if (Model == null) { throw new Exception("Model not defined"); }
            if (Model.KeyMap.Buttons[1].Value == value) return;

            Model.KeyMap.Buttons[1].Value = value;
            OnPropertyChanged();
        }
    }
}
