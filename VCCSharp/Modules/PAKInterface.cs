using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IPAKInterface
    {
        unsafe void UnloadDll(EmuState* emuState);
        unsafe void GetModuleStatus(EmuState* emuState);
        void ResetBus();
        void UpdateBusPointer();
        unsafe int InsertModule(EmuState* emuState, string modulePath);
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

        public void ResetBus()
        {
            Library.PAKInterface.ResetBus();
        }

        public void UpdateBusPointer()
        {
            Library.PAKInterface.UpdateBusPointer();
        }

        public unsafe int InsertModule(EmuState* emuState, string modulePath)
        {
            return Library.PAKInterface.InsertModule(emuState, modulePath);
        }
    }
}
