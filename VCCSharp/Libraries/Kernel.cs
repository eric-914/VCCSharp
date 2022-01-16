using System;
using VCCSharp.Libraries.Models;
using VCCSharp.Models;
using HMODULE = System.IntPtr;
using HANDLE = System.IntPtr;
using static System.IntPtr;

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
        uint SetFilePointer(IntPtr hFile, uint dwMoveMethod, uint lDistanceToMove = 0);
        int FreeConsole();
        int CloseHandle(HANDLE hObject);
        IntPtr GetProcAddress(HMODULE hModule, string lpProcName);
        uint FlushFileBuffers(HANDLE hFile);
        unsafe uint ReadFile(HANDLE hFile, byte* lpBuffer, ulong nNumberOfBytesToRead, ulong* lpNumberOfBytesRead);
        HANDLE CreateFile(string filename, uint desiredAccess, uint dwCreationDisposition);
        uint WriteFile(HANDLE hFile, string lpBuffer, uint nNumberOfBytesToWrite);
        unsafe uint WriteFile(HANDLE hFile, byte* lpBuffer, uint nNumberOfBytesToWrite);
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

        public unsafe uint SetFilePointer(IntPtr hFile, uint dwMoveMethod, uint lDistanceToMove = 0)
            => KernelDll.SetFilePointer(hFile, lDistanceToMove, null, dwMoveMethod);

        public int FreeConsole()
            => KernelDll.FreeConsole();

        public int CloseHandle(HANDLE hObject)
            => KernelDll.CloseHandle(hObject);

        public IntPtr GetProcAddress(HMODULE hModule, string lpProcName)
            => KernelDll.GetProcAddress(hModule, lpProcName);

        public uint FlushFileBuffers(HANDLE hFile)
            => KernelDll.FlushFileBuffers(hFile);

        public unsafe uint ReadFile(HANDLE hFile, byte* lpBuffer, ulong nNumberOfBytesToRead, ulong* lpNumberOfBytesRead)
            => KernelDll.ReadFile(hFile, lpBuffer, nNumberOfBytesToRead, lpNumberOfBytesRead, Zero);

        public HANDLE CreateFile(string filename, uint desiredAccess, uint dwCreationDisposition)
            => KernelDll.CreateFileA(filename, desiredAccess, 0, Zero, dwCreationDisposition, Define.FILE_ATTRIBUTE_NORMAL, Zero);

        public unsafe uint WriteFile(HANDLE hFile, string lpBuffer, uint nNumberOfBytesToWrite)
        {
            ulong temp;

            return KernelDll.WriteFile(hFile, lpBuffer, nNumberOfBytesToWrite, &temp, Zero);
        }

        public unsafe uint WriteFile(HANDLE hFile, byte* lpBuffer, uint nNumberOfBytesToWrite)
        {
            return WriteFile(hFile, Converter.ToString(lpBuffer), nNumberOfBytesToWrite);
        }

    }
}
