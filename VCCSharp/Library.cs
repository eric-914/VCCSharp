using System.Runtime.InteropServices;

using HINSTANCE = System.IntPtr;

namespace VCCSharp.library
{
    class Library
    {
        // ReSharper disable once InconsistentNaming
        private const string DLL = "library.dll";

        //extern "C" __declspec(dllexport) __cdecl int sum(int a,int b); 

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int AppRun(HINSTANCE hInstance, string lpCmdLine);
    }
}
