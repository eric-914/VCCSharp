using System;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    // ReSharper disable once InconsistentNaming
    public interface IPAKInterface
    {
        unsafe PakInterfaceState* GetPakInterfaceState();
        void UnloadDll(byte emulationRunning);
        void GetModuleStatus();
        void ResetBus();
        void UpdateBusPointer();
        int InsertModule(byte emulationRunning, string modulePath);
        void PakTimer();
        int HasHeartBeat();
        void InvokeHeartBeat();
        string GetCurrentModule();
        byte PakPortRead(byte port);
        void PakPortWrite(byte port, byte data);
        ushort PakAudioSample();
        int HasConfigModule();
        void InvokeConfigModule(byte menuItem);
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
        public void UnloadDll(byte emulationRunning)
        {
            //PakInterfaceState* pakInterfaceState = GetPakInterfaceState();

            //if ((pakInterfaceState->DialogOpen == Define.TRUE) && (emuState->EmulationRunning == Define.TRUE))
            //{
            //    MessageBox.Show("Close Configuration Dialog before unloading", "Ok");

            //    return;
            //}

            Library.PAKInterface.UnloadDll(emulationRunning);

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

        public int InsertModule(byte emulationRunning, string modulePath)
        {
            return Library.PAKInterface.InsertModule(emulationRunning, modulePath);
        }

        public int InsertModuleCase0()
        {
            return Library.PAKInterface.InsertModuleCase0();
        }

        public unsafe int InsertModuleCase1(byte emulationRunning, byte* modulePath)
        {
            return Library.PAKInterface.InsertModuleCase1(emulationRunning, modulePath);
        }

        public unsafe int InsertModuleCase2(byte emulationRunning, byte* modulePath)
        {
            return Library.PAKInterface.InsertModuleCase2(emulationRunning, modulePath);
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

        public unsafe void GetModuleStatus()
        {
            if (HasModuleStatus())
            {
                //TODO: Things break here if the status line is empty.
                string text = (_modules.Emu.StatusLine ?? "") + "...";
                byte[] status = Converter.ToByteArray(text);
                fixed (byte* p = status)
                {
                    InvokeModuleStatus(p);
                }
            }
            else
            {
                _modules.Emu.StatusLine = string.Empty;
            }
        }

        public byte PakPortRead(byte port)
        {
            return Library.PAKInterface.PakPortRead(port);
        }

        public void PakPortWrite(byte port, byte data)
        {
            Library.PAKInterface.PakPortWrite(port, data);
        }

        public bool HasModuleStatus()
        {
            return Library.PAKInterface.HasModuleStatus() != 0;
        }

        public unsafe void InvokeModuleStatus(byte* statusLine)
        {
            Library.PAKInterface.InvokeModuleStatus(statusLine);
        }

        public ushort PakAudioSample()
        {
            return Library.PAKInterface.PakAudioSample();
        }

        public int HasConfigModule()
        {
            return Library.PAKInterface.HasConfigModule();
        }

        public void InvokeConfigModule(byte menuItem)
        {
            Library.PAKInterface.InvokeConfigModule(menuItem);
        }
    }
}
