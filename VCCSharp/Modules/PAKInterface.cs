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
using HMODULE = System.IntPtr;

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
        byte CartInserted { get; set; }
        string ModuleName { get; set; } 
    }

    // ReSharper disable once InconsistentNaming
    public class PAKInterface : IPAKInterface
    {
        private readonly IModules _modules;
        private readonly IKernel _kernel;

        private string _dllPath;

        public byte CartInserted { get; set; }
        public string ModuleName { get; set; } = "Blank";
        public int DialogOpen;
        public int RomPackLoaded;

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

            if ((DialogOpen == Define.TRUE) && (emulationRunning == Define.TRUE))
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

                ModuleName = InvokeGetModuleName(catNumber);  //Instantiate the menus from HERE!
                string modName = ModuleName;

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

            ModuleName = Path.GetFileName(modulePath);

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
            if (HasModulePortRead() != 0)
            {
                return ModulePortRead(port);
            }

            return 0;
        }

        public void PakPortWrite(byte port, byte data)
        {
            unsafe
            {
                PakInterfaceState* instance = GetPakInterfaceState();

                if (ModulePortWrite(port, data) == 1)
                {
                    if ((port == 0x40) && (RomPackLoaded) != 0)
                    {
                        instance->BankedCartOffset = (uint)((data & 15) << 14);
                    }
                }
            }
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
            if (!File.Exists(filename)) return 0;   //File Doesn't exist

            var file = File.ReadAllBytes(filename);
            if (file[0] == 'M' && file[1] == 'Z') return 1; //DLL File

            return 2; //Rom Image 
        }

        public HINSTANCE PakLoadLibrary(string modulePath)
        {
            return Library.PAKInterface.PAKLoadLibrary(modulePath);
        }

        public void SetCart(byte cart)
        {
            CartInserted = cart;
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
                RomPackLoaded = Define.TRUE;

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
                ModuleName = "Blank\0";

                RomPackLoaded = Define.FALSE;

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

        public string InvokeGetModuleName(string catNumber)
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

                fixed (byte* b = new byte[256])
                {
                    fn(b, catNumber, fnx);

                    return Converter.ToString(b);
                }
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

        private static ASSERTINTERRUPT _assertInterruptCallback;

        public void InvokeSetInterruptCallPointer()
        {
            unsafe
            {
                _assertInterruptCallback = _modules.CPU.CPUAssertInterrupt;

                IntPtr callback = Marshal.GetFunctionPointerForDelegate(_assertInterruptCallback);

                ASSERTINTERRUPT fnx = Marshal.GetDelegateForFunctionPointer<ASSERTINTERRUPT>(callback);

                PakInterfaceDelegates* d = GetPakInterfaceDelegates();

                IntPtr p = (IntPtr)(d->SetInterruptCallPointer);

                SETINTERRUPTCALLPOINTER fn = Marshal.GetDelegateForFunctionPointer<SETINTERRUPTCALLPOINTER>(p);

                fn(fnx);
            }
        }

        private static PAKMEMREAD8 _readCallback;
        private static PAKMEMWRITE8 _writeCallback;

        public void InvokeDmaMemPointer()
        {
            unsafe
            {
                _readCallback = _modules.TC1014.MemRead8;
                _writeCallback = _modules.TC1014.MemWrite8;

                IntPtr callbackRead = Marshal.GetFunctionPointerForDelegate(_readCallback);
                IntPtr callbackWrite = Marshal.GetFunctionPointerForDelegate(_writeCallback);

                PAKMEMREAD8 fnRead = Marshal.GetDelegateForFunctionPointer<PAKMEMREAD8>(callbackRead);
                PAKMEMWRITE8 fnWrite = Marshal.GetDelegateForFunctionPointer<PAKMEMWRITE8>(callbackWrite);

                PakInterfaceDelegates* d = GetPakInterfaceDelegates();

                IntPtr p = (IntPtr)(d->DmaMemPointers);

                DMAMEMPOINTERS fn = Marshal.GetDelegateForFunctionPointer<DMAMEMPOINTERS>(p);

                fn(fnRead, fnWrite);
            }
        }

        private static SETCART _setCartCallback;

        public void InvokePakSetCart()
        {
            unsafe
            {
                _setCartCallback = SetCart;
                IntPtr callback = Marshal.GetFunctionPointerForDelegate(_setCartCallback);

                SETCART fnx = Marshal.GetDelegateForFunctionPointer<SETCART>(callback);

                PakInterfaceDelegates* d = GetPakInterfaceDelegates();

                IntPtr p = (IntPtr)(d->PakSetCart);

                PAKSETCART fn = Marshal.GetDelegateForFunctionPointer<PAKSETCART>(p);

                fn(fnx);
            }
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

        public int SetDelegates(HINSTANCE hInstLib)
        {
            unsafe
            {
                PakInterfaceDelegates* d = GetPakInterfaceDelegates();

                //delegates->GetModuleName = (GETMODULENAME)GetFunction(hInstLib, "ModuleName");
                //delegates->ConfigModule = (CONFIGMODULE)GetFunction(hInstLib, "ModuleConfig");
                //delegates->PakPortWrite = (PAKPORTWRITE)GetFunction(hInstLib, "PackPortWrite");
                //delegates->PakPortRead = (PAKPORTREAD)GetFunction(hInstLib, "PackPortRead");
                //delegates->SetInterruptCallPointer = (SETINTERRUPTCALLPOINTER)GetFunction(hInstLib, "AssertInterrupt");
                //delegates->DmaMemPointers = (DMAMEMPOINTERS)GetFunction(hInstLib, "MemPointers");
                //delegates->HeartBeat = (HEARTBEAT)GetFunction(hInstLib, "HeartBeat");
                //delegates->PakMemWrite8 = (PAKMEMWRITE8)GetFunction(hInstLib, "PakMemWrite8");
                //delegates->PakMemRead8 = (PAKMEMREAD8)GetFunction(hInstLib, "PakMemRead8");
                //delegates->ModuleStatus = (MODULESTATUS)GetFunction(hInstLib, "ModuleStatus");
                //delegates->ModuleAudioSample = (MODULEAUDIOSAMPLE)GetFunction(hInstLib, "ModuleAudioSample");
                //delegates->ModuleReset = (MODULERESET)GetFunction(hInstLib, "ModuleReset");
                //delegates->SetIniPath = (SETINIPATH)GetFunction(hInstLib, "SetIniPath");
                //delegates->PakSetCart = (PAKSETCART)GetFunction(hInstLib, "SetCart");

                d->GetModuleName = GetFunction(hInstLib, "ModuleName");
                d->ConfigModule = GetFunction(hInstLib, "ModuleConfig");
                d->PakPortWrite = GetFunction(hInstLib, "PackPortWrite");
                d->PakPortRead = GetFunction(hInstLib, "PackPortRead");
                d->SetInterruptCallPointer = GetFunction(hInstLib, "AssertInterrupt");
                d->DmaMemPointers = GetFunction(hInstLib, "MemPointers");
                d->HeartBeat = GetFunction(hInstLib, "HeartBeat");
                d->PakMemWrite8 = GetFunction(hInstLib, "PakMemWrite8");
                d->PakMemRead8 = GetFunction(hInstLib, "PakMemRead8");
                d->ModuleStatus = GetFunction(hInstLib, "ModuleStatus");
                d->ModuleAudioSample = GetFunction(hInstLib, "ModuleAudioSample");
                d->ModuleReset = GetFunction(hInstLib, "ModuleReset");
                d->SetIniPath = GetFunction(hInstLib, "SetIniPath");
                d->PakSetCart = GetFunction(hInstLib, "SetCart");

                return d->GetModuleName == null ? Define.TRUE : Define.FALSE;
            }
        }

        public unsafe void* GetFunction(HMODULE hModule, string lpProcName)
        {
            return Library.PAKInterface.GetFunction(hModule, lpProcName);
        }

        public int HasModulePortRead()
        {
            return Library.PAKInterface.HasModulePortRead();
        }

        public byte ModulePortRead(byte port)
        {
            return Library.PAKInterface.ModulePortRead(port);
        }

        public int ModulePortWrite(byte port, byte data)
        {
            unsafe
            {
                PakInterfaceDelegates* d = GetPakInterfaceDelegates();

                if (d->PakPortWrite != null)
                {
                    IntPtr p = (IntPtr)(d->PakPortWrite);

                    PAKPORTWRITE fn = Marshal.GetDelegateForFunctionPointer<PAKPORTWRITE>(p);

                    fn(port, data);

                    return 0;
                }

                return 1;
            }
        }
    }
}
