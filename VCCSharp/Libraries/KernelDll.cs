using System.Runtime.InteropServices;
using HMODULE = System.IntPtr;

namespace VCCSharp.Libraries
{
    public static class KernelDll
    {
        public const string DLL = "kernel32.dll";

        [DllImport(DLL)]
        public static extern HMODULE LoadLibrary(string dllToLoad);

        [DllImport(DLL)]
        public static extern bool FreeLibrary(HMODULE hModule);
    }
}