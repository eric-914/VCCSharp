using System.Windows;
using System.Windows.Input;
using DX8.Tester.Model;
using VCCSharp.Shared.Commands;
using VCCSharp.Shared.ViewModels;

namespace DX8.Tester;

internal class TestWindowViewModel : NotifyViewModel
{
    private readonly TestWindowModel? _model;

    public int Count => _model?.Count ?? 0;

    public JoystickPairViewModel Joysticks { get; } = new();

    public IJoystickStateViewModel LeftJoystick => _model?.LeftJoystick ?? new JoystickStateViewModel();
    public IJoystickStateViewModel RightJoystick => _model?.RightJoystick ?? new JoystickStateViewModel();

    public ICommand RefreshListCommand { get; } = new ActionCommand();

    public TestWindowViewModel() { }

    public TestWindowViewModel(TestWindowModel model, JoystickPairViewModel joysticks)
    {
        _model = model;
        _model.DeviceLostEvent += (_, _) => DeviceLost();

        Joysticks = joysticks;
        RefreshListCommand = new ActionCommand(RefreshList);
        RefreshList();
    }

    private void RefreshList()
    {
        AvailableJoysticks = _model?.FindJoysticks() ?? new List<string>();
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
        get => _model?.Interval ?? 0;
        set
        {
            if (_model == null) return;

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
