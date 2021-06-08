using System;
using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Models;
using HINSTANCE = System.IntPtr;
using HWND = System.IntPtr;

namespace VCCSharp.Modules
{
    public interface IEmu
    {
        HWND WindowHandle { get; set; }
        HINSTANCE Resources { get; set; }

        void SoftReset();
        void HardReset();
        void SetCpuMultiplier(byte multiplier);
        void SetCpuMultiplierFlag(byte doubleSpeed);
        void SetTurboMode(byte data);

        byte BitDepth { get; set; }
        byte CpuType { get; set; }
        byte FrameSkip { get; set; }
        byte FullScreen { get; set; }
        byte RamSize { get; set; }
        bool ScanLines { get; set; }
        short FrameCounter { get; set; }
        short LineCounter { get; set; }
        long SurfacePitch { get; set; }
        double CpuCurrentSpeed { get; set; }

        bool EmulationRunning { get; set; }
        byte ResetPending { get; set; }

        string StatusLine { get; set; }

        string PakPath { get; set; }

        Point WindowSize { get; set; }
    }

    public class Emu : IEmu
    {
        private readonly IModules _modules;

        public HWND WindowHandle { get; set; }
        public HINSTANCE Resources { get; set; }

        public byte BitDepth { get; set; }
        public byte CpuType { get; set; }

        public byte FrameSkip { get; set; }
        public byte FullScreen { get; set; }

        public byte RamSize { get; set; }
        public bool ScanLines { get; set; }

        public short FrameCounter { get; set; }
        public short LineCounter { get; set; }

        public long SurfacePitch { get; set; }

        public string StatusLine { get; set; }

        public byte DoubleSpeedFlag;
        public byte DoubleSpeedMultiplier = 2;

        public double CpuCurrentSpeed { get; set; } = .894;
        public byte TurboSpeedFlag = 1;

        public Point WindowSize { get; set; }

        public bool EmulationRunning { get; set; }
        public byte ResetPending { get; set; } = Define.RESET_NONE;

        public string PakPath { get; set; }

        public Emu(IModules modules)
        {
            _modules = modules;
        }

        public void SoftReset()
        {
            _modules.TC1014.MC6883Reset();
            _modules.MC6821.PiaReset();

            _modules.CPU.CPUReset();

            GimeReset();
            _modules.TC1014.MmuReset();
            _modules.TC1014.CopyRom();
            _modules.PAKInterface.ResetBus();

            TurboSpeedFlag = 1;
        }

        public void HardReset()
        {
            if (_modules.TC1014.MmuInit(RamSize) == Define.FALSE)
            {
                MessageBox.Show("Can't allocate enough RAM, out of memory", "Error");

                Environment.Exit(0);
            }

            if (CpuType == (byte)CPUTypes.HD6309)
            {
                _modules.CPU.SetCPUToHD6309();
            }
            else
            {
                _modules.CPU.SetCPUToMC6809();
            }

            _modules.TC1014.MC6883Reset();  //Captures internal rom pointer for CPU Interrupt Vectors
            _modules.MC6821.PiaReset();

            _modules.CPU.CPUInit();
            _modules.CPU.CPUReset();    // Zero all CPU Registers and sets the PC to VRESET

            GimeReset();

            _modules.PAKInterface.UpdateBusPointer();

            TurboSpeedFlag = 1;

            _modules.PAKInterface.ResetBus();

            _modules.CoCo.OverClock = 1;
        }

        private void GimeReset()
        {
            _modules.Graphics.ResetGraphicsState();
            _modules.Graphics.MakeRgbPalette();

            int paletteType = _modules.Config.GetPaletteType();

            _modules.Graphics.MakeCmpPalette(paletteType);

            _modules.CoCo.CocoReset();

            _modules.Audio.ResetAudio();
        }

        public void SetCpuMultiplier(byte multiplier)
        {
            DoubleSpeedMultiplier = multiplier;

            SetCpuMultiplierFlag(DoubleSpeedFlag);
        }

        public void SetCpuMultiplierFlag(byte doubleSpeed)
        {
            _modules.CoCo.OverClock = 1;

            DoubleSpeedFlag = doubleSpeed;

            if (DoubleSpeedFlag != 0)
            {
                _modules.CoCo.OverClock = DoubleSpeedMultiplier * TurboSpeedFlag;
            }

            CpuCurrentSpeed = .894;

            if (DoubleSpeedFlag != 0)
            {
                CpuCurrentSpeed *= (DoubleSpeedMultiplier * (double)TurboSpeedFlag);
            }
        }

        public void SetTurboMode(byte data)
        {
            _modules.CoCo.OverClock = 1;

            TurboSpeedFlag = (byte)((data & 1) + 1);

            if (DoubleSpeedFlag != 0)
            {
                _modules.CoCo.OverClock = DoubleSpeedMultiplier * TurboSpeedFlag;
            }

            CpuCurrentSpeed = .894;

            if (DoubleSpeedFlag != 0)
            {
                CpuCurrentSpeed *= (DoubleSpeedMultiplier * (double)TurboSpeedFlag);
            }
        }
    }
}
