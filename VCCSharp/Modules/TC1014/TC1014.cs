using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Models;

namespace VCCSharp.Modules.TC1014
{
    // ReSharper disable InconsistentNaming
    public interface ITC1014
    {
        void MC6883Reset();
        void CopyRom();
        void MmuReset();
        byte MmuInit(byte ramSizeOption);
        byte MemRead8(ushort address);
        void MemWrite8(byte data, ushort address);
        void GimeAssertVerticalInterrupt();
        void GimeAssertHorizontalInterrupt();
        void GimeAssertTimerInterrupt();
        ushort GetMem(int address);
        void SetMapType(byte type);

        void DrawBottomBorder32();
        void DrawTopBorder32();
        void UpdateScreen32();

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

        private byte _vdgMode;
        private byte _disOffset;

        private byte _mmuTask;	    // $FF91 bit 0
        private byte _mmuEnabled;	// $FF90 bit 6
        private byte _ramVectors;	// $FF90 bit 3

        private byte _romMap;		// $FF90 bit 1-0

        private ushort _mmuPrefix;

        private readonly uint[] _memConfig = { 0x20000, 0x80000, 0x200000, 0x800000 };
        private readonly ushort[] _ramMask = { 15, 63, 255, 1023 };
        private readonly byte[] _stateSwitch = { 8, 56, 56, 56 };
        private readonly uint[] _vidMask = { 0x1FFFF, 0x7FFFF, 0x1FFFFF, 0x7FFFFF };

        public unsafe byte* Rom;
        public unsafe byte* Memory;	//Emulated RAM
        public unsafe byte* InternalRomBuffer;

        public byte EnhancedFIRQFlag;
        public byte EnhancedIRQFlag;
        public byte LastIrq;
        public byte LastFirq;

        public byte[] GimeRegisters = new byte[256];

        public byte MmuState;	// Composite variable handles MmuTask and MmuEnabled
        public byte MapType;	// $FFDE/FFDF toggle Map type 0 = ram/rom

        public byte CurrentRamConfig = 1;

        public byte[] VectorMask = { 15, 63, 63, 63 };
        public byte[] VectorMaskA = { 12, 60, 60, 60 };

        public ushort[] MemPageOffsets = new ushort[1024];

        //--TODO: This is really ushort MmuRegisters[4][8]
        public ushort[,] MmuRegisters = new ushort[4, 8];	//[4][8] // $FFA0 - FFAF
        //unsigned short MmuRegisters[4][8];

        //--TODO: This is really byte* MemPages[1024]
        //public unsafe byte** MemPages; //[1024];
        //public unsafe fixed long MemPages[1024];
        public unsafe byte*[] MemPages = new byte*[1024];

        public TC1014(IModules modules)
        {
            _modules = modules;
        }

        public void MC6883Reset()
        {
            _vdgMode = 0;
            _disOffset = 0;

            unsafe
            {
                Rom = GetInternalRomPointer();
            }
        }

        //TODO: Used by MmuInit()
        public void CopyRom()
        {
            const string rom = "coco3.rom";

            ConfigModel configModel = _modules.Config.ConfigModel;

            //--Try loading from Vcc.ini >> CoCoRomPath
            string cocoRomPath = configModel.CoCoRomPath;

            string path = Path.Combine(configModel.CoCoRomPath, rom);

            if (LoadInternalRom(path))
            {
                Debug.WriteLine($"Found {rom} in CoCoRomPath");
                return;
            }

            //--Try loading from Vcc.ini >> ExternalBasicImage
            string externalBasicImage = _modules.Config.ConfigModel.ExternalBasicImage;

            if (!string.IsNullOrEmpty(externalBasicImage) && LoadInternalRom(externalBasicImage))
            {
                Debug.WriteLine($"Found {rom} in ExternalBasicImage");
                return;
            }

            //--Try loading from current executable folder
            string exePath = Path.GetDirectoryName(_modules.Vcc.GetExecPath());
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
            unsafe
            {
                _mmuTask = 0;
                _mmuEnabled = 0;
                _ramVectors = 0;
                MmuState = 0;
                _romMap = 0;
                MapType = 0;
                _mmuPrefix = 0;

                //ushort[,] MmuRegisters = new ushort[4, 8];

                for (ushort index1 = 0; index1 < 8; index1++)
                {
                    for (ushort index2 = 0; index2 < 4; index2++)
                    {
                        MmuRegisters[index2, index1] = (ushort)(index1 + _stateSwitch[CurrentRamConfig]);
                    }
                }

                //for (int index = 0; index < 32; index++)
                //{
                //    instance->MmuRegisters[index] = MmuRegisters[index >> 3, index & 7];
                //}

                for (int index = 0; index < 1024; index++)
                {
                    byte* offset = Memory + (index & _ramMask[CurrentRamConfig]) * 0x2000;
                    MemPages[index] = offset;
                    MemPageOffsets[index] = 1;
                }

                SetRomMap(0);
                SetMapType(0);
            }
        }

