using System;
using System.Runtime.InteropServices;

namespace VCCSharp.Libraries
{
    public static class Kernel
    {
        public const string KERNEL = "kernel32.dll";

        [DllImport(KERNEL)]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport(KERNEL)]
        public static extern bool FreeLibrary(IntPtr hModule);

        [DllImport(KERNEL)]
        public static extern uint WaitForSingleObject(IntPtr handle, uint dwMilliseconds);

        [DllImport(KERNEL)]
        public static extern short SetThreadPriority(IntPtr handle, short nPriority);
    }
}
