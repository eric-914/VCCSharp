using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using VCCSharp.Annotations;
using VCCSharp.Enums;
using VCCSharp.IoC;

namespace VCCSharp.Menu
{
    public class MenuItemViewModel : INotifyPropertyChanged
    {
        public MenuActions Id { get; set; }

        private readonly IViewModelFactory _factory;

        public MenuItemViewModel()
        {
            _factory = Factory.Instance.Get<IViewModelFactory>();

            Command = _factory.CreateCommandViewModel();
        }

        private string _header;
        public string Header
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

        private Action _action;
        public Action Action
        {
            get => _action;
            set
            {
                _action = value;
                Command = _factory.CreateCommandViewModel(value);
            }
        }

        public ObservableCollection<MenuItemViewModel> MenuItems { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class SeparatorItemViewModel : MenuItemViewModel
    {
        public SeparatorItemViewModel()
        {
            Header = null;
        }
    }
}