        /*****************************************************************************************
        * MmuInit Initialize and allocate memory for RAM Internal and External ROM Images.        *
        * Copy Rom Images to buffer space and reset GIME MMU registers to 0                      *
        * Returns NULL if any of the above fail.                                                 *
        *****************************************************************************************/
        public byte MmuInit(byte ramSizeOption)
        {
            unsafe
            {
                uint ramSize = _memConfig[ramSizeOption];

                CurrentRamConfig = ramSizeOption;

                FreeMemory(Memory);

                Memory = AllocateMemory(ramSize);

                if (Memory == null)
                {
                    return 0;
                }

                //--Well, this explains the vertical bands when you start a graphics mode in BASIC w/out PCLS
                for (uint index = 0; index < ramSize; index++)
                {
                    Memory[index] = (byte)((index & 1) == 0 ? 0 : 0xFF);
                }

                Graphics.SetVidMask(_vidMask[CurrentRamConfig]);

                FreeMemory(InternalRomBuffer);
                InternalRomBuffer = AllocateMemory(0x8001); //--TODO: Weird that the extra byte is needed here

                if (InternalRomBuffer == null)
                {
                    return 0;
                }

                //memset(mmuState->InternalRomBuffer, 0xFF, 0x8000);
                for (uint index = 0; index <= 0x8000; index++)
                {
                    InternalRomBuffer[index] = 0xFF;
                }

                CopyRom();
                MmuReset();

                return 1;
            }
        }

        public bool LoadInternalRom(string filename)
        {
            Debug.WriteLine($"LoadInternalRom: {filename}");

            if (!File.Exists(filename)) return false;

            byte[] bytes = File.ReadAllBytes(filename);

            unsafe
            {
                for (ushort index = 0; index < bytes.Length; index++)
                {
                    InternalRomBuffer[index] = bytes[index];
                }
            }

            return true; //(ushort)bytes.Length;
        }

        public void GimeAssertVerticalInterrupt()
        {
            if (((GimeRegisters[0x93] & 8) != 0) && (EnhancedFIRQFlag == 1))
            {
                _modules.CPU.AssertInterrupt(CPUInterrupts.FIRQ, 0); //FIRQ

                LastFirq |= 8;
            }
            else if (((GimeRegisters[0x92] & 8) != 0) && (EnhancedIRQFlag == 1))
            {
                _modules.CPU.AssertInterrupt(CPUInterrupts.IRQ, 0); //IRQ moon patrol demo using this

                LastIrq |= 8;
            }
        }

        public unsafe void FreeMemory(byte* target)
        {
            if (target != null)
            {
                Marshal.FreeHGlobal((IntPtr)target);
            }
        }

        public unsafe byte* AllocateMemory(uint size)
        {
            return (byte*)Marshal.AllocHGlobal((int)size); //malloc(size);
        }

