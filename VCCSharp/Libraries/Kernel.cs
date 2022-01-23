﻿using System;
using VCCSharp.Libraries.Models;
using VCCSharp.Models;
using HMODULE = System.IntPtr;
using HANDLE = System.IntPtr;
using LPVOID = System.IntPtr;
using static System.IntPtr;

namespace VCCSharp.Libraries
{
    public interface IKernel
    {
        HMODULE LoadLibrary(string dllToLoad);
        bool FreeLibrary(HMODULE hModule);
        ushort GetPrivateProfileIntA(string lpAppName, string lpKeyName, short nDefault, string lpFileName);
        uint GetPrivateProfileStringA(string lpAppName, string lpKeyName, string lpDefault, byte[] lpReturnedString, uint nSize, string lpFileName);
        int WritePrivateProfileStringA(string lpAppName, string lpKeyName, string lpString, string lpFileName);
        int QueryPerformanceCounter(ref LARGE_INTEGER lpPerformanceCount);
        int QueryPerformanceFrequency(ref LARGE_INTEGER lpFrequency);
        int ReadFile(HANDLE hFile, byte[] lpBuffer, uint nNumberOfBytesToRead, ref uint lpNumberOfBytesRead, LPVOID lpOverlapped);
        uint SetFilePointer(IntPtr hFile, uint dwMoveMethod, uint lDistanceToMove = 0);
        int FreeConsole();
        int CloseHandle(HANDLE hObject);
        IntPtr GetProcAddress(HMODULE hModule, string lpProcName);
        uint FlushFileBuffers(HANDLE hFile);
        uint ReadFile(HANDLE hFile, byte[] lpBuffer, ulong nNumberOfBytesToRead, ref ulong lpNumberOfBytesRead);
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

        public uint GetPrivateProfileStringA(string lpAppName, string lpKeyName, string lpDefault, byte[] lpReturnedString, uint nSize, string lpFileName)
            => KernelDll.GetPrivateProfileStringA(lpAppName, lpKeyName, lpDefault, lpReturnedString, nSize, lpFileName);

        public int WritePrivateProfileStringA(string lpAppName, string lpKeyName, string lpString, string lpFileName)
            => KernelDll.WritePrivateProfileStringA(lpAppName, lpKeyName, lpString, lpFileName);

        public int QueryPerformanceCounter(ref LARGE_INTEGER lpPerformanceCount)
            => KernelDll.QueryPerformanceCounter(ref lpPerformanceCount);

        public int QueryPerformanceFrequency(ref LARGE_INTEGER lpFrequency)
            => KernelDll.QueryPerformanceFrequency(ref lpFrequency);

        public int ReadFile(HANDLE hFile, byte[] lpBuffer, uint nNumberOfBytesToRead, ref uint lpNumberOfBytesRead, LPVOID lpOverlapped)
            => KernelDll.ReadFile(hFile, lpBuffer, nNumberOfBytesToRead, ref lpNumberOfBytesRead, lpOverlapped);

        public uint SetFilePointer(IntPtr hFile, uint dwMoveMethod, uint lDistanceToMove = 0)
            => KernelDll.SetFilePointer(hFile, lDistanceToMove, IntPtr.Zero, dwMoveMethod);

        public int FreeConsole()
            => KernelDll.FreeConsole();

        public int CloseHandle(HANDLE hObject)
            => KernelDll.CloseHandle(hObject);

        public IntPtr GetProcAddress(HMODULE hModule, string lpProcName)
            => KernelDll.GetProcAddress(hModule, lpProcName);

        public uint FlushFileBuffers(HANDLE hFile)
            => KernelDll.FlushFileBuffers(hFile);

        public uint ReadFile(HANDLE hFile, byte[] lpBuffer, ulong nNumberOfBytesToRead, ref ulong lpNumberOfBytesRead)
            => KernelDll.ReadFile(hFile, lpBuffer, nNumberOfBytesToRead, ref lpNumberOfBytesRead, Zero);

        public HANDLE CreateFile(string filename, uint desiredAccess, uint dwCreationDisposition)
            => KernelDll.CreateFileA(filename, desiredAccess, 0, Zero, dwCreationDisposition, Define.FILE_ATTRIBUTE_NORMAL, Zero);

        public uint WriteFile(HANDLE hFile, string lpBuffer, uint nNumberOfBytesToWrite)
        {
            ulong temp = 0;

            return KernelDll.WriteFile(hFile, lpBuffer, nNumberOfBytesToWrite, ref temp, Zero);
        }

        public unsafe uint WriteFile(HANDLE hFile, byte* lpBuffer, uint nNumberOfBytesToWrite)
        {
            return WriteFile(hFile, Converter.ToString(lpBuffer), nNumberOfBytesToWrite);
        }

    }
}
