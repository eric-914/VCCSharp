using System.IO;
using System.Runtime.InteropServices;
using VCCSharp.Configuration;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using VCCSharp.OpCodes.Definitions;
using static System.IntPtr;
using HINSTANCE = System.IntPtr;

namespace VCCSharp.Modules;

// ReSharper disable once InconsistentNaming
public interface IPAKInterface : IModule
{
    void UnloadDll(bool emulationRunning);
    void GetModuleStatus();
    void ResetBus();
    void UpdateBusPointer();
    int InsertModule(string filename);
    void PakTimer();
    string? GetCurrentModule();
    byte PakPortRead(byte port);
    void PakPortWrite(byte port, byte data);
    ushort PakAudioSample();
    bool HasConfigModule();
    void InvokeConfigModule(byte menuItem);
    bool UnloadPack(bool emulationRunning);
    byte CartInserted { get; }
    string ModuleName { get; }
    byte PakMem8Read(ushort address);
}

// ReSharper disable once InconsistentNaming
public class PAKInterface : IPAKInterface
{
    private readonly IModules _modules;
    private readonly IConfigurationManager _configurationManager;
    private readonly IKernel _kernel;

    // ReSharper disable once InconsistentNaming
    private static readonly object _lock = new();

    private string? _dllPath;

    public byte CartInserted { get; set; }
    public string ModuleName { get; set; } = "Blank";
    private bool _romPackLoaded;
    private uint _bankedCartOffset;

    // Storage for Pak ROMs
    private byte[]? _externalRomBuffer = new byte[Define.PAK_MAX_MEM];

    // ReSharper disable once InconsistentNaming
    private HINSTANCE _hInstLib;

    private readonly PakInterfaceDelegates _d = new();

    public PAKInterface(IModules modules, IConfigurationManager configurationManager, IKernel kernel)
    {
        _modules = modules;
        _configurationManager = configurationManager;
        _kernel = kernel;
    }

    public void PakTimer()
    {
        if (HasHeartBeat())
        {
            InvokeHeartBeat();
        }
    }

    public string? GetCurrentModule()
    {
        return _dllPath;
    }

    public void UnloadDll(bool emulationRunning)
    {
        UnloadModule();

        if (_hInstLib != Zero)
        {
            _kernel.FreeLibrary(_hInstLib);
        }

        _hInstLib = Zero;

        if (emulationRunning)
        {
            _modules.MenuCallbacks.RefreshCartridgeMenu();
        }
    }

