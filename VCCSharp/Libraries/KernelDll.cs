using System;
using System.Runtime.InteropServices;
using VCCSharp.Libraries.Models;
using HANDLE = System.IntPtr;
using HMODULE = System.IntPtr;

namespace VCCSharp.Libraries
{
    public static class KernelDll
    {
        public const string Dll = "kernel32.dll";

        [DllImport(Dll)]
        public static extern HMODULE LoadLibrary(string dllToLoad);

        [DllImport(Dll)]
        public static extern bool FreeLibrary(HMODULE hModule);

        [DllImport(Dll)]
        public static extern ushort GetPrivateProfileIntA(string lpAppName, string lpKeyName, short nDefault, string lpFileName);

        [DllImport(Dll)]
        public static extern uint GetPrivateProfileStringA(string lpAppName, string lpKeyName, string lpDefault, byte[] lpReturnedString, uint nSize, string lpFileName);

        [DllImport(Dll)]
        public static extern int WritePrivateProfileStringA(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        [DllImport(Dll)]
        public static extern unsafe int QueryPerformanceCounter(LARGE_INTEGER* lpPerformanceCount);

        [DllImport(Dll)]
        public static extern unsafe int QueryPerformanceFrequency(LARGE_INTEGER* lpFrequency);

        [DllImport(Dll)]
        public static extern unsafe int ReadFile(HANDLE hFile, byte* lpBuffer, uint nNumberOfBytesToRead, uint* lpNumberOfBytesRead, void* lpOverlapped);

        [DllImport(Dll)]
        public static extern unsafe uint SetFilePointer(HANDLE hFile, uint lDistanceToMove, uint* lpDistanceToMoveHigh, uint dwMoveMethod);

        [DllImport(Dll)]
        public static extern int FreeConsole();

        [DllImport(Dll)]
        public static extern int CloseHandle(HANDLE hObject);

        [DllImport(Dll)]
        public static extern IntPtr GetProcAddress(HMODULE hModule, string lpProcName);

        [DllImport(Dll)]
        public static extern uint FlushFileBuffers(HANDLE hFile);

        [DllImport(Dll)]
        public static extern unsafe uint ReadFile(HANDLE hFile, byte* lpBuffer, ulong nNumberOfBytesToRead, ulong* lpNumberOfBytesRead, IntPtr lpOverlapped);

        [DllImport(Dll)]
        public static extern HANDLE CreateFileA(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, HANDLE hTemplateFile);

        [DllImport(Dll)]
        public static extern unsafe uint WriteFile(HANDLE hFile, string lpBuffer, uint nNumberOfBytesToWrite, ulong* lpNumberOfBytesWritten, IntPtr lpOverlapped);

        [DllImport(Dll)]
        public static extern HMODULE GetModuleHandleA(IntPtr lpModuleName);
    }
}
