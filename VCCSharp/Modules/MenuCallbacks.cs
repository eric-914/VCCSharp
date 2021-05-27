using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using HWND = System.IntPtr;

namespace VCCSharp.Modules
{
    public interface IMenuCallbacks
    {
        unsafe void DynamicMenuCallback(EmuState* emuState, string menuName, MenuActions menuId, int type);
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

        public unsafe void DynamicMenuCallback(EmuState* emuState, string menuName, MenuActions menuId, int type)
        {
            Library.MenuCallbacks.DynamicMenuCallback(emuState, menuName, (int)menuId, type);
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
                EmuState* emuState = _modules.Emu.GetEmuState();

                if (vccState->DialogOpen != 0)
                {
                    return 0;
                }

                vccState->DialogOpen = Define.TRUE;
                int result = OpenLoadCartFileDialog(emuState);
                vccState->DialogOpen = Define.FALSE;

                emuState->EmulationRunning = Define.TRUE;

                return result;
            }
        }

        public int UnloadPack(byte emulationRunning)
        {
            return Library.MenuCallbacks.UnloadPack(emulationRunning);
        }

        public unsafe int OpenLoadCartFileDialog(EmuState* emuState)
        {
            string filename = "";

            string pakPath = Converter.ToString(emuState->PakPath);

            var openFileDlg = new Microsoft.Win32.OpenFileDialog
            {
                FileName = filename,
                DefaultExt = ".txt",
                Filter = "Program Packs|*.ROM;*.ccc;*.DLL;*.pak",
                InitialDirectory = pakPath,
                CheckFileExists = true,
                ShowReadOnly = false,
                Title = "Load Program Pack"
            };

            if (openFileDlg.ShowDialog() == true)
            {
                filename = openFileDlg.FileName;

                if (InsertModule(emuState->EmulationRunning, filename) != 0)
                {
                    return 0;
                }

                //TODO: Need to set this back in EmuState
                Converter.ToByteArray(Path.GetPathRoot(filename), emuState->PakPath);

                return 1;
            }

            return 0;
        }

        public int InsertModule(byte emulationRunning, string modulePath)
        {
            return Library.MenuCallbacks.InsertModule(emulationRunning, modulePath);
        }
    }
}
