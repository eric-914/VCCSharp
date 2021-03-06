using HANDLE = System.IntPtr;
using HMODULE = System.IntPtr;
using static System.IntPtr;

namespace VCCSharp.Libraries
{
    public interface IKernel
    {
        HMODULE LoadLibrary(string dllToLoad);
        bool FreeLibrary(HMODULE hModule);
    }

    public class Kernel : IKernel
    {
        public HMODULE LoadLibrary(string dllToLoad)
            => KernelDll.LoadLibrary(dllToLoad);

        public bool FreeLibrary(HMODULE hModule)
            => KernelDll.FreeLibrary(hModule);
    }
}
