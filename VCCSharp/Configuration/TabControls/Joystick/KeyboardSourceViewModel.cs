using System.Windows.Input;
using VCCSharp.Models.Configuration.Support;
using VCCSharp.Models.Keyboard;
using VCCSharp.Shared.ViewModels;

namespace VCCSharp.Configuration.TabControls.Joystick;

public class KeyboardSourceViewModel : NotifyViewModel
{
    private readonly JoystickKeyMapping _model = new();

    public KeyboardSourceViewModel() { }

    public KeyboardSourceViewModel(JoystickKeyMapping model)
    {
        _model = model;
    }

    public IEnumerable<string> KeyNames => KeyScanMapper.KeyText;

    #region Key Mapping

    public Key Up
    {
        get => _model.Up.Value;
        set
        {
            if (_model.Up.Value == value) return;

            _model.Up.Value = value;
            OnPropertyChanged();
        }
    }

    public Key Down
    {
        get => _model.Down.Value;
        set
        {
            if (_model.Down.Value == value) return;

            _model.Down.Value = value;
            OnPropertyChanged();
        }
    }

    public Key Left
    {
        get => _model.Left.Value;
        set
        {
            if (_model.Left.Value == value) return;

            _model.Left.Value = value;
            OnPropertyChanged();
        }
    }

    public Key Right
    {
        get => _model.Right.Value;
        set
        {
            if (_model.Right.Value == value) return;

            _model.Right.Value = value;
            OnPropertyChanged();
        }
    }

    public Key Fire1
    {
        get => _model.Buttons[0].Value;
        set
        {
            if (_model.Buttons[0].Value == value) return;

            _model.Buttons[0].Value = value;
            OnPropertyChanged();
        }
    }

    public Key Fire2
    {
        get => _model.Buttons[1].Value;
        set
        {
            if (_model.Buttons[1].Value == value) return;

            _model.Buttons[1].Value = value;
            OnPropertyChanged();
        }
    }

    #endregion
}
