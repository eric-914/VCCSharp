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

        public void DynamicMenuCallback(string menuName, MenuActions menuId, int type)
        {
            Library.MenuCallbacks.DynamicMenuCallback(menuName, (int)menuId, type);
        }

        public int DynamicMenuActivated(byte emulationRunning, int menuItem)
        {
            switch (menuItem)
            {
                case Define.ID_MENU_LOAD_CART:
                    return LoadPack();

                case Define.ID_MENU_EJECT_CART:
                    return UnloadPack(emulationRunning);

                default:
                    if (_modules.PAKInterface.HasConfigModule() != 0)
                    {
                        //--Original code was passing an unsigned char, though the menu ids are integers
                        _modules.PAKInterface.InvokeConfigModule((byte)(menuItem - Define.ID_DYNAMENU_START));
                    }
                    return 0;
            }
        }

        public void SetWindowHandle(IntPtr intPtr)
        {
            Library.MenuCallbacks.SetWindowHandle(intPtr);
        }

        public int LoadPack()
        {
            unsafe
            {
                VccState* vccState = _modules.Vcc.GetVccState();

                if (vccState->DialogOpen != 0)
                {
                    return 0;
                }

                vccState->DialogOpen = Define.TRUE;
                int result = OpenLoadCartFileDialog();
                vccState->DialogOpen = Define.FALSE;

                _modules.Emu.EmulationRunning = Define.TRUE;

                return result;
            }
        }

        public int UnloadPack(byte emulationRunning)
        {
            return Library.MenuCallbacks.UnloadPack(emulationRunning);
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
    }
}
