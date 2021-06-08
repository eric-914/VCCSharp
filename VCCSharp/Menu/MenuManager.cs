using System.Collections.ObjectModel;
using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Models;

namespace VCCSharp.Menu
{
    public interface ICartridge
    {
        void Load();
        void Eject();
        void SetMenuItem(string menuName, MenuActions menuId, int type);
        void Reset();
    }

    delegate void AddMenuItem(MenuItemViewModel item);
    delegate void ClearMenuItems();

    public class MenuManager : ICartridge
    {
        private readonly IModules _modules;
        private readonly IMainMenu _menuItems;

        public MenuManager(IModules modules, IMainMenu menuItems)
        {
            _modules = modules;
            _menuItems = menuItems;
        }

        public void Load()
        {
            MessageBox.Show("MenuManager:Load");
        }

        public void Eject()
        {
            MessageBox.Show("MenuManager:Eject");
        }

        private MenuItemViewModel _parent;

        public void SetMenuItem(string menuName, MenuActions menuId, int type)
        {
            if (string.IsNullOrEmpty(menuName))
            {
                AddMenuItem separatorAdd = _menuItems.Plugins.MenuItems.Add;
                Application.Current.Dispatcher.BeginInvoke(separatorAdd, MainMenu.Separator);
                return;
            }

            var item = new MenuItemViewModel
            {
                Id = menuId,
                Header = menuName,
                Action = () => MessageBox.Show($"{menuName} Click"),
                MenuItems = new ObservableCollection<MenuItemViewModel>()
            };

            switch (type)
            {
                case Define.MENU_PARENT: //--Children expected
                    _parent = item;
                    AddMenuItem parentAdd = _menuItems.Plugins.MenuItems.Add;
                    Application.Current.Dispatcher.BeginInvoke(parentAdd, item);
                    break;

                case Define.MENU_CHILD: //--Attach to last parent
                    AddMenuItem childAdd = _parent.MenuItems.Add;
                    Application.Current.Dispatcher.BeginInvoke(childAdd, item);
                    break;

                case Define.MENU_STANDALONE: //--No children
                    AddMenuItem menuAdd = _menuItems.Plugins.MenuItems.Add;
                    Application.Current.Dispatcher.BeginInvoke(menuAdd, item);
                    break;
            }
        }

        /// <summary>
        /// Seems the plugins are completely responsible for rebuilding the cartridge menu system.
        /// </summary>
        public void Reset()
        {
            Application.Current.Dispatcher.BeginInvoke((ClearMenuItems) _menuItems.Plugins.MenuItems.Clear);
        }
    }
}