        public byte MemRead8(ushort address)
        {
            if (address < 0xFE00)
            {
                ushort index = (ushort)(address >> 13);
                ushort mask = (ushort)(address & 0x1FFF);

                ushort mmu = MmuRegisters[MmuState, index];

                if (MemPageOffsets[mmu] == 1)
                {
                    unsafe
                    {
                        return MemPages[mmu][mask];
                    }
                }
                else
                {
                    return _modules.PAKInterface.PakMem8Read((ushort)(MemPageOffsets[mmu] + mask));
                }
            }

            if (address > 0xFEFF)
            {
                return _modules.IOBus.port_read(address);
            }

            return VectorMemRead8(address);
        }

        public byte VectorMemRead8(ushort address)
        {
            if (_ramVectors != 0)
            {
                unsafe
                {
                    //Address must be $FE00 - $FEFF
                    return (Memory[(0x2000 * VectorMask[CurrentRamConfig]) | (address & 0x1FFF)]);
                }
            }

            return MemRead8(address);
        }

        public void MemWrite8(byte data, ushort address)
        {
            if (address < 0xFE00)
            {
                ushort index = (ushort)(address >> 13);
                ushort mask = (ushort)(address & 0x1FFF);

                ushort mmu = MmuRegisters[MmuState, index];

                byte maskA = VectorMaskA[CurrentRamConfig];
                byte maskB = VectorMask[CurrentRamConfig];

                if ((MapType != 0) || (mmu < maskA) || (mmu > maskB))
                {
                    unsafe
                    {
                        MemPages[mmu][mask] = data;
                    }
                }

                return;
            }

            if (address > 0xFEFF)
            {
                _modules.IOBus.port_write(data, address);

                return;
            }

            VectorMemWrite8(data, address);
        }

        public void VectorMemWrite8(byte data, ushort address)
        {
            if (_ramVectors != 0)
            {
                unsafe
                {
                    //Address must be $FE00 - $FEFF
                    Memory[(0x2000 * VectorMask[CurrentRamConfig]) | (address & 0x1FFF)] = data;
                }
            }
            else
            {
                MemWrite8(data, address);
            }
        }

        //--I think this is just a hack to access memory directly for the 40/80 char-wide screen-scrapes
        public ushort GetMem(int address)
        {
            unsafe
            {
                return Memory[address];
            }
        }

        public void DrawTopBorder32()
        {
            GraphicsSurfaces graphicsSurfaces = Graphics.GetGraphicsSurfaces();

            if (Graphics.BorderChange == 0)
            {
                return;
            }

            unsafe
            {
                for (ushort x = 0; x < _modules.Emu.WindowSize.X; x++)
                {
                    graphicsSurfaces.pSurface32[x + ((_modules.Emu.LineCounter * 2) * _modules.Emu.SurfacePitch)] =
                        Graphics.BorderColor32;

                    if (!_modules.Emu.ScanLines
                    )
                    {
                        graphicsSurfaces.pSurface32[
                                x + ((_modules.Emu.LineCounter * 2 + 1) * _modules.Emu.SurfacePitch)] =
                            Graphics.BorderColor32;
                    }
                }
            }
        }

        public void DrawBottomBorder32()
        {
            GraphicsSurfaces graphicsSurfaces = Graphics.GetGraphicsSurfaces();

            if (Graphics.BorderChange == 0)
            {
                return;
            }

            unsafe
            {
                for (ushort x = 0; x < _modules.Emu.WindowSize.X; x++)
                {
                    graphicsSurfaces.pSurface32[
                        x + (2 * (_modules.Emu.LineCounter + Graphics.LinesPerScreen + Graphics.VerticalCenter) *
                             _modules.Emu.SurfacePitch)] = Graphics.BorderColor32;

                    if (!_modules.Emu.ScanLines)
                    {
                        graphicsSurfaces.pSurface32[
                            x + _modules.Emu.SurfacePitch +
                            (2 * (_modules.Emu.LineCounter + Graphics.LinesPerScreen + Graphics.VerticalCenter) *
                             _modules.Emu.SurfacePitch)] = Graphics.BorderColor32;
                    }
                }
            }
        }

