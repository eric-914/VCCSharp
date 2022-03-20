using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;

namespace VCCSharp.Modules.TC1014;

// ReSharper disable InconsistentNaming
// ReSharper disable CommentTypo
public interface ITC1014
{
    void MC6883Reset();
    void CopyRom();
    void MmuReset();
    bool MmuInit(byte ramSizeOption);
    byte MemRead8(ushort address);
    void MemWrite8(byte data, ushort address);
    void GimeAssertVerticalInterrupt();
    void GimeAssertHorizontalInterrupt();
    void GimeAssertTimerInterrupt();
    ushort GetMem(int address);
    void SetMapType(byte type);

    void DrawBottomBorder32(int lineCounter);
    void DrawTopBorder32(int lineCounter);
    void UpdateScreen(int lineCounter);

    byte SAMRead(byte port);
    void SAMWrite(byte data, byte port);
    byte GimeRead(byte port);
    void GimeWrite(byte port, byte data);
    void GimeAssertKeyboardInterrupt();
}
// ReSharper restore InconsistentNaming

// ReSharper disable once InconsistentNaming
public partial class TC1014 : ITC1014
{
    private readonly IModules _modules;
    private IGraphics Graphics => _modules.Graphics;

    private readonly uint[] _memConfig = { 0x20000, 0x80000, 0x200000, 0x800000 };
    private readonly ushort[] _ramMask = { 15, 63, 255, 1023 };
    private readonly byte[] _stateSwitch = { 8, 56, 56, 56 };
    private readonly uint[] _vidMask = { 0x1FFFF, 0x7FFFF, 0x1FFFFF, 0x7FFFFF };

    private readonly byte[] _vectorMask = { 15, 63, 63, 63 };
    private readonly byte[] _vectorMaskA = { 12, 60, 60, 60 };

    private byte _vdgMode;
    private byte _disOffset;

    private byte _mmuTask;	    // $FF91 bit 0
    private bool _mmuEnabled;	// $FF90 bit 6
    private byte _ramVectors;	// $FF90 bit 3

    private byte _romMap;		// $FF90 bit 1-0

    private ushort _mmuPrefix;

    private readonly BytePointer _ram = new();
    private readonly BytePointer _rom = new();
    private readonly BytePointer _irb = new(); //--Internal ROM buffer

    private byte _enhancedFIRQFlag;
    private byte _enhancedIRQFlag;
    private byte _lastIrq;
    private byte _lastFirq;

    private byte _mmuState;	// Composite variable handles MmuTask and MmuEnabled
    private byte _mapType;	// $FFDE/FFDF toggle Map type 0 = ram/rom

    private byte _currentRamConfig = 1;

    private readonly byte[] _gimeRegisters = new byte[256];
    private readonly ushort[] _memPageOffsets = new ushort[1024];
    private readonly ushort[,] _mmuRegisters = new ushort[4, 8];	// $FFA0 - FFAF
    private readonly BytePointer[] _memPages = new BytePointer[1024];

    public TC1014(IModules modules)
    {
        _modules = modules;

        InitializeModes();
    }

    public void MC6883Reset()
    {
        _vdgMode = 0;
        _disOffset = 0;

        _rom.Reset(_irb);
    }

    //TODO: Used by MmuInit()
    public void CopyRom()
    {
        const string rom = "coco3.rom";

        var configuration = _modules.Configuration;

        //--Try loading from Vcc.ini >> CoCoRomPath
        string cocoRomPath = configuration.FilePaths.Rom;

        string path = Path.Combine(configuration.FilePaths.Rom, rom);

        if (LoadInternalRom(path))
        {
            Debug.WriteLine($"Found {rom} in CoCoRomPath");
            return;
        }

        //--Try loading from Vcc.ini >> ExternalBasicImage
        string externalBasicImage = _modules.Configuration.Memory.ExternalBasicImage;

        if (!string.IsNullOrEmpty(externalBasicImage) && LoadInternalRom(externalBasicImage))
        {
            Debug.WriteLine($"Found {rom} in ExternalBasicImage");
            return;
        }

        //--Try loading from current executable folder
        string? exePath = Path.GetDirectoryName(_modules.Vcc.GetExecPath());
        if (exePath == null)
        {
            throw new Exception("Missing .EXE path?");
        }

        string exeFile = Path.Combine(exePath, rom);

        if (LoadInternalRom(exeFile))
        {
            Debug.WriteLine($"Found {rom} in executable folder");
            return;
        }

        //--Give up...
        string message = @$"
Could not locate {rom} in any of these locations:
* Vcc.ini >> CoCoRomPath=""{cocoRomPath}""
* Vcc.ini >> ExternalBasicImage=""{externalBasicImage}""
* In the same folder as the executable: ""{exePath}""
";

        MessageBox.Show(message, "Error");

        Environment.Exit(0);
    }

