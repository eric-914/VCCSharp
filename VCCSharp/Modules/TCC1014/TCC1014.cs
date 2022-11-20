using System.Diagnostics;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Models.Configuration;
using VCCSharp.Modules.TCC1014.Masks;
using VCCSharp.Modules.TCC1014.Modes;

namespace VCCSharp.Modules.TCC1014;

// ReSharper disable InconsistentNaming
// ReSharper disable CommentTypo
public interface ITCC1014 : IModule, IChip
{
    void Initialize(MemorySizes ramSizeOption);

    byte MemRead8(ushort address);
    void MemWrite8(byte data, ushort address);

    byte SAMRead(byte port);
    void SAMWrite(byte data, byte port);

    byte GimeRead(byte port);
    void GimeWrite(byte port, byte data);

    byte VectorRead(byte port);
    void VectorWrite(byte data, byte port);

    void GimeAssertKeyboardInterrupt();
    void GimeAssertVerticalInterrupt();
    void GimeAssertHorizontalInterrupt();
    void GimeAssertTimerInterrupt();

    ModeModel GetMode();
}
// ReSharper restore InconsistentNaming

// ReSharper disable once InconsistentNaming
// ReSharper disable once ClassNeverInstantiated.Global
public class TCC1014 : ITCC1014
{
    private readonly IModules _modules;

    private IGraphics Graphics => _modules.Graphics;
    private IIOBus IO => _modules.IOBus;
    private IPAKInterface PAK => _modules.PAKInterface;
    private IEmu Emu => _modules.Emu;
    private ICoCo CoCo => _modules.CoCo;
    private IKeyboard Keyboard => _modules.Keyboard;
    private IConfiguration Configuration => _modules.ConfigurationManager.Model;

    private readonly VectorMasks _vectorMask = new();
    private readonly VectorMasksAlt _vectorMaskAlt = new();

    private readonly IGIME _gime;

    private XFEXX _ramVectors;	    // $FF90 bit 3      1 = DRAM at XFEXX is constant
    private RomMapping _romMap;		// $FF90 bit 1-0
    private MapTypes _mapType;	    // $FFDE/FFDF toggle Map type 0 = ram/rom

    public TCC1014(IModules modules, IGIME gime)
    {
        _modules = modules;
        _gime = gime;
    }

    /*****************************************************************************
    * Initialize and allocate memory for RAM Internal and External ROM Images.   *
    * Copy Rom Images to buffer space and reset GIME MMU registers to 0          *
    * Returns NULL if any of the above fail.                                     *
    ******************************************************************************/

    public void Initialize(MemorySizes ramSizeOption)
    {
        _gime.MMU.Reset(ramSizeOption);

        Graphics.SetVidMask(_gime.MMU.CurrentRamConfiguration);
    }
    
    public byte MemRead8(ushort address)
    {
        switch (address)
        {
            case < 0xFE00:
                {
                    ushort index = (ushort)(address >> 13);
                    ushort mask = (ushort)(address & 0x1FFF);

                    ushort mmu = _gime.MMU.Registers[_gime.MMU.State, index];

                    if (_gime.MMU.PageOffsets[mmu] == 1)
                    {
                        return _gime.MMU.Pages[mmu][mask];
                    }

                    return PAK.PakMem8Read((ushort)(_gime.MMU.PageOffsets[mmu] + mask));
                }

            case > 0xFEFF:
                return IO.PortRead(address);

            default:
                return VectorMemRead8(address);
        }
    }

    private byte VectorMemRead8(ushort address)
    {
        if (_ramVectors == XFEXX.Vector)
        {
            //Address must be $FE00 - $FEFF
            return _gime.MMU.RAM[(0x2000 * _vectorMask[_gime.MMU.CurrentRamConfiguration]) | (address & 0x1FFF)];
        }

        return MemRead8(address);
    }

    public void MemWrite8(byte data, ushort address)
    {
        switch (address)
        {
            case < 0xFE00:
                {
                    ushort index = (ushort)(address >> 13);
                    ushort mask = (ushort)(address & 0x1FFF);

                    ushort mmu = _gime.MMU.Registers[_gime.MMU.State, index];

                    byte maskA = _vectorMaskAlt[_gime.MMU.CurrentRamConfiguration];
                    byte maskB = _vectorMask[_gime.MMU.CurrentRamConfiguration];

                    if (_mapType != 0 || mmu < maskA || mmu > maskB)
                    {
                        _gime.MMU.Pages[mmu][mask] = data;
                    }

                    break;
                }

            case > 0xFEFF:
                IO.PortWrite(data, address);

                break;

            default:
                VectorMemWrite8(data, address);
                break;
        }
    }

    private void VectorMemWrite8(byte data, ushort address)
    {
        if (_ramVectors == XFEXX.Vector)
        {
            //Address must be $FE00 - $FEFF
            _gime.MMU.RAM[(0x2000 * _vectorMask[_gime.MMU.CurrentRamConfiguration]) | (address & 0x1FFF)] = data;
        }
        else
        {
            MemWrite8(data, address);
        }
    }

