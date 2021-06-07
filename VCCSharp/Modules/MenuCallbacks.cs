using System.IO;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using HWND = System.IntPtr;
using HMENU = System.IntPtr;
using static System.IntPtr;

namespace VCCSharp.Modules
{
    public interface IMenuCallbacks
    {
        void DynamicMenuCallback(string menuName, int menuId, int type);
        void DynamicMenuCallback(string menuName, MenuActions menuId, int type);
        int DynamicMenuActivated(byte emulationRunning, int menuItem);
    }

    public class MenuCallbacks : IMenuCallbacks
    {
        private readonly IModules _modules;

        private byte _menuIndex;

        private readonly DMenu[] _menuItem = new DMenu[100];
        private readonly HMENU[] _hSubMenu = new HMENU[64];

        private HWND _hOld = Zero;
        private HMENU _hMenu = Zero;

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
                    if (_modules.PAKInterface.HasConfigModule())
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

        //--Used by PAK plugins
        public void DynamicMenuCallback(string menuName, int menuId, int type)
        {
            DynamicMenuCallback(menuName, (MenuActions)menuId, type);
        }

        public void DynamicMenuCallback(string menuName, MenuActions menuId, int type)
        {
            string temp = "Eject Cart: ";

            //MenuId=0 Flush Buffer MenuId=1 Done 
            switch (menuId)
            {
                case MenuActions.Flush:
                    _menuIndex = 0;

                    DynamicMenuCallback("Cartridge", MenuActions.Cartridge, Define.MENU_PARENT);	//Recursion is fun
                    DynamicMenuCallback("Load Cart", MenuActions.Load, Define.MENU_CHILD);

                    temp += _modules.PAKInterface.ModuleName;

                    DynamicMenuCallback(temp, MenuActions.Eject, Define.MENU_CHILD);

                    break;

                case MenuActions.Done:
                    RefreshDynamicMenu(_modules.Emu.WindowHandle, _menuIndex);
                    break;

                case MenuActions.Refresh:
                    DynamicMenuCallback(null, MenuActions.Flush, Define.IGNORE);
                    DynamicMenuCallback(null, MenuActions.Done, Define.IGNORE);
                    break;

                default:
                    SetMenuItem(_menuIndex, menuName, (int)menuId, type);

                    _menuIndex++;

                    break;
            }
        }

        public void RefreshDynamicMenu(HWND hWnd, byte menuIndex)
        {
            if (_hMenu == Zero || hWnd != _hOld)
            {
                _hMenu = MenuGetMenu(hWnd);
            }
            else
            {
                MenuDeleteMenu(_hMenu, 1, (uint)Define.MF_BYPOSITION);
            }

            _hOld = hWnd;

            SetMenuRoot(_hMenu);

            int subMenuIndex = 1;

            for (int tempIndex = 0; tempIndex < menuIndex; tempIndex++)
            {
                string menuName = _menuItem[tempIndex].MenuName;

                if (string.IsNullOrEmpty(menuName))
                {
                    _menuItem[tempIndex].Type = Define.MENU_STANDALONE;
                }

                //Create Menu item in title bar if no exist already
                switch (_menuItem[tempIndex].Type)
                {
                    case Define.MENU_PARENT:
                        SetMenuParent(_menuItem[tempIndex], ++subMenuIndex);
                        break;

                    case Define.MENU_CHILD:
                        SetMenuChild(_menuItem[tempIndex], subMenuIndex);
                        break;

                    case Define.MENU_STANDALONE:
                        SetMenuStandalone(_menuItem[tempIndex]);
                        break;
                }
            }

            MenuDrawMenuBar(hWnd);
        }

        public void SetMenuRoot(HMENU hMenu)
        {
            const string menuTitle = "Cartridge";

            HMENU menu = MenuCreatePopupMenu();

            _hSubMenu[0] = menu;

            MENUITEMINFO mii = SetMenuText(menuTitle);
            mii.fMask = Define.MIIM_TYPE | Define.MIIM_SUBMENU | Define.MIIM_ID;
            mii.wID = 4999;
            mii.hSubMenu = menu;

            MenuInsertMenuItem(hMenu, mii, 3, Define.TRUE);
        }

        public void SetMenuParent(DMenu menuItem, int subMenuIndex)
        {
            HMENU menu = MenuCreatePopupMenu();

            _hSubMenu[subMenuIndex] = menu;

            MENUITEMINFO mii = SetMenuText(menuItem.MenuName);
            mii.fMask = Define.MIIM_TYPE | Define.MIIM_SUBMENU | Define.MIIM_ID;
            mii.wID = (uint)menuItem.MenuId;
            mii.hSubMenu = menu;

            HMENU root = _hSubMenu[0];

            MenuInsertMenuItem(root, mii, 0, Define.FALSE);
        }

        public void SetMenuChild(DMenu menuItem, int subMenuIndex)
        {
            HMENU menu = _hSubMenu[subMenuIndex];

            MENUITEMINFO mii = SetMenuText(menuItem.MenuName);
            mii.fMask = Define.MIIM_TYPE | Define.MIIM_ID;
            mii.wID = (uint)menuItem.MenuId;
            mii.hSubMenu = menu;

            MenuInsertMenuItem(menu, mii, 0, Define.FALSE);
        }

        public void SetMenuStandalone(DMenu menuItem)
        {
            HMENU menu = _hSubMenu[0];

            MENUITEMINFO mii = SetMenuText(menuItem.MenuName);
            mii.fMask = Define.MIIM_TYPE | Define.MIIM_ID;
            mii.wID = (uint)menuItem.MenuId;
            mii.hSubMenu = _hMenu;

            MenuInsertMenuItem(menu, mii, 0, Define.FALSE);
        }

        public unsafe MENUITEMINFO SetMenuText(string text)
        {
            fixed (byte* p = Converter.ToByteArray(text))
            {
                return new MENUITEMINFO
                {
                    cbSize = (uint)sizeof(MENUITEMINFO),
                    fType = (uint)(string.IsNullOrEmpty(text) ? Define.MF_SEPARATOR : Define.MFT_STRING),
                    cch = (uint)(text?.Length ?? 0),
                    dwTypeData = p
                };
            }
        }

        public void SetMenuItem(byte menuIndex, string menuName, int menuId, int type)
        {
            _menuItem[menuIndex].MenuName = menuName;
            _menuItem[menuIndex].MenuId = menuId;
            _menuItem[menuIndex].Type = type;
        }

        public HMENU MenuGetMenu(HWND hWnd)
        {
            return Library.MenuCallbacks.MenuGetMenu(hWnd);
        }

        public uint MenuDeleteMenu(HMENU hMenu, uint uPosition, uint uFlags)
        {
            return Library.MenuCallbacks.MenuDeleteMenu(hMenu, uPosition, uFlags);
        }

        public HMENU MenuCreatePopupMenu()
        {
            return Library.MenuCallbacks.MenuCreatePopupMenu();
        }

        public uint MenuDrawMenuBar(HWND hWnd)
        {
            return Library.MenuCallbacks.MenuDrawMenuBar(hWnd);
        }

        public unsafe void MenuInsertMenuItem(HMENU hMenu, MENUITEMINFO mii, uint item, int fByPosition)
        {
            Library.MenuCallbacks.MenuInsertMenuItem(hMenu, &mii, item, fByPosition);
        }
    }
}