        public void UpdateScreen32()
        {
            GraphicsSurfaces graphicsSurfaces = Graphics.GetGraphicsSurfaces();

            ushort y = (ushort)_modules.Emu.LineCounter;
            long xPitch = _modules.Emu.SurfacePitch;

            if ((Graphics.HorizontalCenter != 0) && (Graphics.BorderChange > 0))
            {
                unsafe
                {
                    uint* szSurface32 = graphicsSurfaces.pSurface32;

                    for (ushort x = 0; x < Graphics.HorizontalCenter; x++)
                    {
                        szSurface32[x + (((y + Graphics.VerticalCenter) * 2) * xPitch)] = Graphics.BorderColor32;

                        if (!_modules.Emu.ScanLines)
                        {
                            szSurface32[x + (((y + Graphics.VerticalCenter) * 2 + 1) * xPitch)] =
                                Graphics.BorderColor32;
                        }

                        szSurface32[
                            x + (Graphics.PixelsPerLine * (Graphics.Stretch + 1)) + Graphics.HorizontalCenter +
                            (((y + Graphics.VerticalCenter) * 2) * xPitch)] = Graphics.BorderColor32;

                        if (!_modules.Emu.ScanLines)
                        {
                            szSurface32[
                                x + (Graphics.PixelsPerLine * (Graphics.Stretch + 1)) + Graphics.HorizontalCenter +
                                (((y + Graphics.VerticalCenter) * 2 + 1) * xPitch)] = Graphics.BorderColor32;
                        }
                    }
                }
            }

            if (Graphics.LinesPerRow < 13)
            {
                Graphics.TagY++;
            }

            if (y == Define.FALSE)
            {
                Graphics.StartOfVidRam = Graphics.NewStartOfVidRam;
                Graphics.TagY = y;
            }

            uint start = (uint)(Graphics.StartOfVidRam + (Graphics.TagY / Graphics.LinesPerRow) * (Graphics.VPitch * Graphics.ExtendedText));
            uint yStride = (uint)((((y + Graphics.VerticalCenter) * 2) * xPitch) + (Graphics.HorizontalCenter * 1) - 1);

            SwitchMasterMode32(Graphics.MasterMode, start, yStride);
        }

        public byte SAMRead(byte port)
        {
            unsafe
            {
                if ((port >= 0xF0) && (port <= 0xFF))
                {
                    //IRQ vectors from rom
                    return (Rom[0x3F00 + port]);
                }

                return (0);
            }
        }

        public void SAMWrite(byte data, byte port)
        {
            byte mask;
            byte reg;

            if ((port >= 0xC6) && (port <= 0xD3))   //VDG Display offset Section
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

            if ((port >= 0xC0) && (port <= 0xC5))   //VDG Mode
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
                    temp = LastIrq;
                    LastIrq = 0;

                    return temp;

                case 0x93:
                    temp = LastFirq;
                    LastFirq = 0;

                    return temp;

                case 0x94:
                case 0x95:
                    return 126;

                default:
                    return GimeRegisters[port];
            }
        }

