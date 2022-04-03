using DX8.Tester.Model;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace DX8.Tester;

internal class TestWindowViewModel : NotifyViewModel
{
    private readonly TestWindowModel _model = new();

    public int Count => _model.Count;

    public IJoystickStateViewModel LeftJoystick => _model.LeftJoystick;
    public IJoystickStateViewModel RightJoystick => _model.RightJoystick;

    public ICommand RefreshListCommand { get; }

    public TestWindowViewModel()
    {
        _model.DeviceLostEvent += (_, _) => DeviceLost();

        RefreshListCommand = new ActionCommand(RefreshList);
        RefreshList();
    }

    private void RefreshList()
    {
        AvailableJoysticks = _model.FindJoysticks();
        OnPropertyChanged(nameof(Count));
        OnPropertyChanged(nameof(LeftJoystick));
        OnPropertyChanged(nameof(RightJoystick));
    }

    private void DeviceLost()
    {
        const string message = @"
A joystick connection has been lost.
Choose OK to refresh the joystick list.
";

        MessageBox.Show(message, "Device Lost");

        RefreshList();
    }

    public int Interval
    {
        get => _model.Interval;
        set
        {
            _model.Interval = value;
            OnPropertyChanged();
        }
    }

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
}
