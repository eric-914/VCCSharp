using System.Runtime.InteropServices;
using HINSTANCE = System.IntPtr;
using INT = System.Int32;
using PSTR = System.String;
//using LRESULT = System.Int64;

namespace VCCSharp
{
    public static class Library
    {
        // ReSharper disable once InconsistentNaming
        private const string DLL = "library.dll";

        public static class Vcc
        {
            [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern void VccStartup(HINSTANCE hInstance, CmdLineArguments cmdLineArgs);

            [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern void VccRun();

            [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern INT VccShutdown();

            [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern CmdLineArguments GetCmdLineArgs(PSTR lpCmdLine);
        }
    }
}
