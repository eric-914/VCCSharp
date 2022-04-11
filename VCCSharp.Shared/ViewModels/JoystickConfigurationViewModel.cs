using System.ComponentModel;
using VCCSharp.Shared.Configuration;
using VCCSharp.Shared.Enums;

namespace VCCSharp.Shared.ViewModels;

public interface IJoystickConfigurationViewModel : INotifyPropertyChanged
{
    IJoystickSourceViewModel JoystickSource { get; }
    IKeyboardSourceViewModel KeyboardSource { get; }
    JoystickDevices InputSource { get; set; }
    JoystickEmulations Emulation { get; set; }
}

public interface ILeftJoystickConfigurationViewModel : IJoystickConfigurationViewModel { }
public interface IRightJoystickConfigurationViewModel : IJoystickConfigurationViewModel { }


/// <summary>
/// Contains the different input source configurations (Joystick, Mouse, Keyboard)
/// </summary>
public class JoystickConfigurationViewModel : NotifyViewModel, ILeftJoystickConfigurationViewModel, IRightJoystickConfigurationViewModel
{
    private readonly IJoystickConfiguration? _model;

    public IJoystickSourceViewModel JoystickSource { get; } = new JoystickSourceViewModel();
    public IKeyboardSourceViewModel KeyboardSource { get; } = new KeyboardSourceViewModel();

    public JoystickConfigurationViewModel() { }

    public JoystickConfigurationViewModel(IJoystickConfiguration model, IJoystickSourceViewModel joystickSource, IKeyboardSourceViewModel keyboardSource)
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
        : base(model, joystickSource, keyboardSource) { }
}

public class RightJoystickConfigurationViewModel : JoystickConfigurationViewModel
{
    public RightJoystickConfigurationViewModel(IRightJoystickConfiguration model, IRightJoystickSourceViewModel joystickSource, IRightKeyboardSourceViewModel keyboardSource)
        : base(model, joystickSource, keyboardSource) { }
}