        public void GimeWrite(byte port, byte data)
        {
            GimeRegisters[port] = data;

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
                    Graphics.SetVerticalOffsetRegister((ushort)((GimeRegisters[0x9D] << 8) | GimeRegisters[0x9E]));
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

        public void SetInit0(byte data)
        {
            Graphics.SetCompatMode((byte)((data & 128) == 0 ? 0 : 1));
            SetMmuEnabled((byte)((data & 64) == 0 ? 0 : 1));
            SetRomMap((byte)(data & 3)); //MC0-MC1
            SetVectors((byte)(data & 8)); //MC3

            EnhancedFIRQFlag = (byte)((data & 16) >> 4);
            EnhancedIRQFlag = (byte)((data & 32) >> 5);
        }

        public void SetInit1(byte data)
        {
            SetMmuTask((byte)(data & 1));                       //TR
            _modules.CoCo.SetTimerClockRate((byte)(data & 32));	//TINS
        }

        public void SetTimerMsb()
        {
            ushort temp = (ushort)(((GimeRegisters[0x94] << 8) + GimeRegisters[0x95]) & 4095);

            _modules.CoCo.SetInterruptTimer(temp);
        }

        public void SetTimerLsb()
        {
            ushort temp = (ushort)(((GimeRegisters[0x94] << 8) + GimeRegisters[0x95]) & 4095);

            _modules.CoCo.SetInterruptTimer(temp);
        }

        public void SetGimeIRQSteering()
        {
            bool TestMask(int address, int mask) => (GimeRegisters[address] & mask) != 0;
            byte Test(int mask) => TestMask(0x92, mask) | TestMask(0x93, mask) ? (byte)1 : (byte)0;

            _modules.Keyboard.GimeSetKeyboardInterruptState(Test(2));
            _modules.CoCo.SetVerticalInterruptState(Test(8));
            _modules.CoCo.SetHorizontalInterruptState(Test(16));
            _modules.CoCo.SetTimerInterruptState(Test(32));
        }

        //--TODO: Not sure why this is the same as IRQ above
        public void SetGimeFIRQSteering()
        {
            bool TestMask(int address, int mask) => (GimeRegisters[address] & mask) != 0;
            byte Test(int mask) => TestMask(0x92, mask) | TestMask(0x93, mask) ? (byte)1 : (byte)0;

            _modules.Keyboard.GimeSetKeyboardInterruptState(Test(2));
            _modules.CoCo.SetVerticalInterruptState(Test(8));
            _modules.CoCo.SetHorizontalInterruptState(Test(16));
            _modules.CoCo.SetTimerInterruptState(Test(32));
        }

        public void GimeAssertHorizontalInterrupt()
        {
            if (((GimeRegisters[0x93] & 16) != 0) && (EnhancedFIRQFlag == 1))
            {
                _modules.CPU.AssertInterrupt(CPUInterrupts.FIRQ, 0);

                LastFirq |= 16;
            }
            else if (((GimeRegisters[0x92] & 16) != 0) && (EnhancedIRQFlag == 1))
            {
                _modules.CPU.AssertInterrupt(CPUInterrupts.IRQ, 0);

                LastIrq |= 16;
            }
        }

        public void GimeAssertTimerInterrupt()
        {
            if (((GimeRegisters[0x93] & 32) != 0) && (EnhancedFIRQFlag == 1))
            {
                _modules.CPU.AssertInterrupt(CPUInterrupts.FIRQ, 0);

                LastFirq |= 32;
            }
            else if (((GimeRegisters[0x92] & 32) != 0) && (EnhancedIRQFlag == 1))
            {
                _modules.CPU.AssertInterrupt(CPUInterrupts.IRQ, 0);

                LastIrq |= 32;
            }
        }

        public void SetMapType(byte type)
        {
            MapType = type;

            UpdateMmuArray();
        }

        public void SetRomMap(byte data)
        {
            _romMap = (byte)(data & 3);

            UpdateMmuArray();
        }

        public unsafe void UpdateMmuArray()
        {
            if (MapType != 0)
            {
                MemPages[VectorMask[CurrentRamConfig] - 3] = (Memory + (0x2000 * (VectorMask[CurrentRamConfig] - 3)));
                MemPages[VectorMask[CurrentRamConfig] - 2] = (Memory + (0x2000 * (VectorMask[CurrentRamConfig] - 2)));
                MemPages[VectorMask[CurrentRamConfig] - 1] = (Memory + (0x2000 * (VectorMask[CurrentRamConfig] - 1)));
                MemPages[VectorMask[CurrentRamConfig]] = (Memory + (0x2000 * VectorMask[CurrentRamConfig]));

                MemPageOffsets[VectorMask[CurrentRamConfig] - 3] = 1;
                MemPageOffsets[VectorMask[CurrentRamConfig] - 2] = 1;
                MemPageOffsets[VectorMask[CurrentRamConfig] - 1] = 1;
                MemPageOffsets[VectorMask[CurrentRamConfig]] = 1;

                return;
            }

            switch (_romMap)
            {
                case 0:
                case 1: //16K Internal 16K External
                    MemPages[VectorMask[CurrentRamConfig] - 3] = (InternalRomBuffer);
                    MemPages[VectorMask[CurrentRamConfig] - 2] = (InternalRomBuffer + 0x2000);
                    MemPages[VectorMask[CurrentRamConfig] - 1] = null;
                    MemPages[VectorMask[CurrentRamConfig]] = null;

                    MemPageOffsets[VectorMask[CurrentRamConfig] - 3] = 1;
                    MemPageOffsets[VectorMask[CurrentRamConfig] - 2] = 1;
                    MemPageOffsets[VectorMask[CurrentRamConfig] - 1] = 0;
                    MemPageOffsets[VectorMask[CurrentRamConfig]] = 0x2000;

                    return;

                case 2: // 32K Internal
                    MemPages[VectorMask[CurrentRamConfig] - 3] = InternalRomBuffer;
                    MemPages[VectorMask[CurrentRamConfig] - 2] = InternalRomBuffer + 0x2000;
                    MemPages[VectorMask[CurrentRamConfig] - 1] = InternalRomBuffer + 0x4000;
                    MemPages[VectorMask[CurrentRamConfig]] = InternalRomBuffer + 0x6000;

                    MemPageOffsets[VectorMask[CurrentRamConfig] - 3] = 1;
                    MemPageOffsets[VectorMask[CurrentRamConfig] - 2] = 1;
                    MemPageOffsets[VectorMask[CurrentRamConfig] - 1] = 1;
                    MemPageOffsets[VectorMask[CurrentRamConfig]] = 1;

                    return;

                case 3: //32K External
                    MemPages[VectorMask[CurrentRamConfig] - 1] = null;
                    MemPages[VectorMask[CurrentRamConfig]] = null;
                    MemPages[VectorMask[CurrentRamConfig] - 3] = null;
                    MemPages[VectorMask[CurrentRamConfig] - 2] = null;

                    MemPageOffsets[VectorMask[CurrentRamConfig] - 1] = 0;
                    MemPageOffsets[VectorMask[CurrentRamConfig]] = 0x2000;
                    MemPageOffsets[VectorMask[CurrentRamConfig] - 3] = 0x4000;
                    MemPageOffsets[VectorMask[CurrentRamConfig] - 2] = 0x6000;

                    return;
            }
        }

        public void SetMmuTask(byte task)
        {
            _mmuTask = task;
            MmuState = (byte)((_mmuEnabled == 0 ? 1 : 0) << 1 | _mmuTask);
        }

        public void SetMmuRegister(byte register, byte data)
        {
            byte bankRegister = (byte)(register & 7);
            byte task = (byte)((register & 8) == 0 ? 0 : 1);

            //gime.c returns what was written so I can get away with this
            MmuRegisters[task, bankRegister] = (ushort)(_mmuPrefix | (data & _ramMask[CurrentRamConfig]));
        }

        public void SetVideoOffsetRamBank(byte data)
        {
            switch (CurrentRamConfig)
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

        public void SetMmuPrefix(byte data)
        {
            _mmuPrefix = (ushort)((data & 3) << 8);
        }

        public unsafe byte* GetInternalRomPointer()
        {
            return InternalRomBuffer;
        }

        public void SetVectors(byte data)
        {
            _ramVectors = (byte)(data == 0 ? 0 : 1); //Bit 3 of $FF90 MC3
        }

        public void SetMmuEnabled(byte flag)
        {
            _mmuEnabled = flag;
            MmuState = (byte)((_mmuEnabled == 0 ? 1 : 0) << 1 | _mmuTask);
        }

        public void GimeAssertKeyboardInterrupt()
        {
            if (((GimeRegisters[0x93] & 2) != 0) && (EnhancedFIRQFlag == 1))
            {
                _modules.CPU.AssertInterrupt(CPUInterrupts.FIRQ, 0);

                LastFirq |= 2;
            }
            else if (((GimeRegisters[0x92] & 2) != 0) && (EnhancedIRQFlag == 1))
            {
                _modules.CPU.AssertInterrupt(CPUInterrupts.IRQ, 0);

                LastIrq |= 2;
            }
        }
    }
}
