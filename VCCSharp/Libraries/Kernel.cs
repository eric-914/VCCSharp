using HANDLE = System.IntPtr;
using HMODULE = System.IntPtr;

namespace VCCSharp.Libraries
{
    public interface IKernel
    {
        HMODULE LoadLibrary(string dllToLoad);
        bool FreeLibrary(HMODULE hModule);
        uint WaitForSingleObject(HANDLE handle, uint dwMilliseconds);
        short SetThreadPriority(HANDLE handle, short nPriority);
        short CloseHandle(HANDLE hObject);
    }

    public class Kernel : IKernel
    {
        public HMODULE LoadLibrary(string dllToLoad)
            => KernelDll.LoadLibrary(dllToLoad);

        public bool FreeLibrary(HMODULE  hModule)
            => KernelDll.FreeLibrary(hModule);

        public uint WaitForSingleObject(HANDLE handle, uint dwMilliseconds)
            => KernelDll.WaitForSingleObject(handle, dwMilliseconds);

        public short SetThreadPriority(HANDLE handle, short nPriority)
            => KernelDll.SetThreadPriority(handle, nPriority);

        public short CloseHandle(HANDLE hObject)
            => KernelDll.CloseHandle(hObject);
    }
}
