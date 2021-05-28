using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using VCCSharp.Models.Pak;
using HINSTANCE = System.IntPtr;

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
        int UnloadPack(byte emulationRunning);
    }

    // ReSharper disable once InconsistentNaming
    public class PAKInterface : IPAKInterface
    {
        private readonly IModules _modules;
        private readonly IKernel _kernel;

        private string _dllPath;

        public PAKInterface(IModules modules, IKernel kernel)
        {
            _modules = modules;
            _kernel = kernel;
        }

        public unsafe PakInterfaceState* GetPakInterfaceState()
        {
            return Library.PAKInterface.GetPakInterfaceState();
        }

        public unsafe PakInterfaceDelegates* GetPakInterfaceDelegates()
        {
            return Library.PAKInterface.GetPakInterfaceDelegates();
        }

        public void PakTimer()
        {
            if (HasHeartBeat() != Define.FALSE)
            {
                InvokeHeartBeat();
            }
        }

        public string GetCurrentModule()
        {
            return _dllPath;
        }

        //TODO: Used by LoadROMPack(...), UnloadPack(...), InsertModule(...)
        public unsafe void UnloadDll(byte emulationRunning)
        {
            PakInterfaceState* instance = GetPakInterfaceState();

            if ((instance->DialogOpen == Define.TRUE) && (emulationRunning == Define.TRUE))
            {
                MessageBox.Show("Close Configuration Dialog before unloading", "Ok");

                return;
            }

            UnloadModule();

            if (instance->hInstLib != IntPtr.Zero)
            {
                PakFreeLibrary(instance->hInstLib);
            }

            instance->hInstLib = IntPtr.Zero;

            _modules.MenuCallbacks.DynamicMenuCallback(null, MenuActions.Refresh, Define.IGNORE);
        }

        public void ResetBus()
        {
            unsafe
            {
                GetPakInterfaceState()->BankedCartOffset = 0;

                if (HasModuleReset() == Define.TRUE)
                {
                    InvokeModuleReset();
                }
            }
        }

        public void UpdateBusPointer()
        {
            if (HasSetInterruptCallPointer() == Define.TRUE)
            {
                InvokeSetInterruptCallPointer();
            }
        }

        public int InsertModule(byte emulationRunning, string modulePath)
        {
            int fileType = FileId(modulePath);

            switch (fileType)
            {
                case 0:		//File doesn't exist
                    return InsertModuleCase0();

                case 1:		//File is a DLL
                    return InsertModuleCase1(emulationRunning, modulePath);

                case 2:		//File is a ROM image
                    return InsertModuleCase2(emulationRunning, modulePath);
            }

            return Define.NOMODULE;
        }

        //File doesn't exist
        public int InsertModuleCase0()
        {
            return Define.NOMODULE;
        }

        //File is a DLL
        public int InsertModuleCase1(byte emulationRunning, string modulePath)
        {
            ushort moduleParams = 0;
            string catNumber = "";

            UnloadDll(emulationRunning);

            unsafe
            {
                PakInterfaceState* instance = GetPakInterfaceState();

                instance->hInstLib = PakLoadLibrary(modulePath);

                if (instance->hInstLib == HINSTANCE.Zero)
                {
                    return Define.NOMODULE;
                }

                SetCart(0);

                if (SetDelegates(instance->hInstLib) != 0)
                {
                    PakFreeLibrary(instance->hInstLib);

                    instance->hInstLib = HINSTANCE.Zero;

                    return Define.NOTVCC;
                }

                instance->BankedCartOffset = 0;

                if (HasDmaMemPointer() != 0)
                {
                    InvokeDmaMemPointer();
                }

                if (HasSetInterruptCallPointer() != 0)
                {
                    InvokeSetInterruptCallPointer();
                }

                string modName = Converter.ToString(instance->Modname);
                InvokeGetModuleName(modName, catNumber);  //Instantiate the menus from HERE!

                var temp = $"Configure {modName}";
                var text = $"Module Name: {modName}\n";

                if (HasConfigModule() != 0)
                {
                    moduleParams |= 1;

                    text += "Has Configurable options\n";
                }

                if (HasPakPortWrite() != 0)
                {
                    moduleParams |= 2;

                    text += "Is IO writable\n";
                }

                if (HasPakPortRead() != 0)
                {
                    moduleParams |= 4;

                    text += "Is IO readable\n";
                }

                if (HasSetInterruptCallPointer() != 0)
                {
                    moduleParams |= 8;

                    text += "Generates Interrupts\n";
                }

                if (HasDmaMemPointer() != 0)
                {
                    moduleParams |= 16;

                    text += "Generates DMA Requests\n";
                }

                if (HasHeartBeat() != 0)
                {
                    moduleParams |= 32;

                    text += "Needs Heartbeat\n";
                }

                if (HasModuleAudioSample() != 0)
                {
                    moduleParams |= 64;

                    text += "Analog Audio Outputs\n";
                }

                if (HasPakMemWrite8() != 0)
                {
                    moduleParams |= 128;

                    text += "Needs ChipSelect Write\n";
                }

                if (HasPakMemRead8() != 0)
                {
                    moduleParams |= 256;

                    text += "Needs ChipSelect Read\n";
                }

                if (HasModuleStatus())
                {
                    moduleParams |= 512;

                    text += "Returns Status\n";
                }

                if (HasModuleReset() != 0)
                {
                    moduleParams |= 1024;

                    text += "Needs Reset Notification\n";
                }

                if (HasSetIniPath() != 0)
                {
                    moduleParams |= 2048;

                    var ini = Converter.ToString(_modules.Config.GetConfigState()->IniFilePath);

                    InvokeSetIniPath(ini);
                }

                if (HasPakSetCart() != 0)
                {
                    moduleParams |= 4096;

                    text += "Can Assert CART\n";

                    InvokePakSetCart();
                }

                Console.WriteLine(temp);
                Console.WriteLine(text);

                _dllPath = modulePath;

                return 0;
            }
        }

        //File is a ROM image
        public int InsertModuleCase2(byte emulationRunning, string modulePath)
        {
            UnloadDll(emulationRunning);

            LoadRomPack(emulationRunning, modulePath);

            unsafe
            {
                PakInterfaceState* instance = GetPakInterfaceState();

                Converter.ToByteArray(modulePath, instance->Modname);

                _modules.FileOperations.FilePathStripPath(instance->Modname);
            }

            _modules.MenuCallbacks.DynamicMenuCallback(null, MenuActions.Refresh, Define.IGNORE);

            SetCart(1);

            return 0;
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

        public ushort PakAudioSample()
        {
            if (HasModuleAudioSample() != 0)
            {
                return (ReadModuleAudioSample());
            }

            return 0;
        }

        public int FileId(string filename)
        {
            return Library.PAKInterface.FileID(filename);
        }

        public HINSTANCE PakLoadLibrary(string modulePath)
        {
            return Library.PAKInterface.PAKLoadLibrary(modulePath);
        }

        public void SetCart(byte cart)
        {
            Library.PAKInterface.SetCart(cart);
        }

        public int SetDelegates(HINSTANCE hInstLib)
        {
            return Library.PAKInterface.SetDelegates(hInstLib);
        }

        public void PakFreeLibrary(HINSTANCE hInstLib)
        {
            Library.PAKInterface.PAKFreeLibrary(hInstLib);
        }

        /**
            Load a ROM pack
            return total bytes loaded, or 0 on failure
        */
        public int LoadRomPack(byte emulationRunning, string filename)
        {
            unsafe
            {
                PakInterfaceState* instance = GetPakInterfaceState();

                if (ResetRomBuffer(instance->ExternalRomBuffer) == 0)
                {
                    return 0;
                }

                var rom = File.ReadAllBytes(filename);

                for (int i = 0; i < rom.Length; i++)
                {
                    instance->ExternalRomBuffer[i] = rom[i];
                }

                UnloadDll(emulationRunning);

                instance->BankedCartOffset = 0;
                instance->RomPackLoaded = Define.TRUE;

                return rom.Length;
            }

        }

        public int UnloadPack(byte emulationRunning)
        {
            UnloadDll(emulationRunning);

            unsafe
            {
                PakInterfaceState* instance = GetPakInterfaceState();

                _dllPath = "";
                Converter.ToByteArray("Blank\0", instance->Modname);

                instance->RomPackLoaded = Define.FALSE;

                SetCart(0);

                FreeMemory(instance->ExternalRomBuffer);

                instance->ExternalRomBuffer = null;

                _modules.MenuCallbacks.DynamicMenuCallback(null, MenuActions.Refresh, Define.IGNORE);

                return Define.NOMODULE;
            }
        }

        public unsafe void FreeMemory(byte* target)
        {
            Library.PAKInterface.FreeMemory(target);
        }

        public void UnloadModule()
        {
            unsafe
            {
                PakInterfaceDelegates* d = GetPakInterfaceDelegates();

                d->ConfigModule = null;
                d->DmaMemPointers = null;
                d->GetModuleName = null;
                d->HeartBeat = null;
                d->ModuleAudioSample = null;
                d->ModuleReset = null;
                d->ModuleStatus = null;
                d->PakMemRead8 = null;
                d->PakMemWrite8 = null;
                d->PakPortRead = null;
                d->PakPortWrite = null;
                d->SetInterruptCallPointer = null;
            }
        }

        public unsafe int ResetRomBuffer(byte* buffer)
        {
            Library.PAKInterface.ResetRomBuffer(buffer);

            PakInterfaceState* instance = GetPakInterfaceState();

            // If memory was unable to be allocated, fail
            if (instance->ExternalRomBuffer == null)
            {
                MessageBox.Show("cant allocate ram", "Ok");

                return 0;
            }

            return 1;
        }

        public int HasSetInterruptCallPointer()
        {
            unsafe
            {
                PakInterfaceDelegates* d = GetPakInterfaceDelegates();

                return d->SetInterruptCallPointer == null ? Define.FALSE : Define.TRUE;
            }
        }

        public int HasModuleReset()
        {
            unsafe
            {
                PakInterfaceDelegates* d = GetPakInterfaceDelegates();

                return d->ModuleReset == null ? Define.FALSE : Define.TRUE;
            }
        }

        public bool HasModuleStatus()
        {
            unsafe
            {
                PakInterfaceDelegates* d = GetPakInterfaceDelegates();

                return d->ModuleStatus != null;
            }
        }

        public int HasDmaMemPointer()
        {
            unsafe
            {
                PakInterfaceDelegates* d = GetPakInterfaceDelegates();

                return d->DmaMemPointers == null ? Define.FALSE : Define.TRUE;
            }
        }

        public int HasModuleAudioSample()
        {
            unsafe
            {
                PakInterfaceDelegates* d = GetPakInterfaceDelegates();

                return d->ModuleAudioSample == null ? Define.FALSE : Define.TRUE;
            }
        }

        public int HasPakMemWrite8()
        {
            unsafe
            {
                PakInterfaceDelegates* d = GetPakInterfaceDelegates();

                return d->PakMemWrite8 == null ? Define.FALSE : Define.TRUE;
            }
        }

        public int HasPakMemRead8()
        {
            unsafe
            {
                PakInterfaceDelegates* d = GetPakInterfaceDelegates();

                return d->PakMemRead8 == null ? Define.FALSE : Define.TRUE;
            }
        }

        public int HasSetIniPath()
        {
            unsafe
            {
                PakInterfaceDelegates* d = GetPakInterfaceDelegates();

                return d->SetIniPath == null ? Define.FALSE : Define.TRUE;
            }
        }

        public int HasPakSetCart()
        {
            unsafe
            {
                PakInterfaceDelegates* d = GetPakInterfaceDelegates();

                return d->PakSetCart == null ? Define.FALSE : Define.TRUE;
            }
        }

        public int HasPakPortWrite()
        {
            unsafe
            {
                PakInterfaceDelegates* d = GetPakInterfaceDelegates();

                return d->PakPortWrite == null ? Define.FALSE : Define.TRUE;
            }
        }

        public int HasPakPortRead()
        {
            unsafe
            {
                PakInterfaceDelegates* d = GetPakInterfaceDelegates();

                return d->PakPortRead == null ? Define.FALSE : Define.TRUE;
            }
        }

        public int HasHeartBeat()
        {
            unsafe
            {
                PakInterfaceDelegates* d = GetPakInterfaceDelegates();

                return d->HeartBeat == null ? Define.FALSE : Define.TRUE;
            }
        }

        public int HasConfigModule()
        {
            unsafe
            {
                PakInterfaceDelegates* d = GetPakInterfaceDelegates();

                return d->ConfigModule == null ? Define.FALSE : Define.TRUE;
            }
        }

        public void InvokeHeartBeat()
        {
            unsafe
            {
                PakInterfaceDelegates* d = GetPakInterfaceDelegates();

                IntPtr p = (IntPtr)(d->HeartBeat);

                HEARTBEAT fn = Marshal.GetDelegateForFunctionPointer<HEARTBEAT>(p);

                fn();
            }
        }

        public void InvokeModuleReset()
        {
            unsafe
            {
                PakInterfaceDelegates* d = GetPakInterfaceDelegates();

                IntPtr p = (IntPtr)(d->ModuleReset);

                MODULERESET fn = Marshal.GetDelegateForFunctionPointer<MODULERESET>(p);

                fn();
            }
        }

        public unsafe void InvokeModuleStatus(byte* statusLine)
        {
            PakInterfaceDelegates* d = GetPakInterfaceDelegates();

            IntPtr p = (IntPtr)(d->ModuleStatus);

            MODULESTATUS fn = Marshal.GetDelegateForFunctionPointer<MODULESTATUS>(p);

            fn(statusLine);
        }

        public void InvokeSetIniPath(string ini)
        {
            unsafe
            {
                PakInterfaceDelegates* d = GetPakInterfaceDelegates();

                IntPtr p = (IntPtr)(d->SetIniPath);

                SETINIPATH fn = Marshal.GetDelegateForFunctionPointer<SETINIPATH>(p);

                fn(ini);
            }
        }

        private static DYNAMICMENUCALLBACK _dynamicMenuCallback;

        public void InvokeGetModuleName(string modName, string catNumber)
        {
            //Instantiate the menus from HERE!
            unsafe
            {
                _dynamicMenuCallback = _modules.MenuCallbacks.DynamicMenuCallback;
                IntPtr callback = Marshal.GetFunctionPointerForDelegate(_dynamicMenuCallback);

                DYNAMICMENUCALLBACK fnx = Marshal.GetDelegateForFunctionPointer<DYNAMICMENUCALLBACK>(callback);

                PakInterfaceDelegates* d = GetPakInterfaceDelegates();

                IntPtr p = (IntPtr)(d->GetModuleName);

                GETMODULENAME fn = Marshal.GetDelegateForFunctionPointer<GETMODULENAME>(p);

                //DynamicMenuCallback
                fn(modName, catNumber, fnx);
            }
        }

        public void InvokeConfigModule(byte menuItem)
        {
            unsafe
            {
                PakInterfaceDelegates* d = GetPakInterfaceDelegates();

                IntPtr p = (IntPtr)(d->ConfigModule);

                CONFIGMODULE fn = Marshal.GetDelegateForFunctionPointer<CONFIGMODULE>(p);

                fn(menuItem);
            }
        }

        public void InvokeSetInterruptCallPointer()
        {
            Library.PAKInterface.InvokeSetInterruptCallPointer();
        }

        public void InvokeDmaMemPointer()
        {
            Library.PAKInterface.InvokeDmaMemPointer();
        }

        public void InvokePakSetCart()
        {
            Library.PAKInterface.InvokePakSetCart();
        }

        public ushort ReadModuleAudioSample()
        {
            unsafe
            {
                PakInterfaceDelegates* d = GetPakInterfaceDelegates();

                IntPtr p = (IntPtr)(d->ModuleAudioSample);

                MODULEAUDIOSAMPLE fn = Marshal.GetDelegateForFunctionPointer<MODULEAUDIOSAMPLE>(p);

                return fn();
            }
        }

    }
}