    public void MmuReset()
    {
        _mmuTask = 0;
        _mmuEnabled = false;
        _ramVectors = 0;
        _mmuState = 0;
        _romMap = 0;
        _mapType = 0;
        _mmuPrefix = 0;

        //ushort[,] MmuRegisters = new ushort[4, 8];

        for (ushort index1 = 0; index1 < 8; index1++)
        {
            for (ushort index2 = 0; index2 < 4; index2++)
            {
                _mmuRegisters[index2, index1] = (ushort)(index1 + _stateSwitch[_currentRamConfig]);
            }
        }

        //for (int index = 0; index < 32; index++)
        //{
        //    instance->MmuRegisters[index] = MmuRegisters[index >> 3, index & 7];
        //}

        for (int index = 0; index < 1024; index++)
        {
            _memPages[index] = _ram.GetBytePointer((index & _ramMask[_currentRamConfig]) * 0x2000);
            _memPageOffsets[index] = 1;
        }

        SetRomMap(0);
        SetMapType(0);
    }

    /*****************************************************************************************
    * MmuInit Initialize and allocate memory for RAM Internal and External ROM Images.        *
    * Copy Rom Images to buffer space and reset GIME MMU registers to 0                      *
    * Returns NULL if any of the above fail.                                                 *
    *****************************************************************************************/
    public bool MmuInit(byte ramSizeOption)
    {
        uint ramSize = _memConfig[ramSizeOption];

        _currentRamConfig = ramSizeOption;

        _ram.Reset(ramSize);

        //--Well, this explains the vertical bands when you start a graphics mode in BASIC w/out PCLS
        for (int index = 0; index < ramSize; index++)
        {
            _ram[index] = (byte)((index & 1) == 0 ? 0 : 0xFF);
        }

        Graphics.SetVidMask(_vidMask[_currentRamConfig]);

        _irb.Reset(0x8001); //--TODO: Weird that the extra byte is needed here

        for (int index = 0; index <= 0x8000; index++)
        {
            _irb[index] = 0xFF;
        }

        CopyRom();
        MmuReset();

        return true;
    }

    public bool LoadInternalRom(string filename)
    {
        Debug.WriteLine($"LoadInternalRom: {filename}");

        if (!File.Exists(filename)) return false;

        byte[] bytes = File.ReadAllBytes(filename);

        for (ushort index = 0; index < bytes.Length; index++)
        {
            _irb[index] = bytes[index];
        }

        return true; //(ushort)bytes.Length;
    }

    public void GimeAssertVerticalInterrupt()
    {
        if (((_gimeRegisters[0x93] & 8) != 0) && (_enhancedFIRQFlag == 1))
        {
            _modules.CPU.AssertInterrupt(CPUInterrupts.FIRQ, 0); //FIRQ

            _lastFirq |= 8;
        }
        else if (((_gimeRegisters[0x92] & 8) != 0) && (_enhancedIRQFlag == 1))
        {
            _modules.CPU.AssertInterrupt(CPUInterrupts.IRQ, 0); //IRQ moon patrol demo using this

            _lastIrq |= 8;
        }
    }

    public byte MemRead8(ushort address)
    {
        if (address < 0xFE00)
        {
            ushort index = (ushort)(address >> 13);
            ushort mask = (ushort)(address & 0x1FFF);

            ushort mmu = _mmuRegisters[_mmuState, index];

            if (_memPageOffsets[mmu] == 1)
            {
                return _memPages[mmu][mask];
            }

            return _modules.PAKInterface.PakMem8Read((ushort)(_memPageOffsets[mmu] + mask));
        }

        if (address > 0xFEFF)
        {
            return _modules.IOBus.PortRead(address);
        }

        return VectorMemRead8(address);
    }

    public byte VectorMemRead8(ushort address)
    {
        if (_ramVectors != 0)
        {
            //Address must be $FE00 - $FEFF
            return (_ram[(0x2000 * _vectorMask[_currentRamConfig]) | (address & 0x1FFF)]);
        }

        return MemRead8(address);
    }

