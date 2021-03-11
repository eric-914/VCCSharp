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
        int HasHeartBeat();
        void InvokeHeartBeat();
    }

    public class PAKInterface : IPAKInterface
    {
        private readonly IModules _modules;
        private readonly IKernel _kernel;

        public PAKInterface(IModules modules, IKernel kernel)
        {
            _modules = modules;
            _kernel = kernel;
        }

        public unsafe PakInterfaceState* GetPakInterfaceState()
        {
            return Library.PAKInterface.GetPakInterfaceState();
        }

        public void PakTimer()
        {
            if (HasHeartBeat() != Define.FALSE) {
                InvokeHeartBeat();
            }
        }

        //TODO: Used by LoadROMPack(...), UnloadPack(...), InsertModule(...)
        public unsafe void UnloadDll(EmuState* emuState)
        {
            //PakInterfaceState* pakInterfaceState = GetPakInterfaceState();

            //if ((pakInterfaceState->DialogOpen == Define.TRUE) && (emuState->EmulationRunning == Define.TRUE))
            //{
            //    MessageBox.Show("Close Configuration Dialog before unloading", "Ok");

            //    return;
            //}

            Library.PAKInterface.UnloadDll(emuState);

            //if (pakInterfaceState->hInstLib != null) {
            //    _kernel.FreeLibrary(pakInterfaceState->hInstLib);
            //}

            //pakInterfaceState->hInstLib = Zero;

            //_modules.MenuCallbacks.DynamicMenuCallback(emuState, null, MenuActions.Refresh, Define.IGNORE);
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

        public int InsertModuleCase0()
        {
            return Library.PAKInterface.InsertModuleCase0();
        }

        public unsafe int InsertModuleCase1(EmuState* emuState, byte* modulePath)
        {
            return Library.PAKInterface.InsertModuleCase1(emuState, modulePath);
        }

        public unsafe int InsertModuleCase2(EmuState* emuState, byte* modulePath)
        {
            return Library.PAKInterface.InsertModuleCase2(emuState, modulePath);
        }

        public int HasHeartBeat()
        {
            return Library.PAKInterface.HasHeartBeat();
        }

        public void InvokeHeartBeat()
        {
            Library.PAKInterface.InvokeHeartBeat();
        }
    }
}
