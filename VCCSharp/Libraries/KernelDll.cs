using System;
using System.Runtime.InteropServices;
using HANDLE = System.IntPtr;
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
        public static extern uint WaitForSingleObject(HANDLE handle, uint dwMilliseconds);

        [DllImport(DLL)]
        public static extern short SetThreadPriority(HANDLE handle, short nPriority);

        [DllImport(DLL)]
        public static extern short CloseHandle(HANDLE hObject);

        //HANDLE CreateEventA(
        //    LPSECURITY_ATTRIBUTES lpEventAttributes,
        //    BOOL                  bManualReset,
        //    BOOL                  bInitialState,
        //    LPCSTR                lpName
        //);
        [DllImport(DLL)]
        public static extern HANDLE CreateEventA(IntPtr lpEventAttributes, int bManualReset, int bInitialState, string lpName);
    }
}