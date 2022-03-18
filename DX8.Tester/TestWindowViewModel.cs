using DX8.Tester.Model;
using System.Collections.Generic;

namespace DX8.Tester;

internal class TestWindowViewModel : NotifyViewModel
{
    private readonly TestWindowModel _model = new();

    public List<string> Joysticks { get; }

    public IDxJoystickState Joystick => _model.Joystick;

    public TestWindowViewModel()
    {
        Joysticks = _model.FindJoysticks();
    }

    public void Refresh()
    {
        _model.Refresh();

        OnPropertyChanged(nameof(Joystick));
    }
}