    public void ResetBus()
    {
        _bankedCartOffset = 0;

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

    public int InsertModule(string modulePath)
    {
        bool emulationRunning = _modules.Emu.EmulationRunning;
        int fileType = FileId(modulePath);

        return fileType switch
        {
            0 => //File doesn't exist
                Define.NOMODULE,
            1 => //File is a DLL
                InsertLibraryModule(emulationRunning, modulePath),
            2 => //File is a ROM image
                InsertRomModule(emulationRunning, modulePath),
            _ => Define.NOMODULE
        };
    }

    //File is a DLL
    private int InsertLibraryModule(bool emulationRunning, string modulePath)
    {
        ushort moduleParams = 0;
        //string catNumber = "";

        UnloadDll(emulationRunning);

        _hInstLib = _kernel.LoadLibrary(modulePath);

        if (_hInstLib == Zero)
        {
            return Define.NOMODULE;
        }

        SetCart(0);

        if (SetDelegates(_hInstLib))
        {
            _kernel.FreeLibrary(_hInstLib);

            _hInstLib = Zero;

            return Define.NOTVCC;
        }

        _bankedCartOffset = 0;

        if (HasDmaMemPointer())
        {
            InvokeDmaMemPointer();
        }

        if (HasSetInterruptCallPointer())
        {
            InvokeSetInterruptCallPointer();
        }

        ModuleName = InvokeGetModuleName();  //Instantiate the menus from HERE!
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

            InvokeSetIniPath(_configurationManager.GetFilePath());
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
    private int InsertRomModule(bool emulationRunning, string modulePath)
    {
        UnloadDll(emulationRunning);

        LoadRomPack(emulationRunning, modulePath);

        ModuleName = Path.GetFileName(modulePath);

        _modules.MenuCallbacks.RefreshCartridgeMenu();

        SetCart(1);

        return 0;
    }

    public void GetModuleStatus()
    {
        if (HasModuleStatus())
        {
            //TODO: Things break here if the status line is empty.
            string text = (_modules.Emu.StatusLine ?? "") + "...";
            byte[] status = Converter.ToByteArray(text);

            InvokeModuleStatus(status);
        }
        else
        {
            _modules.Emu.StatusLine = string.Empty;
        }
    }

    public byte PakPortRead(byte port)
    {
        return HasModulePortRead() ? ModulePortRead(port) : (byte)0;
    }

    public void PakPortWrite(byte port, byte data)
    {
        if (ModulePortWrite(port, data) == 1)
        {
            if (port == 0x40 && _romPackLoaded)
            {
                _bankedCartOffset = (uint)((data & 15) << 14);
            }
        }
    }

    public ushort PakAudioSample()
    {
        return HasModuleAudioSample() ? ReadModuleAudioSample() : (ushort)0;
    }

    private static int FileId(string filename)
    {
        if (!File.Exists(filename)) return 0;   //File Doesn't exist

        var file = File.ReadAllBytes(filename);
        if (file[0] == 'M' && file[1] == 'Z') return 1; //DLL File

        return 2; //Rom Image 
    }

    private void SetCart(byte cart)
    {
        CartInserted = cart;
    }

    /**
        Load a ROM pack
        return total bytes loaded, or 0 on failure
    */
    private int LoadRomPack(bool emulationRunning, string filename)
    {
        _externalRomBuffer = new byte[Define.PAK_MAX_MEM];

        var rom = File.ReadAllBytes(filename);

        for (int i = 0; i < rom.Length; i++)
        {
            _externalRomBuffer[i] = rom[i];
        }

        //UnloadDll(emulationRunning);

        _bankedCartOffset = 0;
        _romPackLoaded = true;

        return rom.Length;
    }

    public bool UnloadPack(bool emulationRunning)
    {
        UnloadDll(emulationRunning);

        _dllPath = "";
        ModuleName = "Blank";

        _romPackLoaded = false;

        SetCart(0);

        //FreeMemory(instance->ExternalRomBuffer);

        _externalRomBuffer = null;

        _modules.MenuCallbacks.RefreshCartridgeMenu();

        return true;
    }

    private void UnloadModule()
    {
        _d.ConfigModule = Zero;
        _d.DmaMemPointers = Zero;
        _d.GetModuleName = Zero;
        _d.HeartBeat = Zero;
        _d.ModuleAudioSample = Zero;
        _d.ModuleReset = Zero;
        _d.ModuleStatus = Zero;
        _d.PakMemRead8 = Zero;
        _d.PakMemWrite8 = Zero;
        _d.PakPortRead = Zero;
        _d.PakPortWrite = Zero;
        _d.SetInterruptCallPointer = Zero;
    }

    private bool HasSetInterruptCallPointer()
    {
        return _d.SetInterruptCallPointer != Zero;
    }

    private bool HasModuleReset()
    {
        return _d.ModuleReset != Zero;
    }

    private bool HasModuleStatus()
    {
        return _d.ModuleStatus != Zero;
    }

    private bool HasDmaMemPointer()
    {
        return _d.DmaMemPointers != Zero;
    }

    private bool HasModuleAudioSample()
    {
        return _d.ModuleAudioSample != Zero;
    }

    private bool HasPakMemWrite8()
    {
        return _d.PakMemWrite8 != Zero;
    }

    private bool HasPakMemRead8()
    {
        return _d.PakMemRead8 != Zero;
    }

    private bool HasSetIniPath()
    {
        return _d.SetIniPath != Zero;
    }

    private bool HasPakSetCart()
    {
        return _d.PakSetCart != Zero;
    }

    private bool HasPakPortWrite()
    {
        return _d.PakPortWrite != Zero;
    }

    private bool HasPakPortRead()
    {
        return _d.PakPortRead != Zero;
    }

    private bool HasHeartBeat()
    {
        return _d.HeartBeat != Zero;
    }

    public bool HasConfigModule()
    {
        return _d.ConfigModule != Zero;
    }

    private void InvokeHeartBeat()
    {
        IntPtr p = _d.HeartBeat;

        HEARTBEAT fn = Marshal.GetDelegateForFunctionPointer<HEARTBEAT>(p);

        fn();
    }

    private void InvokeModuleReset()
    {
        IntPtr p = _d.ModuleReset;

        MODULERESET fn = Marshal.GetDelegateForFunctionPointer<MODULERESET>(p);

        fn();
    }

    private void InvokeModuleStatus(byte[] statusLine)
    {
        IntPtr p = _d.ModuleStatus;

        MODULESTATUS fn = Marshal.GetDelegateForFunctionPointer<MODULESTATUS>(p);

        fn(statusLine);
    }

    private void InvokeSetIniPath(string? ini)
    {
        if (ini == null) throw new ArgumentNullException(nameof(ini));

        IntPtr p = _d.SetIniPath;

        SETINIPATH fn = Marshal.GetDelegateForFunctionPointer<SETINIPATH>(p);

        fn(ini);
    }

    private static DYNAMICMENUCALLBACK? _dynamicMenuCallback;

    private string InvokeGetModuleName()
    {
        //Instantiate the menus from HERE!
        _dynamicMenuCallback = _modules.MenuCallbacks.BuildCartridgeMenu;
        IntPtr callback = Marshal.GetFunctionPointerForDelegate(_dynamicMenuCallback);

        DYNAMICMENUCALLBACK fnx = Marshal.GetDelegateForFunctionPointer<DYNAMICMENUCALLBACK>(callback);

        IntPtr p = _d.GetModuleName;

        GETMODULENAME fn = Marshal.GetDelegateForFunctionPointer<GETMODULENAME>(p);

        var buffer = new byte[256];

        //TODO: Does catNumber serve a purpose
        fn(buffer, "", fnx);

        return Converter.ToString(buffer);
    }

    public void InvokeConfigModule(byte menuItem)
    {
        IntPtr p = _d.ConfigModule;

        CONFIGMODULE fn = Marshal.GetDelegateForFunctionPointer<CONFIGMODULE>(p);

        fn(menuItem);
    }

    private static ASSERTINTERRUPT? _assertInterruptCallback;

    private void InvokeSetInterruptCallPointer()
    {
        void CpuAssertInterrupt(byte irq, byte flag)
        {
            _modules.CPU.AssertInterrupt((CPUInterrupts)irq, flag);
        }

        _assertInterruptCallback = CpuAssertInterrupt;

        IntPtr callback = Marshal.GetFunctionPointerForDelegate(_assertInterruptCallback);

        ASSERTINTERRUPT fnx = Marshal.GetDelegateForFunctionPointer<ASSERTINTERRUPT>(callback);

        IntPtr p = _d.SetInterruptCallPointer;

        SETINTERRUPTCALLPOINTER fn = Marshal.GetDelegateForFunctionPointer<SETINTERRUPTCALLPOINTER>(p);

        fn(fnx);
    }

    private static PAKMEMREAD8? _readCallback;
    private static PAKMEMWRITE8? _writeCallback;

    private void InvokeDmaMemPointer()
    {
        _readCallback = _modules.TCC1014.MemRead8;
        _writeCallback = _modules.TCC1014.MemWrite8;

        IntPtr callbackRead = Marshal.GetFunctionPointerForDelegate(_readCallback);
        IntPtr callbackWrite = Marshal.GetFunctionPointerForDelegate(_writeCallback);

        PAKMEMREAD8 fnRead = Marshal.GetDelegateForFunctionPointer<PAKMEMREAD8>(callbackRead);
        PAKMEMWRITE8 fnWrite = Marshal.GetDelegateForFunctionPointer<PAKMEMWRITE8>(callbackWrite);

        IntPtr p = _d.DmaMemPointers;

        DMAMEMPOINTERS fn = Marshal.GetDelegateForFunctionPointer<DMAMEMPOINTERS>(p);

        fn(fnRead, fnWrite);
    }

    private static SETCART? _setCartCallback;

    private void InvokePakSetCart()
    {
        _setCartCallback = SetCart;
        IntPtr callback = Marshal.GetFunctionPointerForDelegate(_setCartCallback);

        SETCART fnx = Marshal.GetDelegateForFunctionPointer<SETCART>(callback);

        IntPtr p = _d.PakSetCart;

        PAKSETCART fn = Marshal.GetDelegateForFunctionPointer<PAKSETCART>(p);

        fn(fnx);
    }

    private ushort ReadModuleAudioSample()
    {
        IntPtr p = _d.ModuleAudioSample;

        MODULEAUDIOSAMPLE fn = Marshal.GetDelegateForFunctionPointer<MODULEAUDIOSAMPLE>(p);

        return fn();
    }

    // ReSharper disable once ParameterHidesMember
    private bool SetDelegates(HINSTANCE hInstLib)
    {
        _d.GetModuleName = _kernel.GetProcAddress(hInstLib, "ModuleName");
        _d.ConfigModule = _kernel.GetProcAddress(hInstLib, "ModuleConfig");
        _d.PakPortWrite = _kernel.GetProcAddress(hInstLib, "PackPortWrite");
        _d.PakPortRead = _kernel.GetProcAddress(hInstLib, "PackPortRead");
        _d.SetInterruptCallPointer = _kernel.GetProcAddress(hInstLib, "AssertInterrupt");
        _d.DmaMemPointers = _kernel.GetProcAddress(hInstLib, "MemPointers");
        _d.HeartBeat = _kernel.GetProcAddress(hInstLib, "HeartBeat");
        _d.PakMemWrite8 = _kernel.GetProcAddress(hInstLib, "PakMemWrite8");
        _d.PakMemRead8 = _kernel.GetProcAddress(hInstLib, "PakMemRead8");
        _d.ModuleStatus = _kernel.GetProcAddress(hInstLib, "ModuleStatus");
        _d.ModuleAudioSample = _kernel.GetProcAddress(hInstLib, "ModuleAudioSample");
        _d.ModuleReset = _kernel.GetProcAddress(hInstLib, "ModuleReset");
        _d.SetIniPath = _kernel.GetProcAddress(hInstLib, "SetIniPath");
        _d.PakSetCart = _kernel.GetProcAddress(hInstLib, "SetCart");

        return true; //_d.GetModuleName == null;
    }

    private int ModulePortWrite(byte port, byte data)
    {
        if (_d.PakPortWrite != Zero)
        {
            IntPtr p = _d.PakPortWrite;

            PAKPORTWRITE fn = Marshal.GetDelegateForFunctionPointer<PAKPORTWRITE>(p);

            fn(port, data);

            return 0;
        }

        return 1;
    }

    public byte PakMem8Read(ushort address)
    {
        if (HasModuleMem8Read())
        {
            return ModuleMem8Read(address);
        }

        int offset = (int)((address & 32767) + _bankedCartOffset);

        if (_externalRomBuffer != null)
        {
            //Threading makes it possible to reach here where ExternalRomBuffer = NULL despite check.
            lock (_lock)
            {
                if (_externalRomBuffer != null)
                {
                    return _externalRomBuffer[offset];
                }
            }
        }

        return 0;
    }

    private bool HasModulePortRead()
    {
        return _d.PakPortRead != Zero;
    }

    private byte ModulePortRead(byte port)
    {
        IntPtr p = _d.PakPortRead;

        PAKPORTREAD fn = Marshal.GetDelegateForFunctionPointer<PAKPORTREAD>(p);

        return fn(port);
    }

    private bool HasModuleMem8Read()
    {
        return _d.PakMemRead8 != Zero;
    }

    private byte ModuleMem8Read(ushort address)
    {
        IntPtr p = _d.PakMemRead8;

        PAKMEMREAD8 fn = Marshal.GetDelegateForFunctionPointer<PAKMEMREAD8>(p);

        return fn((ushort)(address & 32767));
    }

    public void ModuleReset()
    {
        _dllPath = null;

        CartInserted = 0;
        ModuleName = "Blank";

        _romPackLoaded = false;
        _bankedCartOffset = 0;

        _externalRomBuffer = new byte[Define.PAK_MAX_MEM];

        _hInstLib = Zero;

        //InsertModule();   // Should this be here?
    }
}
