using DX8.Tester.Model;
using System.Collections.Generic;

namespace DX8.Tester;

internal class TestWindowViewModel : NotifyViewModel
{
    private readonly TestWindowModel _model = new();

    public List<string> AvailableJoysticks { get; }
    public int Count => _model.Count;

    public IJoystickStateViewModel LeftJoystick => _model.LeftJoystick;
    public IJoystickStateViewModel RightJoystick => _model.RightJoystick;

    public TestWindowViewModel()
    {
        AvailableJoysticks = _model.FindJoysticks();
    }

    public void Refresh()
    {
        _model.Refresh();

        OnPropertyChanged(nameof(LeftJoystick));
        OnPropertyChanged(nameof(RightJoystick));
    }
}
