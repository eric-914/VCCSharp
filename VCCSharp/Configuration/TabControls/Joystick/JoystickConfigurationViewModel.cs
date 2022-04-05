using System.Windows.Input;
using VCCSharp.Enums;
using VCCSharp.Models.Keyboard;
using VCCSharp.Shared.ViewModels;

namespace VCCSharp.Configuration.TabControls.Joystick;

/// <summary>
/// Contains the different input source configurations (Joystick, Mouse, Keyboard)
/// </summary>
public class JoystickConfigurationViewModel : NotifyViewModel
{
    private readonly Models.Configuration.Joystick _model = new();

    //--TODO: Getting the IDxJoystickState requires State.State
    public JoystickStateViewModel State { get; } = new();
    public JoystickSourceViewModel JoystickSource { get; } = new();

    public JoystickConfigurationViewModel() { }

    public JoystickConfigurationViewModel(JoystickSides side, Models.Configuration.Joystick model, JoystickSourceViewModel source, JoystickStateViewModel state)
    {
        _model = model;
        JoystickSource = source;
        Side = side;
        State = state;
    }

    #region Constants

    public IEnumerable<string> KeyNames => KeyScanMapper.KeyText;

    #endregion

    public JoystickSides Side { get; set; }

    public int InputSource
    {
        get => (int)_model.InputSource.Value;
        set
        {
            if ((int)_model.InputSource.Value == value) return;

            _model.InputSource.Value = (JoystickDevices)value;
            OnPropertyChanged();
        }
    }

    public JoystickEmulations Emulation
    {
        get => _model.Type.Value;
        set
        {
            if (_model.Type.Value == value) return;

            _model.Type.Value = value;
            OnPropertyChanged();
        }
    }

    #region Key Mapping

    public Key Up
    {
        get => _model.KeyMap.Up.Value;
        set
        {
            if (_model.KeyMap.Up.Value == value) return;

            _model.KeyMap.Up.Value = value;
            OnPropertyChanged();
        }
    }

    public Key Down
    {
        get => _model.KeyMap.Down.Value;
        set
        {
            if (_model.KeyMap.Down.Value == value) return;

            _model.KeyMap.Down.Value = value;
            OnPropertyChanged();
        }
    }

    public Key Left
    {
        get => _model.KeyMap.Left.Value;
        set
        {
            if (_model.KeyMap.Left.Value == value) return;

            _model.KeyMap.Left.Value = value;
            OnPropertyChanged();
        }
    }

    public Key Right
    {
        get => _model.KeyMap.Right.Value;
        set
        {
            if (_model.KeyMap.Right.Value == value) return;

            _model.KeyMap.Right.Value = value;
            OnPropertyChanged();
        }
    }

    public Key Fire1
    {
        get => _model.KeyMap.Buttons[0].Value;
        set
        {
            if (_model.KeyMap.Buttons[0].Value == value) return;

            _model.KeyMap.Buttons[0].Value = value;
            OnPropertyChanged();
        }
    }

    public Key Fire2
    {
        get => _model.KeyMap.Buttons[1].Value;
        set
        {
            if (_model.KeyMap.Buttons[1].Value == value) return;

            _model.KeyMap.Buttons[1].Value = value;
            OnPropertyChanged();
        }
    }

    #endregion
}
