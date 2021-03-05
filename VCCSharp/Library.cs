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
            public static extern unsafe void VccStartup(HINSTANCE hInstance, ref CmdLineArguments cmdLineArgs, EmuState *emu);

            [DllImport(DLL)]
            public static extern void VccRun();

            [DllImport(DLL)]
            public static extern INT VccShutdown();

            [DllImport(DLL)]
            public static extern INT VccStartupThreading();

            [DllImport(DLL)]
            public static extern void CheckQuickLoad(string qLoadFile);

            [DllImport(DLL)]
            public static extern void CreatePrimaryWindow();
        }

        public static class CoCo
        {
            [DllImport(DLL)]
            public static extern void SetClockSpeed(ushort cycles);
        }

        public static class Config
        {
            //void __cdecl InitConfig(EmuState* emuState, CmdLineArguments* cmdArg)
            [DllImport(DLL)]
            public static extern unsafe void InitConfig(ref CmdLineArguments cmdLineArgs, EmuState *emu);
        }

        public static class DirectDraw
        {
            [DllImport(DLL)]
            public static extern bool InitDirectDraw(HINSTANCE hInstance, HINSTANCE resources);
        }

        [DllImport("kernel32.dll")]
        public static extern HMODULE LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(HMODULE hModule);
    }
}
