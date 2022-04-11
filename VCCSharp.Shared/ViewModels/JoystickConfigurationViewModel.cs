﻿using VCCSharp.Shared.Configuration;
using VCCSharp.Shared.Enums;

namespace VCCSharp.Shared.ViewModels;

/// <summary>
/// Contains the different input source configurations (Joystick, Mouse, Keyboard)
/// </summary>
public class JoystickConfigurationViewModel : NotifyViewModel
{
    private readonly IJoystickConfiguration? _model;

    public IJoystickSourceViewModel JoystickSource { get; } = new JoystickSourceViewModel();
    public IKeyboardSourceViewModel KeyboardSource { get; } = new KeyboardSourceViewModel();
    public JoystickSides Side { get; }

    public JoystickConfigurationViewModel() { }

    public JoystickConfigurationViewModel(JoystickSides side, IJoystickConfiguration model, IJoystickSourceViewModel joystickSource, IKeyboardSourceViewModel keyboardSource)
    {
        _model = model;
        JoystickSource = joystickSource;
        KeyboardSource = keyboardSource;
        Side = side;
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
