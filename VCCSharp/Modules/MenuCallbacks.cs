﻿using System.IO;
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
        void RefreshDynamicMenu();
        void BuildCartridgeMenu(string menuName, int menuId, int type);
        void BuildCartridgeMenu(string menuName, MenuActions menuId, int type);
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
            return DynamicMenuActivated(emulationRunning, (MenuActions) menuItem);
        }

        public int DynamicMenuActivated(byte emulationRunning, MenuActions menuItem)
        {
            switch (menuItem)
            {
                case MenuActions.Load:
                    return LoadPack();

                case MenuActions.Eject: 
                    return _modules.PAKInterface.UnloadPack(emulationRunning != Define.FALSE);

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
        private int LoadPack()
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

        //--Used by PAK plugins
        public void BuildCartridgeMenu(string menuName, int menuId, int type)
        {
            BuildCartridgeMenu(menuName, (MenuActions)menuId, type);
        }

        public void BuildCartridgeMenu(string menuName, MenuActions menuId, int type)
        {
            switch (menuId)
            {
                case MenuActions.Flush:
                    _menuIndex = 0;
                    BuildCartridgeMenu("Cartridge", MenuActions.Cartridge, Define.MENU_PARENT);	//Recursion is fun
                    BuildCartridgeMenu("Load Cart", MenuActions.Load, Define.MENU_CHILD);
                    BuildCartridgeMenu($"Eject Cart: {_modules.PAKInterface.ModuleName}", MenuActions.Eject, Define.MENU_CHILD);

                    break;

                case MenuActions.Done:
                    DrawDynamicMenu();
                    break;

                //--Used by plug-ins to add whatever they want
                default:
                    SetMenuItem(_menuIndex++, menuName, (int)menuId, type);
                    break;
            }
        }

        public void RefreshDynamicMenu()
        {
            BuildCartridgeMenu(null, MenuActions.Flush, Define.IGNORE);
            BuildCartridgeMenu(null, MenuActions.Done, Define.IGNORE);
        }

        private void DrawDynamicMenu()
        {
            HWND hWnd = _modules.Emu.WindowHandle;

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

            for (int tempIndex = 0; tempIndex < _menuIndex; tempIndex++)
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

        private void SetMenuRoot(HMENU hMenu)
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

        private void SetMenuParent(DMenu menuItem, int subMenuIndex)
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

        private void SetMenuChild(DMenu menuItem, int subMenuIndex)
        {
            HMENU menu = _hSubMenu[subMenuIndex];

            MENUITEMINFO mii = SetMenuText(menuItem.MenuName);
            mii.fMask = Define.MIIM_TYPE | Define.MIIM_ID;
            mii.wID = (uint)menuItem.MenuId;
            mii.hSubMenu = menu;

            MenuInsertMenuItem(menu, mii, 0, Define.FALSE);
        }

        private void SetMenuStandalone(DMenu menuItem)
        {
            HMENU menu = _hSubMenu[0];

            MENUITEMINFO mii = SetMenuText(menuItem.MenuName);
            mii.fMask = Define.MIIM_TYPE | Define.MIIM_ID;
            mii.wID = (uint)menuItem.MenuId;
            mii.hSubMenu = _hMenu;

            MenuInsertMenuItem(menu, mii, 0, Define.FALSE);
        }

        private static unsafe MENUITEMINFO SetMenuText(string text)
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

        private void SetMenuItem(byte menuIndex, string menuName, int menuId, int type)
        {
            _menuItem[menuIndex].MenuName = menuName;
            _menuItem[menuIndex].MenuId = menuId;
            _menuItem[menuIndex].Type = type;
        }

        private static HMENU MenuGetMenu(HWND hWnd)
        {
            return Library.MenuCallbacks.MenuGetMenu(hWnd);
        }

        private static void MenuDeleteMenu(HWND hMenu, uint uPosition, uint uFlags)
        {
            Library.MenuCallbacks.MenuDeleteMenu(hMenu, uPosition, uFlags);
        }

        private static HMENU MenuCreatePopupMenu()
        {
            return Library.MenuCallbacks.MenuCreatePopupMenu();
        }

        private static void MenuDrawMenuBar(HWND hWnd)
        {
            Library.MenuCallbacks.MenuDrawMenuBar(hWnd);
        }

        private static unsafe void MenuInsertMenuItem(HMENU hMenu, MENUITEMINFO mii, uint item, int fByPosition)
        {
            Library.MenuCallbacks.MenuInsertMenuItem(hMenu, &mii, item, fByPosition);
        }
    }
}
