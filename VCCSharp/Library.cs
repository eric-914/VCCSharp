using System.Runtime.InteropServices;
using VCCSharp.Models;
using HINSTANCE = System.IntPtr;
using HMODULE = System.IntPtr;
using INT = System.Int32;

namespace VCCSharp
{
    public static class Library
    {
        // ReSharper disable once InconsistentNaming
        public const string DLL = "library.dll";

        public static class Emu
        {
            [DllImport(DLL)]
            public static extern unsafe EmuState *GetEmuState();

            [DllImport(DLL)]
            public static extern unsafe void SetEmuState(EmuState *emuState);
        }

        public static class Vcc
        {
            [DllImport(DLL)]
            public static extern unsafe VccState *GetVccState();

            [DllImport(DLL)]
            public static extern void VccRun();

            [DllImport(DLL)]
            public static extern unsafe INT VccShutdown(EmuState *emuState);

            [DllImport(DLL)]
            public static extern INT VccStartupThreading();

            [DllImport(DLL)]
            public static extern void SetAppTitle(HINSTANCE hResources, string binFileName);

            [DllImport(DLL)]
            public static extern void CreatePrimaryWindow();

            [DllImport(DLL)]
            public static extern void CheckScreenModeChange();
        }

        public static class CoCo
        {
            [DllImport(DLL)]
            public static extern void SetClockSpeed(ushort cycles);
        }

        public static class Config
        {
            [DllImport(DLL)]
            public static extern unsafe void InitConfig(EmuState *emu, ref CmdLineArguments cmdLineArgs);
        }

        public static class DirectDraw
        {
            [DllImport(DLL)]
            public static extern bool InitDirectDraw(HINSTANCE hInstance, HINSTANCE resources);

            [DllImport(DLL)]
            public static extern void ClearScreen();
        }

        public static class MenuCallbacks
        {
            [DllImport(DLL)]
            public static extern unsafe void DynamicMenuCallback(EmuState *emu, string menuName, int menuId, int type);
        }

        public static class QuickLoad
        {
            [DllImport(DLL)]
            public static extern unsafe int QuickStart(EmuState *emu, string binFileName);
        }

        [DllImport("kernel32.dll")]
        public static extern HMODULE LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(HMODULE hModule);
    }
}
