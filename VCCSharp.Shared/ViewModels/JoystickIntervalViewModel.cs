using VCCSharp.Configuration.Models;
using VCCSharp.Shared.Dx;

namespace VCCSharp.Shared.ViewModels;

public class JoystickIntervalViewModel : NotifyViewModel
{
    private readonly IInterval? _configuration;
    private readonly IDxManager? _manager;

    public JoystickIntervalViewModel() { }

    public JoystickIntervalViewModel(IInterval configuration, IDxManager manager)
    {
        _configuration = configuration;
        _manager = manager;
    }

    public int Value
    {
        get => _configuration?.Interval ?? 0;
        set
        {
            if (_configuration == null || _configuration.Interval == value) return;

            _configuration.Interval = value;

            if (_manager != null)
            {
                _manager.Interval = value;
            }

            OnPropertyChanged();
        }
    }
}
