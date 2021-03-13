using VCCSharp.Models;
using HMODULE = System.IntPtr;

namespace VCCSharp.Libraries
{
    public interface IKernel
    {
        HMODULE LoadLibrary(string dllToLoad);
        bool FreeLibrary(HMODULE hModule);
        ushort GetPrivateProfileIntA(string lpAppName, string lpKeyName, short nDefault, string lpFileName);
        unsafe uint GetPrivateProfileStringA(string lpAppName, string lpKeyName, string lpDefault, byte* lpReturnedString, uint nSize, string lpFileName);
        unsafe int QueryPerformanceCounter(LARGE_INTEGER* lpPerformanceCount);
    }

    public class Kernel : IKernel
    {
        public HMODULE LoadLibrary(string dllToLoad)
            => KernelDll.LoadLibrary(dllToLoad);

        public bool FreeLibrary(HMODULE hModule)
            => KernelDll.FreeLibrary(hModule);

        public ushort GetPrivateProfileIntA(string lpAppName, string lpKeyName, short nDefault, string lpFileName)
            => KernelDll.GetPrivateProfileIntA(lpAppName, lpKeyName, nDefault, lpFileName);

        public unsafe uint GetPrivateProfileStringA(string lpAppName, string lpKeyName, string lpDefault, byte* lpReturnedString, uint nSize, string lpFileName)
            => KernelDll.GetPrivateProfileStringA(lpAppName, lpKeyName, lpDefault, lpReturnedString, nSize, lpFileName);

        public unsafe int QueryPerformanceCounter(LARGE_INTEGER* lpPerformanceCount)
            => KernelDll.QueryPerformanceCounter(lpPerformanceCount);
    }
}
