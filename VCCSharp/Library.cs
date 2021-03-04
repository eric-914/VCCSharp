using System.Runtime.InteropServices;
using VCCSharp.Models;
using HINSTANCE = System.IntPtr;
using HMODULE = System.IntPtr;
using INT = System.Int32;
using PSTR = System.String;
//using LRESULT = System.Int64;

namespace VCCSharp
{
    public static class Library
    {
        // ReSharper disable once InconsistentNaming
        public const string DLL = "library.dll";

        public static class Vcc
        {
            [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern void VccStartup(HINSTANCE hInstance, ref CmdLineArguments cmdLineArgs, ref EmuState emu);

            [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern void VccRun();

            [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern INT VccShutdown();

            [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern INT VccStartupThreading();

            [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern void CheckQuickLoad(string qLoadFile);
        }

        public static class CoCo
        {
            [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern void SetClockSpeed(ushort cycles);
        }

        public static class DirectDraw
        {
            [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern bool InitDirectDraw(HINSTANCE hInstance, HINSTANCE resources);
        }

        [DllImport("kernel32.dll")]
        public static extern HMODULE LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(HMODULE hModule);
    }
}
