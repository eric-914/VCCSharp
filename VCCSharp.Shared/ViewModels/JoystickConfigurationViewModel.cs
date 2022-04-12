using VCCSharp.Shared.Configuration;
using VCCSharp.Shared.Enums;

namespace VCCSharp.Shared.ViewModels;

public interface ILeftJoystickConfigurationViewModel { }
public interface IRightJoystickConfigurationViewModel { }

/// <summary>
/// Contains the different input source configurations (Joystick, Mouse, Keyboard)
/// </summary>
public class JoystickConfigurationViewModel : NotifyViewModel, ILeftJoystickConfigurationViewModel, IRightJoystickConfigurationViewModel
{
    private readonly IJoystickConfiguration? _model;

    public JoystickSourceViewModel JoystickSource { get; } = new();
    public KeyboardSourceViewModel KeyboardSource { get; } = new();

    public JoystickConfigurationViewModel() { }

    protected JoystickConfigurationViewModel(IJoystickConfiguration model, JoystickSourceViewModel joystickSource, KeyboardSourceViewModel keyboardSource)
    {
        _model = model;
        JoystickSource = joystickSource;
        KeyboardSource = keyboardSource;
    }

    public JoystickDevices InputSource
    {
        get => _model?.InputSource.Value ?? JoystickDevices.Keyboard;
        set
        {
            if (_model == null || _model.InputSource.Value == value) return;

            _model.InputSource.Value = value;
            OnPropertyChanged();
        }
    }

    public JoystickEmulations Emulation
    {
        get => _model?.Type.Value ?? JoystickEmulations.Standard;
        set
        {
            if (_model == null || _model.Type.Value == value) return;

            _model.Type.Value = value;
            OnPropertyChanged();
        }
    }
}

public class LeftJoystickConfigurationViewModel : JoystickConfigurationViewModel
{
    public LeftJoystickConfigurationViewModel(ILeftJoystickConfiguration model, ILeftJoystickSourceViewModel joystickSource, ILeftKeyboardSourceViewModel keyboardSource)
        : base(model, (JoystickSourceViewModel)joystickSource, (KeyboardSourceViewModel)keyboardSource) { }
}

public class RightJoystickConfigurationViewModel : JoystickConfigurationViewModel
{
    public RightJoystickConfigurationViewModel(IRightJoystickConfiguration model, IRightJoystickSourceViewModel joystickSource, IRightKeyboardSourceViewModel keyboardSource)
        : base(model, (JoystickSourceViewModel)joystickSource, (KeyboardSourceViewModel)keyboardSource) { }
}
