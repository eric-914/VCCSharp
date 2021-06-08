using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using HINSTANCE = System.IntPtr;
using HMODULE = System.IntPtr;

namespace VCCSharp.Modules
{
    // ReSharper disable once InconsistentNaming
    public interface IPAKInterface
    {
        void UnloadDll(bool emulationRunning);
        void GetModuleStatus();
        void ResetBus();
        void UpdateBusPointer();
        int InsertModule(bool emulationRunning, string modulePath);
        void PakTimer();
        string GetCurrentModule();
        byte PakPortRead(byte port);
        void PakPortWrite(byte port, byte data);
        ushort PakAudioSample();
        bool HasConfigModule();
        void InvokeConfigModule(byte menuItem);
        bool UnloadPack(bool emulationRunning);
        byte CartInserted { get; set; }
        string ModuleName { get; set; }
        byte PakMem8Read(ushort address);
    }

    // ReSharper disable once InconsistentNaming
    public class PAKInterface : IPAKInterface
    {
        private readonly IModules _modules;

        // ReSharper disable once InconsistentNaming
        private static readonly object _lock = new object();

        private string _dllPath;

        public byte CartInserted { get; set; }
        public string ModuleName { get; set; } = "Blank";
        public int DialogOpen;
        public int RomPackLoaded;
        public uint BankedCartOffset;

        // Storage for Pak ROMs
        public byte[] ExternalRomBuffer = new byte[Define.PAK_MAX_MEM];

        // ReSharper disable once InconsistentNaming
        public HINSTANCE hInstLib;

        private readonly PakInterfaceDelegates _d = new PakInterfaceDelegates();

        public PAKInterface(IModules modules)
        {
            _modules = modules;
        }

        //public unsafe PakInterfaceDelegates* GetPakInterfaceDelegates()
        //{
        //    return Library.PAKInterface.GetPakInterfaceDelegates();
        //}

        public void PakTimer()
        {
            if (HasHeartBeat())
            {
                InvokeHeartBeat();
            }
        }

        public string GetCurrentModule()
        {
            return _dllPath;
        }

        public void UnloadDll(bool emulationRunning)
        {
            if ((DialogOpen == Define.TRUE) && emulationRunning)
            {
                MessageBox.Show("Close Configuration Dialog before unloading", "Ok");

                return;
            }

            UnloadModule();

            if (hInstLib != IntPtr.Zero)
            {
                PakFreeLibrary(hInstLib);
            }

            hInstLib = IntPtr.Zero;

            _modules.MenuCallbacks.RefreshCartridgeMenu();
        }

        public void ResetBus()
        {
            BankedCartOffset = 0;

            if (HasModuleReset())
            {
                InvokeModuleReset();
            }
        }

        public void UpdateBusPointer()
        {
            if (HasSetInterruptCallPointer())
            {
                InvokeSetInterruptCallPointer();
            }
        }

        public int InsertModule(bool emulationRunning, string modulePath)
        {
            int fileType = FileId(modulePath);

            return fileType switch
            {
                0 => //File doesn't exist
                    InsertModuleCase0(),
                1 => //File is a DLL
                    InsertModuleCase1(emulationRunning, modulePath),
                2 => //File is a ROM image
                    InsertModuleCase2(emulationRunning, modulePath),
                _ => Define.NOMODULE
            };
        }

        //File doesn't exist
        public int InsertModuleCase0()
        {
            return Define.NOMODULE;
        }

