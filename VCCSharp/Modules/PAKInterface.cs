using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IPAKInterface
    {
        unsafe PakInterfaceState* GetPakInterfaceState();
        unsafe void UnloadDll(EmuState* emuState);
        unsafe void GetModuleStatus(EmuState* emuState);
        void ResetBus();
        void UpdateBusPointer();
        unsafe int InsertModule(EmuState* emuState, string modulePath);
        void PakTimer();
    }

    public class PAKInterface : IPAKInterface
    {
        private readonly IModules _modules;

        public PAKInterface(IModules modules)
        {
            _modules = modules;
        }

        public unsafe PakInterfaceState* GetPakInterfaceState()
        {
            return Library.PAKInterface.GetPakInterfaceState();
        }

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

        public void PakTimer()
        {
            Library.PAKInterface.PakTimer();
        }
    }
}