    //$FFC0 - $FFDF -- 0
    public byte SAMRead(byte port)
    {
        //Reading from this range is ignored.
        return 0;
    }

    //$FFC0 - $FFDF
    public void SAMWrite(byte data, byte port)
    {
        SAMHandlers handler = Ports.SAMHandler(port);

        switch (handler)
        {
            case SAMHandlers.DisplayModelControl:   //0xC0-0xC5
                SetGimeVdgMode(port);
                break;

            case SAMHandlers.DisplayOffset:         //0xC6-0xD3
                SetGimeVdgOffset(port);
                break;

            case SAMHandlers.Page_1:                //0xD4-0xD5
                //--Deprecated for CoCo3
                break;

            case SAMHandlers.CPURate:               //0xD6-0xD9
                //0xD6-0xD7  POKE 65495,0 :: COCO 1/2
                //0xD8-0xD9  POKE 65497,0 :: COCO 3
                Emu.SetCpuMultiplierFlag((byte)(port & 1));
                break;

            case SAMHandlers.MemorySize:            //0xDA-0xDD
                //--Deprecated for CoCo3
                break;

            case SAMHandlers.MapType:               //0xDE-0xDF
                UpdateMmuArray((MapTypes)(port & 1), _romMap);
                break;
        }
    }

    //$FFF0 - $FFFF
    public byte VectorRead(byte port)
    {
        //F0-F1 Illegal opcode and ÷ by zero
        //F2-F3 SWI3
        //F4-F5 SWI2
        //F6-F7 FIRQ
        //F8-F9 IRQ
        //FA-FB SWI
        //FC-FD NMI
        //FE-FF Reset

        //These are "ghosted" from the end of the BASIC ROM at BFF0
        return _gime.MMU.ROM[0x3F00 + port];
    }

    //$FFF0 - $FFFF
    public void VectorWrite(byte data, byte port)
    {
        //--Writes to this range are ignored
    }

    private void SetGimeVdgOffset(byte port)
    {
        port -= 0xC6;

        byte reg = (byte)((port & 0x0E) >> 1);
        byte mask = (byte)(1 << reg);

        _gime.VDG.DisplayOffset = (byte)(_gime.VDG.DisplayOffset & (0xFF - mask)); //Shut the bit off

        if ((port & 1) != 0)
        {
            _gime.VDG.DisplayOffset |= mask;
        }

        Graphics.SetGimeVdgOffset(_gime.VDG.DisplayOffset);
    }

    private void SetGimeVdgMode(byte port)
    {
        port -= 0xC0;

        byte reg = (byte)((port & 0x0E) >> 1);
        byte mask = (byte)(1 << reg);

        _gime.VDG.Mode = (byte)(_gime.VDG.Mode & (0xFF - mask));

        if ((port & 1) != 0)
        {
            _gime.VDG.Mode |= mask;
        }

        Graphics.SetGimeVdgMode(_gime.VDG.Mode);
    }

    public byte GimeRead(byte port)
    {
        return port switch
        {
            Ports.IRQENR => //0x92
                _gime.ReadIRQ(),

            Ports.FIRQENR => //0x93
                _gime.ReadFIRQ(),

            Ports.TIMER_MS => //0x94 //--Timer
                126, //--What is this magic number?

            Ports.TIMER_LS => //0x95
                126, //--What is this magic number?

            _ => _gime.Registers[port]
        };
    }

    public void GimeWrite(byte port, byte data)
    {
        _gime.Registers[port] = data;

        switch (port)
        {
            case Ports.INITO: //0x90
                SetInit0(data);
                break;

            case Ports.INIT1: //0x91
                SetInit1(data);
                break;

            case Ports.IRQENR: //0x92
                SetGimeIRQSteering();
                break;

            case Ports.FIRQENR: //0x93
                SetGimeFIRQSteering();
                break;

            case Ports.TIMER_MS: //0x94
                SetTimerMsb();
                break;

            case Ports.TIMER_LS: //0x95
                SetTimerLsb();
                break;

            case Ports.RESERVED_96: //0x96
                break;

            case Ports.RESERVED_97: //0x97
                break;

            case Ports.VideoModeRegister: //0x98
                Graphics.SetGimeVmode(data);
                break;

            case Ports.VideoResolutionRegister: //0x99
                Graphics.SetGimeVres(data);
                break;

            case Ports.BorderRegister: //0x9A
                Graphics.SetGimeBorderColor(data);
                break;

            case Ports.RESERVED_9B: //0x9B -- TODO: Another RESERVED in use?
                SetVideoOffsetRamBank(data);
                break;

            case Ports.VerticalScrollRegister: // 0x9C -- TODO: Probably missing something here.
                break;

            case Ports.VerticalOffset1Register: //0x9D
            case Ports.VerticalOffset0Register: //0x9E
                Graphics.SetVerticalOffsetRegister((ushort)((_gime.Registers[Ports.VerticalOffset1Register] << 8) | _gime.Registers[Ports.VerticalOffset0Register]));
                break;

            case Ports.HorizontalOffsetRegister: //0x9F
                Graphics.SetGimeHorizontalOffset(data);
                break;

            case >= 0xA0 and <= 0xAF:
                SetMmuRegister(port, data);
                break;

            case >= 0xB0 and <= 0xBF:
                Graphics.SetGimePalette((byte)(port - 0xB0), (byte)(data & 63));
                break;
        }
    }

