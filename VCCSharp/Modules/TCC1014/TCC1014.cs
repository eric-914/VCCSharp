using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Modules.TCC1014.Masks;
using VCCSharp.Modules.TCC1014.Modes;

namespace VCCSharp.Modules.TCC1014;

// ReSharper disable InconsistentNaming
// ReSharper disable CommentTypo
public interface ITCC1014 : IModule
{
    void MC6883Reset();
    void CopyRom();
    void MmuReset();
    bool MmuInit(MemorySizes ramSizeOption);
    byte MemRead8(ushort address);
    void MemWrite8(byte data, ushort address);
    void GimeAssertVerticalInterrupt();
    void GimeAssertHorizontalInterrupt();
    void GimeAssertTimerInterrupt();
    ushort GetMem(int address);
    void SetMapType(byte type);

    byte SAMRead(byte port);
    void SAMWrite(byte data, byte port);
    byte GimeRead(byte port);
    void GimeWrite(byte port, byte data);
    void GimeAssertKeyboardInterrupt();
    ModeModel GetMode();
}
// ReSharper restore InconsistentNaming

// ReSharper disable once InconsistentNaming
// ReSharper disable once ClassNeverInstantiated.Global
public class TCC1014 : ITCC1014
{
    private readonly IModules _modules;
    private readonly IRomLoader _romLoader;

    private IGraphics Graphics => _modules.Graphics;
    private ICPU CPU => _modules.CPU;

    private readonly RamMask _ramMask = new();
    
    private readonly VectorMasks _vectorMask = new();
    private readonly VectorMasksAlt _vectorMaskAlt = new();

    private readonly GIME _gime = new();

    private byte _ramVectors;	// $FF90 bit 3
    private byte _romMap;		// $FF90 bit 1-0
    private byte _mapType;	    // $FFDE/FFDF toggle Map type 0 = ram/rom

    public TCC1014(IModules modules, IRomLoader romLoader)
    {
        _modules = modules;
        _romLoader = romLoader;
    }

    public void MC6883Reset()
    {
        _gime.VDG.Reset();
        _gime.MMU.ROM.Reset(_gime.MMU.IRB);
    }

    //TODO: Used by MmuInit()
    public void CopyRom()
    {
        _romLoader.CopyRom(_gime.MMU.IRB);
    }

    public void MmuReset()
    {
        _gime.MMU.Reset();

        _ramVectors = 0;
        
        _romMap = 0;
        _mapType = 0;

        for (int index = 0; index < 1024; index++)
        {
            _gime.MMU.Pages[index] = _gime.MMU.RAM.GetBytePointer((index & _ramMask[_gime.MMU.CurrentRamConfiguration]) * 0x2000);
            _gime.MMU.PageOffsets[index] = 1;
        }

        SetRomMap(0);
        SetMapType(0);
    }

    /*****************************************************************************************
    * MmuInit Initialize and allocate memory for RAM Internal and External ROM Images.        *
    * Copy Rom Images to buffer space and reset GIME MMU registers to 0                      *
    * Returns NULL if any of the above fail.                                                 *
    *****************************************************************************************/
    public bool MmuInit(MemorySizes ramSizeOption)
    {
        _gime.MMU.Reset(ramSizeOption);

        Graphics.SetVidMask(_gime.MMU.CurrentRamConfiguration);

        CopyRom();
        MmuReset();

        return true;
    }

    public void GimeAssertVerticalInterrupt()
    {
        if ((_gime.Registers[Ports.FIRQENR] & 8) != 0 && _gime.Interrupts.FIRQ.EnhancedFlag)
        {
            CPU.AssertInterrupt(CPUInterrupts.FIRQ, 0); //FIRQ

            _gime.Interrupts.FIRQ.Last |= InterruptFlags.Vertical;
        }
        else if ((_gime.Registers[Ports.IRQENR] & 8) != 0 && _gime.Interrupts.IRQ.EnhancedFlag)
        {
            CPU.AssertInterrupt(CPUInterrupts.IRQ, 0); //IRQ moon patrol demo using this

            _gime.Interrupts.IRQ.Last |= InterruptFlags.Vertical;
        }
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

                return _modules.PAKInterface.PakMem8Read((ushort)(_gime.MMU.PageOffsets[mmu] + mask));
            }

            case > 0xFEFF:
                return _modules.IOBus.PortRead(address);

