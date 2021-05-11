using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    // ReSharper disable once InconsistentNaming
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
        string GetCurrentModule();
        byte PakPortRead(byte port);
        void PakPortWrite(byte port, byte data);
    }

    // ReSharper disable once InconsistentNaming
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

        public string GetCurrentModule()
        {
            unsafe
            {
                return Converter.ToString(GetPakInterfaceState()->DllPath);
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

        public void ResetBus()
        {
            unsafe
            {
                GetPakInterfaceState()->BankedCartOffset = 0;

                if (HasModuleReset() == Define.TRUE) {
                    InvokeModuleReset();
                }
            }
        }

        public void UpdateBusPointer()
        {
            if (HasSetInterruptCallPointer() == Define.TRUE) {
                InvokeSetInterruptCallPointer();
            }
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

        public int HasSetInterruptCallPointer()
        {
            return Library.PAKInterface.HasSetInterruptCallPointer();
        }

        public void InvokeSetInterruptCallPointer()
        {
            Library.PAKInterface.InvokeSetInterruptCallPointer();
        }

        public int HasModuleReset()
        {
            return Library.PAKInterface.HasModuleReset();
        }

        public void InvokeModuleReset()
        {
            Library.PAKInterface.InvokeModuleReset();
        }

        public unsafe void GetModuleStatus(EmuState* emuState)
        {
            Library.PAKInterface.GetModuleStatus(emuState);
        }

        public byte PakPortRead(byte port)
        {
            return Library.PAKInterface.PakPortRead(port);
        }

        public void PakPortWrite(byte port, byte data)
        {
            Library.PAKInterface.PakPortWrite(port, data);
        }
    }
}
