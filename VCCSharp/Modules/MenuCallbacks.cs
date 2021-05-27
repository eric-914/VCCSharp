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
            Library.MenuCallbacks.DynamicMenuCallback(menuName, (int)menuId, type);
        }

        public void SetWindowHandle(IntPtr intPtr)
        {
            Library.MenuCallbacks.SetWindowHandle(intPtr);
        }
    }
}