            default:
                return VectorMemRead8(address);
        }
    }

    private byte VectorMemRead8(ushort address)
    {
        if (_ramVectors != 0)
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
                _modules.IOBus.PortWrite(data, address);

                break;

            default:
                VectorMemWrite8(data, address);
                break;
        }
    }

    private void VectorMemWrite8(byte data, ushort address)
    {
        if (_ramVectors != 0)
        {
            //Address must be $FE00 - $FEFF
            _gime.MMU.RAM[(0x2000 * _vectorMask[_gime.MMU.CurrentRamConfiguration]) | (address & 0x1FFF)] = data;
        }
        else
        {
            MemWrite8(data, address);
        }
    }

    //--I think this is just a hack to access memory directly for the 40/80 char-wide screen-scrapes
    public ushort GetMem(int address)
    {
        return _gime.MMU.RAM[address];
    }

    public byte SAMRead(byte port)
    {
        if (port >= 0xF0) // && port <= 0xFF)
        {
            //IRQ vectors from rom
            return _gime.MMU.ROM[0x3F00 + port];
        }

        return 0;
    }

    public void SAMWrite(byte data, byte port)
    {
        byte mask;
        byte reg;

        if (port is >= 0xC6 and <= 0xD3)   //VDG Display offset Section
        {
            port -= 0xC6;
            reg = (byte)((port & 0x0E) >> 1);
            mask = (byte)(1 << reg);

            _gime.VDG.DisplayOffset = (byte)(_gime.VDG.DisplayOffset & (0xFF - mask)); //Shut the bit off

            if ((port & 1) != 0)
            {
                _gime.VDG.DisplayOffset |= mask;
            }

            Graphics.SetGimeVdgOffset(_gime.VDG.DisplayOffset);
        }

        if (port is >= 0xC0 and <= 0xC5)   //VDG Mode
        {
            port -= 0xC0;
            reg = (byte)((port & 0x0E) >> 1);
            mask = (byte)(1 << reg);

            _gime.VDG.Mode = (byte)(_gime.VDG.Mode & (0xFF - mask));

            if ((port & 1) != 0)
            {
                _gime.VDG.Mode |= mask;
            }

            Graphics.SetGimeVdgMode(_gime.VDG.Mode);
        }

        if (port is 0xDE or 0xDF)
        {
            SetMapType((byte)(port & 1));
        }

        if (port is 0xD7 or 0xD9)
        {
            _modules.Emu.SetCpuMultiplierFlag(1);
        }

        if (port is 0xD6 or 0xD8)
        {
            _modules.Emu.SetCpuMultiplierFlag(0);
        }
    }

    public byte GimeRead(byte port)
    {
        switch (port)
        {
            case Ports.IRQENR:
                return _gime.ReadIRQ();

            case Ports.FIRQENR:
                return _gime.ReadFIRQ();

            case Ports.TIMER_MS: //--Timer
            case Ports.TIMER_LS:
                return 126; //--What is this magic number?

            default:
                return _gime.Registers[port];
        }
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

            case Ports.RESERVED_96: //0x96 -- TODO: Is this TurboMode or Reserved?  65495 maps to $FFD7
                _modules.Emu.SetTurboMode((byte)(data & 1));
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
        SetMmuEnabled((data & 64) == 0);
        SetRomMap((byte)(data & 3)); //MC0-MC1
        SetVectors((byte)(data & 8)); //MC3

        _gime.Interrupts.FIRQ.SetFlag((byte)((data & 16) >> 4));
        _gime.Interrupts.IRQ.SetFlag((byte)((data & 32) >> 5));
    }

    private void SetInit1(byte data)
    {
        SetMmuTask((byte)(data & 1));                       //TR
        _modules.CoCo.SetTimerClockRate((byte)(data & 32));	//TINS
    }

    private void SetTimerMsb() => _modules.CoCo.SetInterruptTimer(Timer);

    private void SetTimerLsb() => _modules.CoCo.SetInterruptTimer(Timer);

    private ushort Timer => (ushort)(((_gime.Registers[Ports.TIMER_MS] << 8) + _gime.Registers[Ports.TIMER_LS]) & 0x0FFF); //--Limit to 12-bit value

    private void SetGimeIRQSteering()
    {
        bool TestMask(int address, int mask) => (_gime.Registers[address] & mask) != 0;
        byte Test(int mask) => TestMask(Ports.IRQENR, mask) | TestMask(Ports.FIRQENR, mask) ? (byte)1 : (byte)0;

        _modules.Keyboard.GimeSetKeyboardInterruptState(Test(2));
        _modules.CoCo.SetVerticalInterruptState(Test(8));
        _modules.CoCo.SetHorizontalInterruptState(Test(16));
        _modules.CoCo.SetTimerInterruptState(Test(32));
    }

    //--TODO: Not sure why this is the same as IRQ above
    private void SetGimeFIRQSteering()
    {
        bool TestMask(int address, int mask) => (_gime.Registers[address] & mask) != 0;
        byte Test(int mask) => TestMask(Ports.IRQENR, mask) | TestMask(Ports.FIRQENR, mask) ? (byte)1 : (byte)0;

        _modules.Keyboard.GimeSetKeyboardInterruptState(Test(2));
        _modules.CoCo.SetVerticalInterruptState(Test(8));
        _modules.CoCo.SetHorizontalInterruptState(Test(16));
        _modules.CoCo.SetTimerInterruptState(Test(32));
    }

    public void GimeAssertHorizontalInterrupt()
    {
        if ((_gime.Registers[Ports.FIRQENR] & 16) != 0 && _gime.Interrupts.FIRQ.EnhancedFlag)
        {
            CPU.AssertInterrupt(CPUInterrupts.FIRQ, 0);

            _gime.Interrupts.FIRQ.Last |= InterruptFlags.Horizontal;
        }
        else if ((_gime.Registers[Ports.IRQENR] & 16) != 0 && _gime.Interrupts.IRQ.EnhancedFlag)
        {
            CPU.AssertInterrupt(CPUInterrupts.IRQ, 0);

            _gime.Interrupts.IRQ.Last |= InterruptFlags.Horizontal;
        }
    }

    public void GimeAssertTimerInterrupt()
    {
        if ((_gime.Registers[Ports.FIRQENR] & 32) != 0 && _gime.Interrupts.FIRQ.EnhancedFlag)
        {
            CPU.AssertInterrupt(CPUInterrupts.FIRQ, 0);

            _gime.Interrupts.FIRQ.Last |= InterruptFlags.Timer;
        }
        else if ((_gime.Registers[Ports.IRQENR] & 32) != 0 && _gime.Interrupts.IRQ.EnhancedFlag)
        {
            CPU.AssertInterrupt(CPUInterrupts.IRQ, 0);

            _gime.Interrupts.IRQ.Last |= InterruptFlags.Timer;
        }
    }

    public void SetMapType(byte type)
    {
        _mapType = type; //={0-ROM,1-RAM}

        UpdateMmuArray();
    }

    private void SetRomMap(byte data)
    {
        _romMap = (byte)(data & 3);

        UpdateMmuArray();
    }

    private void UpdateMmuArray()
    {
        if (_mapType != 0)
        {
            _gime.MMU.Pages[_vectorMask[_gime.MMU.CurrentRamConfiguration] - 3] = _gime.MMU.RAM.GetBytePointer(0x2000 * (_vectorMask[_gime.MMU.CurrentRamConfiguration] - 3));
            _gime.MMU.Pages[_vectorMask[_gime.MMU.CurrentRamConfiguration] - 2] = _gime.MMU.RAM.GetBytePointer(0x2000 * (_vectorMask[_gime.MMU.CurrentRamConfiguration] - 2));
            _gime.MMU.Pages[_vectorMask[_gime.MMU.CurrentRamConfiguration] - 1] = _gime.MMU.RAM.GetBytePointer(0x2000 * (_vectorMask[_gime.MMU.CurrentRamConfiguration] - 1));
            _gime.MMU.Pages[_vectorMask[_gime.MMU.CurrentRamConfiguration]] = _gime.MMU.RAM.GetBytePointer(0x2000 * _vectorMask[_gime.MMU.CurrentRamConfiguration]);

            _gime.MMU.PageOffsets[_vectorMask[_gime.MMU.CurrentRamConfiguration] - 3] = 1;
            _gime.MMU.PageOffsets[_vectorMask[_gime.MMU.CurrentRamConfiguration] - 2] = 1;
            _gime.MMU.PageOffsets[_vectorMask[_gime.MMU.CurrentRamConfiguration] - 1] = 1;
            _gime.MMU.PageOffsets[_vectorMask[_gime.MMU.CurrentRamConfiguration]] = 1;

            return;
        }

        switch (_romMap)
        {
            case 0:
            case 1: //16K Internal 16K External
                _gime.MMU.Pages[_vectorMask[_gime.MMU.CurrentRamConfiguration] - 3] = _gime.MMU.IRB.GetBytePointer(0x0000);
                _gime.MMU.Pages[_vectorMask[_gime.MMU.CurrentRamConfiguration] - 2] = _gime.MMU.IRB.GetBytePointer(0x2000);
                _gime.MMU.Pages[_vectorMask[_gime.MMU.CurrentRamConfiguration] - 1] = new BytePointer();
                _gime.MMU.Pages[_vectorMask[_gime.MMU.CurrentRamConfiguration]] = new BytePointer();

                _gime.MMU.PageOffsets[_vectorMask[_gime.MMU.CurrentRamConfiguration] - 3] = 1;
                _gime.MMU.PageOffsets[_vectorMask[_gime.MMU.CurrentRamConfiguration] - 2] = 1;
                _gime.MMU.PageOffsets[_vectorMask[_gime.MMU.CurrentRamConfiguration] - 1] = 0;
                _gime.MMU.PageOffsets[_vectorMask[_gime.MMU.CurrentRamConfiguration]] = 0x2000;

                return;

            case 2: // 32K Internal
                _gime.MMU.Pages[_vectorMask[_gime.MMU.CurrentRamConfiguration] - 3] = _gime.MMU.IRB.GetBytePointer(0x0000);
                _gime.MMU.Pages[_vectorMask[_gime.MMU.CurrentRamConfiguration] - 2] = _gime.MMU.IRB.GetBytePointer(0x2000);
                _gime.MMU.Pages[_vectorMask[_gime.MMU.CurrentRamConfiguration] - 1] = _gime.MMU.IRB.GetBytePointer(0x4000);
                _gime.MMU.Pages[_vectorMask[_gime.MMU.CurrentRamConfiguration]] = _gime.MMU.IRB.GetBytePointer(0x6000);

                _gime.MMU.PageOffsets[_vectorMask[_gime.MMU.CurrentRamConfiguration] - 3] = 1;
                _gime.MMU.PageOffsets[_vectorMask[_gime.MMU.CurrentRamConfiguration] - 2] = 1;
                _gime.MMU.PageOffsets[_vectorMask[_gime.MMU.CurrentRamConfiguration] - 1] = 1;
                _gime.MMU.PageOffsets[_vectorMask[_gime.MMU.CurrentRamConfiguration]] = 1;

                return;

            case 3: //32K External
                _gime.MMU.Pages[_vectorMask[_gime.MMU.CurrentRamConfiguration] - 1] = new BytePointer();
                _gime.MMU.Pages[_vectorMask[_gime.MMU.CurrentRamConfiguration]] = new BytePointer();
                _gime.MMU.Pages[_vectorMask[_gime.MMU.CurrentRamConfiguration] - 3] = new BytePointer();
                _gime.MMU.Pages[_vectorMask[_gime.MMU.CurrentRamConfiguration] - 2] = new BytePointer();

                _gime.MMU.PageOffsets[_vectorMask[_gime.MMU.CurrentRamConfiguration] - 1] = 0;
                _gime.MMU.PageOffsets[_vectorMask[_gime.MMU.CurrentRamConfiguration]] = 0x2000;
                _gime.MMU.PageOffsets[_vectorMask[_gime.MMU.CurrentRamConfiguration] - 3] = 0x4000;
                _gime.MMU.PageOffsets[_vectorMask[_gime.MMU.CurrentRamConfiguration] - 2] = 0x6000;

                return;
        }
    }

    private void SetMmuTask(byte task)
    {
        _gime.MMU.Task = task;
        _gime.MMU.State = (byte)((_gime.MMU.Enabled ? 1 : 0) << 1 | _gime.MMU.Task);
    }

    private void SetMmuRegister(byte register, byte data)
    {
        byte bankRegister = (byte)(register & 7);
        byte task = (byte)((register & 8) == 0 ? 0 : 1);

        //gime.c returns what was written so I can get away with this
        _gime.MMU.Registers[task, bankRegister] = (ushort)(_gime.MMU.Prefix | (data & _ramMask[_gime.MMU.CurrentRamConfiguration]));
    }

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

    private void SetMmuPrefix(byte data)
    {
        _gime.MMU.Prefix = (ushort)((data & 3) << 8);
    }

    private void SetVectors(byte data)
    {
        _ramVectors = (byte)(data == 0 ? 0 : 1); //Bit 3 of $FF90 MC3
    }

    private void SetMmuEnabled(bool flag)
    {
        _gime.MMU.Enabled = flag;
        _gime.MMU.State = (byte)((_gime.MMU.Enabled ? 1 : 0) << 1 | _gime.MMU.Task);
    }

    public void GimeAssertKeyboardInterrupt()
    {
        if ((_gime.Registers[Ports.FIRQENR] & 2) != 0 && (_gime.Interrupts.FIRQ.EnhancedFlag))
        {
            CPU.AssertInterrupt(CPUInterrupts.FIRQ, 0);

            _gime.Interrupts.FIRQ.Last |= InterruptFlags.Keyboard;
        }
        else if ((_gime.Registers[Ports.IRQENR] & 2) != 0 && (_gime.Interrupts.IRQ.EnhancedFlag))
        {
            CPU.AssertInterrupt(CPUInterrupts.IRQ, 0);

            _gime.Interrupts.IRQ.Last |= InterruptFlags.Keyboard;
        }
    }

    public ModeModel GetMode()
    {
        return new ModeModel(_gime.MMU.RAM, _modules);
    }

    public void Reset()
    {
        _ramVectors = 0;
        _romMap = 0;
        _mapType = 0;

        _gime.Initialize();
    }
}
