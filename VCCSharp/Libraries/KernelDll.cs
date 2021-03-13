using System.Runtime.InteropServices;
using VCCSharp.Models;
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

        [DllImport(DLL)]
        public static extern ushort GetPrivateProfileIntA(string lpAppName, string lpKeyName, short nDefault, string lpFileName);

        [DllImport(DLL)]
        public static extern unsafe uint GetPrivateProfileStringA(string lpAppName, string lpKeyName, string lpDefault, byte* lpReturnedString, uint  nSize, string lpFileName);

        [DllImport(DLL)]
        public static extern unsafe int QueryPerformanceCounter(LARGE_INTEGER *lpPerformanceCount);
    }
}