        //File is a DLL
        public int InsertModuleCase1(bool emulationRunning, string modulePath)
        {
            ushort moduleParams = 0;
            string catNumber = "";

            UnloadDll(emulationRunning);

            hInstLib = PakLoadLibrary(modulePath);

            if (hInstLib == HINSTANCE.Zero)
            {
                return Define.NOMODULE;
            }

            SetCart(0);

            if (SetDelegates(hInstLib))
            {
                PakFreeLibrary(hInstLib);

                hInstLib = HINSTANCE.Zero;

                return Define.NOTVCC;
            }

            BankedCartOffset = 0;

            if (HasDmaMemPointer())
            {
                InvokeDmaMemPointer();
            }

            if (HasSetInterruptCallPointer())
            {
                InvokeSetInterruptCallPointer();
            }

            ModuleName = InvokeGetModuleName(catNumber);  //Instantiate the menus from HERE!
            string modName = ModuleName;

            var temp = $"Configure {modName}";
            var text = $"Module Name: {modName}\n";

            if (HasConfigModule())
            {
                moduleParams |= 1;

                text += "Has Configurable options\n";
            }

            if (HasPakPortWrite())
            {
                moduleParams |= 2;

                text += "Is IO writable\n";
            }

            if (HasPakPortRead())
            {
                moduleParams |= 4;

                text += "Is IO readable\n";
            }

            if (HasSetInterruptCallPointer())
            {
                moduleParams |= 8;

                text += "Generates Interrupts\n";
            }

            if (HasDmaMemPointer())
            {
                moduleParams |= 16;

                text += "Generates DMA Requests\n";
            }

            if (HasHeartBeat())
            {
                moduleParams |= 32;

                text += "Needs Heartbeat\n";
            }

            if (HasModuleAudioSample())
            {
                moduleParams |= 64;

                text += "Analog Audio Outputs\n";
            }

            if (HasPakMemWrite8())
            {
                moduleParams |= 128;

                text += "Needs ChipSelect Write\n";
            }

            if (HasPakMemRead8())
            {
                moduleParams |= 256;

                text += "Needs ChipSelect Read\n";
            }

            if (HasModuleStatus())
            {
                moduleParams |= 512;

                text += "Returns Status\n";
            }

            if (HasModuleReset())
            {
                moduleParams |= 1024;

                text += "Needs Reset Notification\n";
            }

            if (HasSetIniPath())
            {
                moduleParams |= 2048;

                InvokeSetIniPath(_modules.Config.IniFilePath);
            }

            if (HasPakSetCart())
            {
                moduleParams |= 4096;

                text += "Can Assert CART\n";

                InvokePakSetCart();
            }

            Console.WriteLine(temp);
            Console.WriteLine(text);
            Console.WriteLine(moduleParams);

            _dllPath = modulePath;

            return 0;
        }

        //File is a ROM image
        public int InsertModuleCase2(bool emulationRunning, string modulePath)
        {
            UnloadDll(emulationRunning);

            LoadRomPack(emulationRunning, modulePath);

            ModuleName = Path.GetFileName(modulePath);

            _modules.MenuCallbacks.RefreshCartridgeMenu();

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
            return HasModulePortRead() ? ModulePortRead(port) : (byte) 0;
        }

        public void PakPortWrite(byte port, byte data)
        {
            if (ModulePortWrite(port, data) == 1)
            {
                if (port == 0x40 && RomPackLoaded != 0)
                {
                    BankedCartOffset = (uint)((data & 15) << 14);
                }
            }
        }

