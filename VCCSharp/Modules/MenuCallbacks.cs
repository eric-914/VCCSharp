using System.IO;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Menu;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IMenuCallbacks
    {
        void RefreshCartridgeMenu();
        void BuildCartridgeMenu(string menuName, int menuId, int type);
        void CartridgeMenuItemClicked(int menuItem);
    }

    public class MenuCallbacks : IMenuCallbacks
    {
        private readonly IModules _modules;
        private readonly ICartridge _cartridge;

        private bool _loadPakDialogOpen;

        public MenuCallbacks(IModules modules, ICartridge cartridge)
        {
            _modules = modules;
            _cartridge = cartridge;
        }

        public void CartridgeMenuItemClicked(int menuItem)
        {
            //Calls to the loaded DLL so it can do the right thing
            if (CartridgeMenuItemClicked((MenuActions) menuItem))
            {
                _modules.Emu.ResetPending = ResetPendingStates.Hard;
            }
        }

        private bool CartridgeMenuItemClicked(MenuActions menuItem)
        {
            switch (menuItem)
            {
                case MenuActions.Load:
                    return LoadPak();

                case MenuActions.Eject: 
                    return _modules.PAKInterface.UnloadPack(_modules.Emu.EmulationRunning);

                default:
                    if (_modules.PAKInterface.HasConfigModule())
                    {
                        //--Original code was passing an unsigned char, though the menu ids are integers
                        _modules.PAKInterface.InvokeConfigModule((byte)(menuItem - Define.ID_DYNAMENU_START));
                    }
                    return false;
            }
        }

        //--Used by PAK plugins
        public void BuildCartridgeMenu(string menuName, int menuId, int type)
        {
            BuildCartridgeMenu(menuName, (MenuActions)menuId, type);
        }

        private void BuildCartridgeMenu(string menuName, MenuActions menuId, int type)
        {
            switch (menuId)
            {
                case MenuActions.Flush:
                    _cartridge.Reset();

                    //Recursion is fun
                    BuildCartridgeMenu("Cartridge", MenuActions.Cartridge, Define.MENU_PARENT);	
                    BuildCartridgeMenu("Load Cart", MenuActions.Load, Define.MENU_CHILD);
                    BuildCartridgeMenu($"Eject Cart: {_modules.PAKInterface.ModuleName}", MenuActions.Eject, Define.MENU_CHILD);

                    break;

                case MenuActions.Done:
                    break;

                //--Used by plug-ins to add whatever they want
                default:
                    _cartridge.SetMenuItem(menuName, menuId, type);

                    break;
            }
        }

        public void RefreshCartridgeMenu()
        {
            BuildCartridgeMenu(null, MenuActions.Flush, Define.IGNORE);
            BuildCartridgeMenu(null, MenuActions.Done, Define.IGNORE);
        }

        private int OpenLoadCartFileDialog()
        {
            string filename = "";

            var openFileDlg = new Microsoft.Win32.OpenFileDialog
            {
                FileName = filename,
                DefaultExt = ".txt",
                Filter = "Program Packs|*.ROM;*.ccc;*.DLL;*.pak",
                InitialDirectory = _modules.Emu.PakPath,
                CheckFileExists = true,
                ShowReadOnly = false,
                Title = "Load Program Pack"
            };

            if (openFileDlg.ShowDialog() == true)
            {
                filename = openFileDlg.FileName;

                if (_modules.PAKInterface.InsertModule(_modules.Emu.EmulationRunning, filename) != 0)
                {
                    return 0;
                }

                _modules.Emu.PakPath = Path.GetPathRoot(filename);

                return 1;
            }

            return 0;
        }

        private bool LoadPak()
        {
            if (_loadPakDialogOpen)
            {
                return false;
            }

            _loadPakDialogOpen = true;
            int result = OpenLoadCartFileDialog();
            _loadPakDialogOpen = false;

            _modules.Emu.EmulationRunning = true;

            return result != 0;
        }
    }
}
