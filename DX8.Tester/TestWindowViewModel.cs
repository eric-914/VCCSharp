using DX8.Tester.Model;
using System.Collections.Generic;

namespace DX8.Tester;

internal class TestWindowViewModel : NotifyViewModel
{
    private readonly TestWindowModel _model = new();
    
    private List<string> _availableJoysticks = new();
    public List<string> AvailableJoysticks
    {
        get => _availableJoysticks;
        private set
        {
            _availableJoysticks = value;
            OnPropertyChanged();
        }
    }

    public int Count => _model.Count;

    public IJoystickStateViewModel LeftJoystick => _model.LeftJoystick;
    public IJoystickStateViewModel RightJoystick => _model.RightJoystick;

    public TestWindowViewModel()
    {
        RefreshList();
    }

    public void RefreshList()
    {
        AvailableJoysticks = _model.FindJoysticks();
        OnPropertyChanged(nameof(LeftJoystick));
        OnPropertyChanged(nameof(RightJoystick));
    }
}
