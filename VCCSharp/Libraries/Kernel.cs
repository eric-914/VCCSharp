using VCCSharp.Models;
using HMODULE = System.IntPtr;
using HANDLE = System.IntPtr;

namespace VCCSharp.Libraries
{
    public interface IKernel
    {
        HMODULE LoadLibrary(string dllToLoad);
        bool FreeLibrary(HMODULE hModule);
        ushort GetPrivateProfileIntA(string lpAppName, string lpKeyName, short nDefault, string lpFileName);
        unsafe uint GetPrivateProfileStringA(string lpAppName, string lpKeyName, string lpDefault, byte* lpReturnedString, uint nSize, string lpFileName);
        int WritePrivateProfileStringA(string lpAppName, string lpKeyName, string lpString, string lpFileName);
        unsafe int QueryPerformanceCounter(LARGE_INTEGER* lpPerformanceCount);
        unsafe int QueryPerformanceFrequency(LARGE_INTEGER* lpFrequency);
        unsafe int ReadFile(HANDLE hFile, byte* lpBuffer, uint nNumberOfBytesToRead, uint* lpNumberOfBytesRead, void* lpOverlapped);
        unsafe uint SetFilePointer(HANDLE hFile, uint lDistanceToMove, uint* lpDistanceToMoveHigh, uint dwMoveMethod);
        int FreeConsole();
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

        public int WritePrivateProfileStringA(string lpAppName, string lpKeyName, string lpString, string lpFileName)
            => KernelDll.WritePrivateProfileStringA(lpAppName, lpKeyName, lpString, lpFileName);

        public unsafe int QueryPerformanceCounter(LARGE_INTEGER* lpPerformanceCount)
            => KernelDll.QueryPerformanceCounter(lpPerformanceCount);

        public unsafe int QueryPerformanceFrequency(LARGE_INTEGER* lpFrequency)
            => KernelDll.QueryPerformanceFrequency(lpFrequency);

        public unsafe int ReadFile(HANDLE hFile, byte* lpBuffer, uint nNumberOfBytesToRead, uint* lpNumberOfBytesRead, void* lpOverlapped)
            => KernelDll.ReadFile(hFile, lpBuffer, nNumberOfBytesToRead, lpNumberOfBytesRead, lpOverlapped);

        public unsafe uint SetFilePointer(HANDLE hFile, uint lDistanceToMove, uint* lpDistanceToMoveHigh, uint dwMoveMethod)
            => KernelDll.SetFilePointer(hFile, lDistanceToMove, lpDistanceToMoveHigh, dwMoveMethod);

        public int FreeConsole()
            => KernelDll.FreeConsole();
    }
}