    public void MemWrite8(byte data, ushort address)
    {
        if (address < 0xFE00)
        {
            ushort index = (ushort)(address >> 13);
            ushort mask = (ushort)(address & 0x1FFF);

            ushort mmu = _mmuRegisters[_mmuState, index];

            byte maskA = _vectorMaskA[_currentRamConfig];
            byte maskB = _vectorMask[_currentRamConfig];

            if ((_mapType != 0) || (mmu < maskA) || (mmu > maskB))
            {
                _memPages[mmu][mask] = data;
            }

            return;
        }

        if (address > 0xFEFF)
        {
            _modules.IOBus.PortWrite(data, address);

            return;
        }

        VectorMemWrite8(data, address);
    }

    private void VectorMemWrite8(byte data, ushort address)
    {
        if (_ramVectors != 0)
        {
            //Address must be $FE00 - $FEFF
            _ram[(0x2000 * _vectorMask[_currentRamConfig]) | (address & 0x1FFF)] = data;
        }
        else
        {
            MemWrite8(data, address);
        }
    }

    //--I think this is just a hack to access memory directly for the 40/80 char-wide screen-scrapes
    public ushort GetMem(int address)
    {
        return _ram[address];
    }

    public byte SAMRead(byte port)
    {
        if (port >= 0xF0) // && port <= 0xFF)
        {
            //IRQ vectors from rom
            return _rom[0x3F00 + port];
        }

        return (0);
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

            _disOffset = (byte)(_disOffset & (0xFF - mask)); //Shut the bit off

            if ((port & 1) != 0)
            {
                _disOffset |= mask;
            }

            Graphics.SetGimeVdgOffset(_disOffset);
        }

        if (port is >= 0xC0 and <= 0xC5)   //VDG Mode
        {
            port -= 0xC0;
            reg = (byte)((port & 0x0E) >> 1);
            mask = (byte)(1 << reg);
            _vdgMode = (byte)(_vdgMode & (0xFF - mask));

            if ((port & 1) != 0)
            {
                _vdgMode |= mask;
            }

            Graphics.SetGimeVdgMode(_vdgMode);
        }

        if ((port == 0xDE) || (port == 0xDF))
        {
            SetMapType((byte)(port & 1));
        }

        if ((port == 0xD7) || (port == 0xD9))
        {
            _modules.Emu.SetCpuMultiplierFlag(1);
        }

