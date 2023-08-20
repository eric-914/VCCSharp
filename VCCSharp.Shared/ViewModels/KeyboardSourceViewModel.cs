using System.Windows.Input;
using VCCSharp.Configuration.Models;
using VCCSharp.Configuration.Support;
using VCCSharp.Shared.Configuration;

namespace VCCSharp.Shared.ViewModels;

public class KeyboardSourceViewModel : NotifyViewModel, ILeft<KeyboardSourceViewModel>, IRight<KeyboardSourceViewModel>
{
    private readonly IJoystickKeyMappingConfiguration? _model;

    public KeyboardSourceViewModel() { }

    public KeyboardSourceViewModel(IJoystickKeyMappingConfiguration model)
    {
        _model = model;
    }

    public IEnumerable<string> KeyNames => _model?.KeyNames ?? new List<string>();

    #region Key Mapping

    private static Key GetKey(IKeySelectConfiguration? source) => source?.Value ?? Key.None;

    private static bool SetKey(IKeySelectConfiguration? source, Key value)
    {
        if (source == null) return false;
        source.Value = value;
        return true;
    }

    public Key Up
    {
        get => GetKey(_model?.Up);
        set { if (SetKey(_model?.Up, value)) OnPropertyChanged(); }
    }

    public Key Down
    {
        get => GetKey(_model?.Down);
        set { if (SetKey(_model?.Down, value)) OnPropertyChanged(); }
    }

    public Key Left
    {
        get => GetKey(_model?.Left);
        set { if (SetKey(_model?.Left, value)) OnPropertyChanged(); }
    }

    public Key Right
    {
        get => GetKey(_model?.Right);
        set { if (SetKey(_model?.Right, value)) OnPropertyChanged(); }
    }

    public Key Fire1
    {
        get => GetKey(_model?.Buttons[0]);
        set { if (SetKey(_model?.Buttons[0], value)) OnPropertyChanged(); }
    }

    public Key Fire2
    {
        get => GetKey(_model?.Buttons[1]);
        set { if (SetKey(_model?.Buttons[1], value)) OnPropertyChanged(); }
    }

    #endregion
}

public class LeftKeyboardSourceViewModel : KeyboardSourceViewModel
{
    public LeftKeyboardSourceViewModel(ILeft<IJoystickKeyMappingConfiguration> model) 
        : base((IJoystickKeyMappingConfiguration)model) { }
}

public class RightKeyboardSourceViewModel : KeyboardSourceViewModel
{
    public RightKeyboardSourceViewModel(IRight<IJoystickKeyMappingConfiguration> model) 
        : base((IJoystickKeyMappingConfiguration)model) { }
}
