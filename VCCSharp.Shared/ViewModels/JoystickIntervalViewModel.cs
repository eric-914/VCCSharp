using VCCSharp.Shared.Dx;
using VCCSharp.Shared.Models;

namespace VCCSharp.Shared.ViewModels;

public class JoystickIntervalViewModel : NotifyViewModel
{
    private readonly IInterval _configuration = new NullJoysticksConfiguration();
    private readonly IDxManager? _manager;

    public JoystickIntervalViewModel() { }

    public JoystickIntervalViewModel(IInterval configuration, IDxManager manager)
    {
        _configuration = configuration;
        _manager = manager;
    }

    public int Value
    {
        get => _configuration.Interval;
        set
        {
            if (_configuration.Interval == value) return;

            _configuration.Interval = value;

            if (_manager != null)
            {
                _manager.Interval = value;
            }

            OnPropertyChanged();
        }
    }
}
