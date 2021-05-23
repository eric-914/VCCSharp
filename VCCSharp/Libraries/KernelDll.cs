using System.Runtime.InteropServices;
using VCCSharp.Models;
using HMODULE = System.IntPtr;
using HANDLE = System.IntPtr;

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
        public static extern unsafe uint GetPrivateProfileStringA(string lpAppName, string lpKeyName, string lpDefault, byte* lpReturnedString, uint nSize, string lpFileName);

        [DllImport(DLL)]
        public static extern int WritePrivateProfileStringA(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        [DllImport(DLL)]
        public static extern unsafe int QueryPerformanceCounter(LARGE_INTEGER* lpPerformanceCount);

        [DllImport(DLL)]
        public static extern unsafe int QueryPerformanceFrequency(LARGE_INTEGER* lpFrequency);

        [DllImport(DLL)]
        public static extern unsafe int ReadFile(HANDLE hFile, byte* lpBuffer, uint nNumberOfBytesToRead, uint* lpNumberOfBytesRead, void* lpOverlapped);

        [DllImport(DLL)]
        public static extern unsafe uint SetFilePointer(HANDLE hFile, uint lDistanceToMove, uint* lpDistanceToMoveHigh, uint dwMoveMethod);

        [DllImport(DLL)]
        public static extern int FreeConsole();

        [DllImport(DLL)]
        public static extern int CloseHandle(HANDLE hObject);
    }
}