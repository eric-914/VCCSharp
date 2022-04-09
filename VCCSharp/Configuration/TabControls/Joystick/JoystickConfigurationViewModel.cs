using VCCSharp.Enums;
using VCCSharp.Shared.Enums;
using VCCSharp.Shared.ViewModels;

namespace VCCSharp.Configuration.TabControls.Joystick;

/// <summary>
/// Contains the different input source configurations (Joystick, Mouse, Keyboard)
/// </summary>
public class JoystickConfigurationViewModel : NotifyViewModel
{
    private readonly Models.Configuration.Joystick _model = new();

    public JoystickSourceViewModel JoystickSource { get; } = new();
    public KeyboardSourceViewModel KeyboardSource { get; } = new();

    public JoystickConfigurationViewModel() { }

    public JoystickConfigurationViewModel(JoystickSides side, Models.Configuration.Joystick model, JoystickSourceViewModel joystickSource, KeyboardSourceViewModel keyboardSource)
    {
        _model = model;
        JoystickSource = joystickSource;
        KeyboardSource = keyboardSource;
        Side = side;
    }

    #region Constants


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

}