        if ((port == 0xD6) || (port == 0xD8))
        {
            _modules.Emu.SetCpuMultiplierFlag(0);
        }
    }

    public byte GimeRead(byte port)
    {
        byte temp;

        switch (port)
        {
            case 0x92:
                temp = _lastIrq;
                _lastIrq = 0;

                return temp;

            case 0x93:
                temp = _lastFirq;
                _lastFirq = 0;

                return temp;

            case 0x94:
            case 0x95:
                return 126;

            default:
                return _gimeRegisters[port];
        }
    }

    public void GimeWrite(byte port, byte data)
    {
        _gimeRegisters[port] = data;

        switch (port)
        {
            case 0x90:
                SetInit0(data);
                break;

            case 0x91:
                SetInit1(data);
                break;

            case 0x92:
                SetGimeIRQSteering();
                break;

            case 0x93:
                SetGimeFIRQSteering();
                break;

            case 0x94:
                SetTimerMsb();
                break;

            case 0x95:
                SetTimerLsb();
                break;

            case 0x96:
                _modules.Emu.SetTurboMode((byte)(data & 1));
                break;

            case 0x97:
                break;

            case 0x98:
                Graphics.SetGimeVmode(data);
                break;

            case 0x99:
                Graphics.SetGimeVres(data);
                break;

            case 0x9A:
                Graphics.SetGimeBorderColor(data);
                break;

            case 0x9B:
                SetVideoOffsetRamBank(data);
                break;

            case 0x9C:
                break;

            case 0x9D:
            case 0x9E:
                Graphics.SetVerticalOffsetRegister((ushort)((_gimeRegisters[0x9D] << 8) | _gimeRegisters[0x9E]));
                break;

            case 0x9F:
                Graphics.SetGimeHorizontalOffset(data);
                break;

            case 0xA0:
            case 0xA1:
            case 0xA2:
            case 0xA3:
            case 0xA4:
            case 0xA5:
            case 0xA6:
            case 0xA7:
            case 0xA8:
            case 0xA9:
            case 0xAA:
            case 0xAB:
            case 0xAC:
            case 0xAD:
            case 0xAE:
            case 0xAF:
                SetMmuRegister(port, data);
                break;

            case 0xB0:
            case 0xB1:
            case 0xB2:
            case 0xB3:
            case 0xB4:
            case 0xB5:
            case 0xB6:
            case 0xB7:
            case 0xB8:
            case 0xB9:
            case 0xBA:
            case 0xBB:
            case 0xBC:
            case 0xBD:
            case 0xBE:
            case 0xBF:
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

        _enhancedFIRQFlag = (byte)((data & 16) >> 4);
        _enhancedIRQFlag = (byte)((data & 32) >> 5);
    }

    private void SetInit1(byte data)
    {
        SetMmuTask((byte)(data & 1));                       //TR
        _modules.CoCo.SetTimerClockRate((byte)(data & 32));	//TINS
    }

    private void SetTimerMsb()
    {
        ushort temp = (ushort)(((_gimeRegisters[0x94] << 8) + _gimeRegisters[0x95]) & 4095);

        _modules.CoCo.SetInterruptTimer(temp);
    }

    private void SetTimerLsb()
    {
        ushort temp = (ushort)(((_gimeRegisters[0x94] << 8) + _gimeRegisters[0x95]) & 4095);

        _modules.CoCo.SetInterruptTimer(temp);
    }

    private void SetGimeIRQSteering()
    {
        bool TestMask(int address, int mask) => (_gimeRegisters[address] & mask) != 0;
        byte Test(int mask) => TestMask(0x92, mask) | TestMask(0x93, mask) ? (byte)1 : (byte)0;

        _modules.Keyboard.GimeSetKeyboardInterruptState(Test(2));
        _modules.CoCo.SetVerticalInterruptState(Test(8));
        _modules.CoCo.SetHorizontalInterruptState(Test(16));
        _modules.CoCo.SetTimerInterruptState(Test(32));
    }

    //--TODO: Not sure why this is the same as IRQ above
    private void SetGimeFIRQSteering()
    {
        bool TestMask(int address, int mask) => (_gimeRegisters[address] & mask) != 0;
        byte Test(int mask) => TestMask(0x92, mask) | TestMask(0x93, mask) ? (byte)1 : (byte)0;

        _modules.Keyboard.GimeSetKeyboardInterruptState(Test(2));
        _modules.CoCo.SetVerticalInterruptState(Test(8));
        _modules.CoCo.SetHorizontalInterruptState(Test(16));
        _modules.CoCo.SetTimerInterruptState(Test(32));
    }

    public void GimeAssertHorizontalInterrupt()
    {
        if (((_gimeRegisters[0x93] & 16) != 0) && (_enhancedFIRQFlag == 1))
        {
            _modules.CPU.AssertInterrupt(CPUInterrupts.FIRQ, 0);

            _lastFirq |= 16;
        }
        else if (((_gimeRegisters[0x92] & 16) != 0) && (_enhancedIRQFlag == 1))
        {
            _modules.CPU.AssertInterrupt(CPUInterrupts.IRQ, 0);

            _lastIrq |= 16;
        }
    }

    public void GimeAssertTimerInterrupt()
    {
        if (((_gimeRegisters[0x93] & 32) != 0) && (_enhancedFIRQFlag == 1))
        {
            _modules.CPU.AssertInterrupt(CPUInterrupts.FIRQ, 0);

            _lastFirq |= 32;
        }
        else if (((_gimeRegisters[0x92] & 32) != 0) && (_enhancedIRQFlag == 1))
        {
            _modules.CPU.AssertInterrupt(CPUInterrupts.IRQ, 0);

            _lastIrq |= 32;
        }
    }

    public void SetMapType(byte type)
    {
        _mapType = type;

        UpdateMmuArray();
    }

    private void SetRomMap(byte data)
    {
        _romMap = (byte)(data & 3);

        UpdateMmuArray();
    }

    public void UpdateMmuArray()
    {
        if (_mapType != 0)
        {
            _memPages[_vectorMask[_currentRamConfig] - 3] = _ram.GetBytePointer(0x2000 * (_vectorMask[_currentRamConfig] - 3));
            _memPages[_vectorMask[_currentRamConfig] - 2] = _ram.GetBytePointer(0x2000 * (_vectorMask[_currentRamConfig] - 2));
            _memPages[_vectorMask[_currentRamConfig] - 1] = _ram.GetBytePointer(0x2000 * (_vectorMask[_currentRamConfig] - 1));
            _memPages[_vectorMask[_currentRamConfig]] = _ram.GetBytePointer(0x2000 * _vectorMask[_currentRamConfig]);

            _memPageOffsets[_vectorMask[_currentRamConfig] - 3] = 1;
            _memPageOffsets[_vectorMask[_currentRamConfig] - 2] = 1;
            _memPageOffsets[_vectorMask[_currentRamConfig] - 1] = 1;
            _memPageOffsets[_vectorMask[_currentRamConfig]] = 1;

            return;
        }

        switch (_romMap)
        {
            case 0:
            case 1: //16K Internal 16K External
                _memPages[_vectorMask[_currentRamConfig] - 3] = _irb.GetBytePointer(0x0000);
                _memPages[_vectorMask[_currentRamConfig] - 2] = _irb.GetBytePointer(0x2000);
                _memPages[_vectorMask[_currentRamConfig] - 1] = new BytePointer();
                _memPages[_vectorMask[_currentRamConfig]] = new BytePointer();

                _memPageOffsets[_vectorMask[_currentRamConfig] - 3] = 1;
                _memPageOffsets[_vectorMask[_currentRamConfig] - 2] = 1;
                _memPageOffsets[_vectorMask[_currentRamConfig] - 1] = 0;
                _memPageOffsets[_vectorMask[_currentRamConfig]] = 0x2000;

                return;

            case 2: // 32K Internal
                _memPages[_vectorMask[_currentRamConfig] - 3] = _irb.GetBytePointer(0x0000);
                _memPages[_vectorMask[_currentRamConfig] - 2] = _irb.GetBytePointer(0x2000);
                _memPages[_vectorMask[_currentRamConfig] - 1] = _irb.GetBytePointer(0x4000);
                _memPages[_vectorMask[_currentRamConfig]] = _irb.GetBytePointer(0x6000);

                _memPageOffsets[_vectorMask[_currentRamConfig] - 3] = 1;
                _memPageOffsets[_vectorMask[_currentRamConfig] - 2] = 1;
                _memPageOffsets[_vectorMask[_currentRamConfig] - 1] = 1;
                _memPageOffsets[_vectorMask[_currentRamConfig]] = 1;

                return;

            case 3: //32K External
                _memPages[_vectorMask[_currentRamConfig] - 1] = new BytePointer();
                _memPages[_vectorMask[_currentRamConfig]] = new BytePointer();
                _memPages[_vectorMask[_currentRamConfig] - 3] = new BytePointer();
                _memPages[_vectorMask[_currentRamConfig] - 2] = new BytePointer();

                _memPageOffsets[_vectorMask[_currentRamConfig] - 1] = 0;
                _memPageOffsets[_vectorMask[_currentRamConfig]] = 0x2000;
                _memPageOffsets[_vectorMask[_currentRamConfig] - 3] = 0x4000;
                _memPageOffsets[_vectorMask[_currentRamConfig] - 2] = 0x6000;

                return;
        }
    }

    private void SetMmuTask(byte task)
    {
        _mmuTask = task;
        _mmuState = (byte)((_mmuEnabled ? 1 : 0) << 1 | _mmuTask);
    }

    private void SetMmuRegister(byte register, byte data)
    {
        byte bankRegister = (byte)(register & 7);
        byte task = (byte)((register & 8) == 0 ? 0 : 1);

        //gime.c returns what was written so I can get away with this
        _mmuRegisters[task, bankRegister] = (ushort)(_mmuPrefix | (data & _ramMask[_currentRamConfig]));
    }

    private void SetVideoOffsetRamBank(byte data)
    {
        switch (_currentRamConfig)
        {
            case 0: // 128K
                return;

            case 1: //512K
                return;

            case 2: //2048K
                Graphics.SetVideoBank((byte)(data & 3));
                SetMmuPrefix(0);

                return;

            case 3: //8192K	//No Can 3 
                Graphics.SetVideoBank((byte)(data & 0x0F));
                SetMmuPrefix((byte)((data & 0x30) >> 4));

                return;
        }
    }

    private void SetMmuPrefix(byte data)
    {
        _mmuPrefix = (ushort)((data & 3) << 8);
    }

    private void SetVectors(byte data)
    {
        _ramVectors = (byte)(data == 0 ? 0 : 1); //Bit 3 of $FF90 MC3
    }

    private void SetMmuEnabled(bool flag)
    {
        _mmuEnabled = flag;
        _mmuState = (byte)((_mmuEnabled ? 1 : 0) << 1 | _mmuTask);
    }

    public void GimeAssertKeyboardInterrupt()
    {
        if (((_gimeRegisters[0x93] & 2) != 0) && (_enhancedFIRQFlag == 1))
        {
            _modules.CPU.AssertInterrupt(CPUInterrupts.FIRQ, 0);

            _lastFirq |= 2;
        }
        else if (((_gimeRegisters[0x92] & 2) != 0) && (_enhancedIRQFlag == 1))
        {
            _modules.CPU.AssertInterrupt(CPUInterrupts.IRQ, 0);

            _lastIrq |= 2;
        }
    }
}