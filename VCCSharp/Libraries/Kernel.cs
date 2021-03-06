using HANDLE = System.IntPtr;
using HMODULE = System.IntPtr;
using static System.IntPtr;

namespace VCCSharp.Libraries
{
    public interface IKernel
    {
        HMODULE LoadLibrary(string dllToLoad);
        bool FreeLibrary(HMODULE hModule);
        uint WaitForSingleObject(HANDLE handle, uint dwMilliseconds);
        short SetThreadPriority(HANDLE handle, short nPriority);
        short CloseHandle(HANDLE hObject);
        HANDLE CreateEventA(int bManualReset, int bInitialState, string lpName);
    }

    public class Kernel : IKernel
    {
        public HMODULE LoadLibrary(string dllToLoad)
            => KernelDll.LoadLibrary(dllToLoad);

        public bool FreeLibrary(HMODULE hModule)
            => KernelDll.FreeLibrary(hModule);

        public uint WaitForSingleObject(HANDLE handle, uint dwMilliseconds)
            => KernelDll.WaitForSingleObject(handle, dwMilliseconds);

        public short SetThreadPriority(HANDLE handle, short nPriority)
            => KernelDll.SetThreadPriority(handle, nPriority);

        public short CloseHandle(HANDLE hObject)
            => KernelDll.CloseHandle(hObject);

        public HANDLE CreateEventA(int bManualReset, int bInitialState, string lpName)
            => KernelDll.CreateEventA(Zero, bManualReset, bInitialState, lpName);
    }
}
