using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IPAKInterface
    {
        unsafe void UnloadDll(EmuState* emuState);
        unsafe void GetModuleStatus(EmuState* emuState);
    }

    public class PAKInterface : IPAKInterface
    {
        public unsafe void UnloadDll(EmuState* emuState)
        {
            Library.PAKInterface.UnloadDll(emuState);
        }

        public unsafe void GetModuleStatus(EmuState* emuState)
        {
            Library.PAKInterface.GetModuleStatus(emuState);
        }
    }
}
