using System;
using VCCSharp.Libraries.Models;
using VCCSharp.Models;
using static System.IntPtr;
using HANDLE = System.IntPtr;
using HMODULE = System.IntPtr;
using LPVOID = System.IntPtr;

namespace VCCSharp.Libraries
{
    public interface IKernel
    {
        HMODULE LoadLibrary(string dllToLoad);
        bool FreeLibrary(HMODULE hModule);
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
        uint WriteFile(HANDLE hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite);
        uint WriteFile(HANDLE hFile, uint data);
        uint WriteFile(HANDLE hFile, ushort data);
    }

    public class Kernel : IKernel
    {
        public HMODULE LoadLibrary(string dllToLoad)
            => KernelDll.LoadLibrary(dllToLoad);

        public bool FreeLibrary(HMODULE hModule)
            => KernelDll.FreeLibrary(hModule);

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

        public uint WriteFile(HANDLE hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite)
        {
            return WriteFile(hFile, Converter.ToString(lpBuffer), nNumberOfBytesToWrite);
        }

        public uint WriteFile(HANDLE hFile, byte[] lpBuffer)
        {
            return WriteFile(hFile, Converter.ToString(lpBuffer), (uint)lpBuffer.Length);
        }

        public uint WriteFile(IntPtr hFile, uint data)
        {
            ushort high = (ushort)(data >> 16);
            ushort low = (ushort)(data & 0xFFFF);

            uint value = WriteFile(hFile, high);
            value += WriteFile(hFile, low);

            return value;
        }

        public uint WriteFile(IntPtr hFile, ushort data)
        {
            byte b1 = (byte)((data >> 8) & 0xFF);
            byte b0 = (byte)(data & 0xFF);

            byte[] lpBuffer = { b1, b0 };

            return WriteFile(hFile, lpBuffer);
        }
    }
}
