using System.Collections.ObjectModel;
using System.Windows.Input;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Shared.ViewModels;

namespace VCCSharp.Menu;

public class MenuItemViewModel : NotifyViewModel
{
    public MenuActions Id { get; set; } = MenuActions.Done;

    private readonly IViewModelFactory _factory;

    public MenuItemViewModel()
    {
        _factory = Factory.Instance.Get<IViewModelFactory>();

        Command = _factory.CreateCommandViewModel();
    }

    private string? _header;
    public string? Header
    {
        get => _header;
        set
        {
            if (_header == value) return;

            _header = value; 
            OnPropertyChanged();
        }
    }

    public ICommand Command { get; set; }
    public bool IsCheckable { get; set; } = false;

    public bool IsSeparator => string.IsNullOrEmpty(Header);
    public bool IsHeader => !IsSeparator;

    private Action _action = () => throw new NotImplementedException();
    public Action Action
    {
        get => _action;
        set
        {
            _action = value;
            Command = _factory.CreateCommandViewModel(value);
        }
    }

    public ObservableCollection<MenuItemViewModel> MenuItems { get; set; } = new();
}

public class SeparatorItemViewModel : MenuItemViewModel
{
    public SeparatorItemViewModel()
    {
        Header = null;
    }
}