        public ushort PakAudioSample()
        {
            if (HasModuleAudioSample())
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

        public void SetCart(byte cart)
        {
            CartInserted = cart;
        }

        /**
            Load a ROM pack
            return total bytes loaded, or 0 on failure
        */
        public int LoadRomPack(bool emulationRunning, string filename)
        {
            ExternalRomBuffer = new byte[Define.PAK_MAX_MEM];

            var rom = File.ReadAllBytes(filename);

            for (int i = 0; i < rom.Length; i++)
            {
                ExternalRomBuffer[i] = rom[i];
            }

            UnloadDll(emulationRunning);

            BankedCartOffset = 0;
            RomPackLoaded = Define.TRUE;

            return rom.Length;
        }

        public bool UnloadPack(bool emulationRunning)
        {
            UnloadDll(emulationRunning);

            _dllPath = "";
            ModuleName = "Blank";

            RomPackLoaded = Define.FALSE;

            SetCart(0);

            //FreeMemory(instance->ExternalRomBuffer);

            ExternalRomBuffer = null;

            _modules.MenuCallbacks.RefreshCartridgeMenu();

            return true;
        }

        public void UnloadModule()
        {
            unsafe
            {
                _d.ConfigModule = null;
                _d.DmaMemPointers = null;
                _d.GetModuleName = null;
                _d.HeartBeat = null;
                _d.ModuleAudioSample = null;
                _d.ModuleReset = null;
                _d.ModuleStatus = null;
                _d.PakMemRead8 = null;
                _d.PakMemWrite8 = null;
                _d.PakPortRead = null;
                _d.PakPortWrite = null;
                _d.SetInterruptCallPointer = null;
            }
        }

        public bool HasSetInterruptCallPointer()
        {
            unsafe
            {
                return _d.SetInterruptCallPointer != null;
            }
        }

        public bool HasModuleReset()
        {
            unsafe
            {
                return _d.ModuleReset != null;
            }
        }

        public bool HasModuleStatus()
        {
            unsafe
            {
                return _d.ModuleStatus != null;
            }
        }

        public bool HasDmaMemPointer()
        {
            unsafe
            {
                return _d.DmaMemPointers != null;
            }
        }

        public bool HasModuleAudioSample()
        {
            unsafe
            {
                return _d.ModuleAudioSample != null;
            }
        }

        public bool HasPakMemWrite8()
        {
            unsafe
            {
                return _d.PakMemWrite8 != null;
            }
        }

        public bool HasPakMemRead8()
        {
            unsafe
            {

                return _d.PakMemRead8 != null;
            }
        }

        public bool HasSetIniPath()
        {
            unsafe
            {
                return _d.SetIniPath != null;
            }
        }

        public bool HasPakSetCart()
        {
            unsafe
            {
                return _d.PakSetCart != null;
            }
        }

        public bool HasPakPortWrite()
        {
            unsafe
            {
                return _d.PakPortWrite != null;
            }
        }

        public bool HasPakPortRead()
        {
            unsafe
            {
                return _d.PakPortRead != null;
            }
        }

        public bool HasHeartBeat()
        {
            unsafe
            {
                return _d.HeartBeat != null;
            }
        }

        public bool HasConfigModule()
        {
            unsafe
            {
                return _d.ConfigModule != null;
            }
        }

        public void InvokeHeartBeat()
        {
            unsafe
            {
                IntPtr p = (IntPtr)(_d.HeartBeat);

                HEARTBEAT fn = Marshal.GetDelegateForFunctionPointer<HEARTBEAT>(p);

                fn();
            }
        }

        public void InvokeModuleReset()
        {
            unsafe
            {
                IntPtr p = (IntPtr)(_d.ModuleReset);

                MODULERESET fn = Marshal.GetDelegateForFunctionPointer<MODULERESET>(p);

                fn();
            }
        }

        public unsafe void InvokeModuleStatus(byte* statusLine)
        {
            IntPtr p = (IntPtr)(_d.ModuleStatus);

            MODULESTATUS fn = Marshal.GetDelegateForFunctionPointer<MODULESTATUS>(p);

            fn(statusLine);
        }

        public void InvokeSetIniPath(string ini)
        {
            unsafe
            {
                IntPtr p = (IntPtr)(_d.SetIniPath);

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
                _dynamicMenuCallback = _modules.MenuCallbacks.BuildCartridgeMenu;
                IntPtr callback = Marshal.GetFunctionPointerForDelegate(_dynamicMenuCallback);

                DYNAMICMENUCALLBACK fnx = Marshal.GetDelegateForFunctionPointer<DYNAMICMENUCALLBACK>(callback);

                IntPtr p = (IntPtr)_d.GetModuleName;

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
                IntPtr p = (IntPtr)(_d.ConfigModule);

                CONFIGMODULE fn = Marshal.GetDelegateForFunctionPointer<CONFIGMODULE>(p);

                fn(menuItem);
            }
        }

        private static ASSERTINTERRUPT _assertInterruptCallback;

        public void InvokeSetInterruptCallPointer()
        {
            void CpuAssertInterrupt(byte irq, byte flag)
            {
                _modules.CPU.CPUAssertInterrupt((CPUInterrupts)irq, flag);
            }

            unsafe
            {
                _assertInterruptCallback = CpuAssertInterrupt;

                IntPtr callback = Marshal.GetFunctionPointerForDelegate(_assertInterruptCallback);

                ASSERTINTERRUPT fnx = Marshal.GetDelegateForFunctionPointer<ASSERTINTERRUPT>(callback);

                IntPtr p = (IntPtr)(_d.SetInterruptCallPointer);

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

                IntPtr p = (IntPtr)(_d.DmaMemPointers);

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

                IntPtr p = (IntPtr)(_d.PakSetCart);

                PAKSETCART fn = Marshal.GetDelegateForFunctionPointer<PAKSETCART>(p);

                fn(fnx);
            }
        }

        public ushort ReadModuleAudioSample()
        {
            unsafe
            {
                IntPtr p = (IntPtr)(_d.ModuleAudioSample);

                MODULEAUDIOSAMPLE fn = Marshal.GetDelegateForFunctionPointer<MODULEAUDIOSAMPLE>(p);

                return fn();
            }
        }

        // ReSharper disable once ParameterHidesMember
        public bool SetDelegates(HINSTANCE hInstLib)
        {
            unsafe
            {
                _d.GetModuleName = GetFunction(hInstLib, "ModuleName");
                _d.ConfigModule = GetFunction(hInstLib, "ModuleConfig");
                _d.PakPortWrite = GetFunction(hInstLib, "PackPortWrite");
                _d.PakPortRead = GetFunction(hInstLib, "PackPortRead");
                _d.SetInterruptCallPointer = GetFunction(hInstLib, "AssertInterrupt");
                _d.DmaMemPointers = GetFunction(hInstLib, "MemPointers");
                _d.HeartBeat = GetFunction(hInstLib, "HeartBeat");
                _d.PakMemWrite8 = GetFunction(hInstLib, "PakMemWrite8");
                _d.PakMemRead8 = GetFunction(hInstLib, "PakMemRead8");
                _d.ModuleStatus = GetFunction(hInstLib, "ModuleStatus");
                _d.ModuleAudioSample = GetFunction(hInstLib, "ModuleAudioSample");
                _d.ModuleReset = GetFunction(hInstLib, "ModuleReset");
                _d.SetIniPath = GetFunction(hInstLib, "SetIniPath");
                _d.PakSetCart = GetFunction(hInstLib, "SetCart");

                return _d.GetModuleName == null;
            }
        }

        public int ModulePortWrite(byte port, byte data)
        {
            unsafe
            {
                if (_d.PakPortWrite != null)
                {
                    IntPtr p = (IntPtr)(_d.PakPortWrite);

                    PAKPORTWRITE fn = Marshal.GetDelegateForFunctionPointer<PAKPORTWRITE>(p);

                    fn(port, data);

                    return 0;
                }

                return 1;
            }
        }

        public byte PakMem8Read(ushort address)
        {
            if (HasModuleMem8Read())
            {
                return ModuleMem8Read(address);
            }

            int offset = (int)((address & 32767) + BankedCartOffset);

            if (ExternalRomBuffer != null)
            {
                //Threading makes it possible to reach here where ExternalRomBuffer = NULL despite check.
                lock (_lock)
                {
                    if (ExternalRomBuffer != null)
                    {
                        return (ExternalRomBuffer[offset]);
                    }
                }
            }

            return 0;
        }

        public bool HasModulePortRead()
        {
            unsafe
            {
                return _d.PakPortRead != null;
            }
        }

        public byte ModulePortRead(byte port)
        {
            unsafe
            {
                IntPtr p = (IntPtr)(_d.PakPortRead);

                PAKPORTREAD fn = Marshal.GetDelegateForFunctionPointer<PAKPORTREAD>(p);

                return fn(port);
            }
        }

        public bool HasModuleMem8Read()
        {
            unsafe
            {
                return _d.PakMemRead8 != null;
            }
        }

        public byte ModuleMem8Read(ushort address)
        {
            unsafe
            {
                IntPtr p = (IntPtr)(_d.PakMemRead8);

                PAKMEMREAD8 fn = Marshal.GetDelegateForFunctionPointer<PAKMEMREAD8>(p);

                return fn((ushort)(address & 32767));
            }
        }

        public HINSTANCE PakLoadLibrary(string modulePath)
        {
            return Library.PakInterface.PAKLoadLibrary(modulePath);
        }

        public void PakFreeLibrary(HINSTANCE h)
        {
            Library.PakInterface.PAKFreeLibrary(h);
        }

        public unsafe void* GetFunction(HMODULE hModule, string lpProcName)
        {
            return Library.PakInterface.GetFunction(hModule, lpProcName);
        }
    }
}
