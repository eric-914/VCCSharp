using System.Linq;
using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;

namespace VCCSharp.Menu
{
    public interface ICartridge
    {
        void Load();
        void Eject();
        void SetCartridgeTitle(string text);
        void SetMenuItem(int menuIndex, string menuName, int menuId, int type);
    }

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

        public void SetCartridgeTitle(string text)
        {
            var eject = Cartridge.MenuItems.First(x => x.Id == MenuActions.Eject);

            eject.Header = $"Eject Cart: {text}";
        }

        public void SetMenuItem(int menuIndex, string menuName, int menuId, int type)
        {
            MessageBox.Show($"MenuManager:SetMenuItem({menuIndex},{menuName},{menuId},{type})");
        }

        private MenuItemViewModel Cartridge => _menuItems.Plugins.MenuItems.First();
    }
}
