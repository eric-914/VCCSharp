using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace VCCSharp.Menu
{
    public class MenuItemViewModel
    {
        public string Header { get; set; }
        public ICommand Command { get; set; } = new CommandViewModel(() => { });
        public bool IsCheckable { get; set; } = false;

        public bool IsSeparator => string.IsNullOrEmpty(Header);
        public bool IsHeader => !IsSeparator;

        private Action _action;
        public Action Action
        {
            get => _action;
            set
            {
                _action = value;
                Command = new CommandViewModel(value);
            }
        }

        public ObservableCollection<MenuItemViewModel> MenuItems { get; set; }
    }

    public class SeparatorItemViewModel : MenuItemViewModel
    {
        public SeparatorItemViewModel()
        {
            Header = null;
        }
    }
}