    private void SetInit0(byte data)
    {
        Graphics.SetCompatMode((data & 128) == 0 ? CompatibilityModes.CoCo3 : CompatibilityModes.CoCo2);

        _gime.MMU.SetEnabled((data & 64) == 0);

        UpdateMmuArray(_mapType, (RomMapping)(data & 3));   //MC0-MC1

        _ramVectors = (XFEXX)(data & 8);      //Bit 3 of $FF90 MC3

        _gime.Interrupts.SetFlags(data);
    }

    private void SetInit1(byte data)
    {
        SetMmuTask((byte)(data & 1));               //TR
        CoCo.SetTimerClockRate((byte)(data & 32));	//TINS
    }

    private void SetTimerMsb() => CoCo.SetInterruptTimer(Timer);

    private void SetTimerLsb() => CoCo.SetInterruptTimer(Timer);

    private ushort Timer => (ushort)(((_gime.Registers[Ports.TIMER_MS] << 8) + _gime.Registers[Ports.TIMER_LS]) & 0x0FFF); //--Limit to 12-bit value

    private void SetGimeIRQSteering()
    {
        bool TestMask(int address, int mask) => (_gime.Registers[address] & mask) != 0;
        byte Test(int mask) => TestMask(Ports.IRQENR, mask) | TestMask(Ports.FIRQENR, mask) ? (byte)1 : (byte)0;

        Keyboard.GimeSetKeyboardInterruptState(Test(2));
        CoCo.SetVerticalInterruptState(Test(8));
        CoCo.SetHorizontalInterruptState(Test(16));
        CoCo.SetTimerInterruptState(Test(32));
    }

    //--TODO: Not sure why this is the same as IRQ above
    private void SetGimeFIRQSteering()
    {
        bool TestMask(int address, int mask) => (_gime.Registers[address] & mask) != 0;
        byte Test(int mask) => TestMask(Ports.IRQENR, mask) | TestMask(Ports.FIRQENR, mask) ? (byte)1 : (byte)0;

        Keyboard.GimeSetKeyboardInterruptState(Test(2));
        CoCo.SetVerticalInterruptState(Test(8));
        CoCo.SetHorizontalInterruptState(Test(16));
        CoCo.SetTimerInterruptState(Test(32));
    }

    private void UpdateMmuArray(MapTypes mapType, RomMapping romMap)
    {
        _mapType = mapType;
        _romMap = romMap;

        switch (_mapType)
        {
            case MapTypes.RAM:
                _gime.SetMapTypeRam();
                break;

            case MapTypes.ROM:
                _gime.SetMapTypeRom(_romMap);
                break;
        }
    }

    private void SetMmuTask(byte task)
    {
        _gime.MMU.Task = task;
        _gime.MMU.State = (byte)((_gime.MMU.Enabled ? 1 : 0) << 1 | _gime.MMU.Task);
    }

    private void SetMmuRegister(byte register, byte data) => _gime.MMU.SetRegister(register, data);

    private void SetVideoOffsetRamBank(byte data)
    {
        switch (_gime.MMU.CurrentRamConfiguration)
        {
            case MemorySizes._4K:
            case MemorySizes._16K:
            case MemorySizes._32K:
            case MemorySizes._64K:
            case MemorySizes._128K:
            case MemorySizes._512K:
                return;

            case MemorySizes._2048K:
                Graphics.SetVideoBank((byte)(data & 3));
                SetMmuPrefix(0);

                return;

            case MemorySizes._8192K: //No Can 3 
                Graphics.SetVideoBank((byte)(data & 0x0F));
                SetMmuPrefix((byte)((data & 0x30) >> 4));

                return;
        }
    }

    private void SetMmuPrefix(byte data) => _gime.MMU.Prefix = (ushort)((data & 3) << 8);

    public void GimeAssertKeyboardInterrupt() => _gime.AssertKeyboardInterrupt();

    public void GimeAssertVerticalInterrupt() => _gime.AssertVerticalInterrupt();

    public void GimeAssertHorizontalInterrupt() => _gime.AssertHorizontalInterrupt();

    public void GimeAssertTimerInterrupt() => _gime.AssertTimerInterrupt();

    public ModeModel GetMode() => new(_gime.MMU.RAM, _modules);

    public void ModuleReset() => ChipReset();

    public void ChipReset()
    {
        Debug.WriteLine("TCC1014.ChipReset()");

        Initialize(Configuration.Memory.Ram.Value);

        _ramVectors = XFEXX.RAM;

        _gime.Initialize();
        _gime.VDG.Reset();

        _gime.LoadRom();
        _gime.MMU.Reset();
        _gime.MMU.ResetPages();

        UpdateMmuArray(MapTypes.ROM, RomMapping._16kInternal_16kExternal);
    }
}
