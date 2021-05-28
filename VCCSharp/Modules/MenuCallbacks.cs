using System;
using System.IO;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IMenuCallbacks
    {
        void DynamicMenuCallback(string menuName, MenuActions menuId, int type);
        int DynamicMenuActivated(byte emulationRunning, int menuItem);
        void SetWindowHandle(IntPtr intPtr);
    }

    public class MenuCallbacks : IMenuCallbacks
    {
        private readonly IModules _modules;

        public MenuCallbacks(IModules modules)
        {
            _modules = modules;
        }

        public int DynamicMenuActivated(byte emulationRunning, int menuItem)
        {
            switch (menuItem)
            {
                case Define.ID_MENU_LOAD_CART:
                    return LoadPack();

                case Define.ID_MENU_EJECT_CART:
                    return _modules.PAKInterface.UnloadPack(emulationRunning);

                default:
                    if (_modules.PAKInterface.HasConfigModule() != 0)
                    {
                        //--Original code was passing an unsigned char, though the menu ids are integers
                        _modules.PAKInterface.InvokeConfigModule((byte)(menuItem - Define.ID_DYNAMENU_START));
                    }
                    return 0;
            }
        }

        private bool _dialogOpen;
        public int LoadPack()
        {
            if (_dialogOpen)
            {
                return 0;
            }

            _dialogOpen = true;
            int result = OpenLoadCartFileDialog();
            _dialogOpen = false;

            _modules.Emu.EmulationRunning = Define.TRUE;

            return result;
        }

        public int OpenLoadCartFileDialog()
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

        public void DynamicMenuCallback(string menuName, MenuActions menuId, int type)
        {
            string temp = "Eject Cart: ";

            //MenuId=0 Flush Buffer MenuId=1 Done 
            switch (menuId)
            {
                case MenuActions.Flush:
                    SetMenuIndex(0);

                    DynamicMenuCallback("Cartridge", MenuActions.Cartridge, Define.MENU_PARENT);	//Recursion is fun
                    DynamicMenuCallback("Load Cart", MenuActions.Load, Define.MENU_CHILD);

                    unsafe
                    {
                        temp += Converter.ToString(_modules.PAKInterface.GetPakInterfaceState()->Modname);
                    }

                    DynamicMenuCallback(temp, MenuActions.Eject, Define.MENU_CHILD);

                    break;

                case MenuActions.Done:
                    RefreshDynamicMenu();
                    break;

                case MenuActions.Refresh:
                    DynamicMenuCallback(null, MenuActions.Flush, Define.IGNORE);
                    DynamicMenuCallback(null, MenuActions.Done, Define.IGNORE);
                    break;

                default:
                    SetMenuItem(menuName, (int)menuId, type);

                    SetMenuIndex((byte)(GetMenuIndex() + 1));

                    break;
            }
        }

        public void SetWindowHandle(IntPtr intPtr)
        {
            Library.MenuCallbacks.SetWindowHandle(intPtr);
        }

        public void RefreshDynamicMenu()
        {
            Library.MenuCallbacks.RefreshDynamicMenu();
        }

        public void SetMenuIndex(byte value)
        {
            Library.MenuCallbacks.SetMenuIndex(value);
        }

        public byte GetMenuIndex()
        {
            return Library.MenuCallbacks.GetMenuIndex();
        }

        public void SetMenuItem(string menuName, int menuId, int type)
        {
            Library.MenuCallbacks.SetMenuItem(menuName, menuId, type);
        }
    }
}
