using System;
using System.Windows;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IEmu
    {
        unsafe EmuState* GetEmuState();
        unsafe void SetEmuState(EmuState* emuState);
        void SoftReset();
        unsafe void HardReset(EmuState* emuState);
        void GimeReset();
        void SetCPUToHD6309();
        void SetCPUToMC6809();
    }

    public class Emu : IEmu
    {
        private readonly ITC1014 _tc1014;
        private readonly IMC6821 _mc6821;
        private readonly IPAKInterface _pakInterface;
        private readonly ICPU _cpu;
        private readonly ICoCo _coco;
        private readonly IGraphics _graphics;
        private readonly IConfig _config;
        private readonly IAudio _audio;

        public Emu(IModules modules)
        {
            _tc1014 = modules.TC1014;
            _mc6821 = modules.MC6821;
            _pakInterface = modules.PAKInterface;
            _cpu = modules.CPU;
            _coco = modules.CoCo;
            _graphics = modules.Graphics;
            _config = modules.Config;
            _audio = modules.Audio;
        }

        public unsafe EmuState* GetEmuState()
        {
            return Library.Emu.GetEmuState();
        }

        public unsafe void SetEmuState(EmuState* emuState)
        {
            Library.Emu.SetEmuState(emuState);
        }

        public void SoftReset()
        {
            _tc1014.MC6883Reset();
            _mc6821.MC6821_PiaReset();

            _cpu.CPUReset();

            GimeReset();
            _tc1014.MmuReset();
            _tc1014.CopyRom();
            _pakInterface.ResetBus();

            unsafe
            {
                EmuState* emuState = GetEmuState();

                emuState->TurboSpeedFlag = 1;
            }
        }

        public unsafe void HardReset(EmuState* emuState)
        {
            if (_tc1014.MmuInit(emuState->RamSize) == Define.FALSE)
            {
                MessageBox.Show("Can't allocate enough RAM, out of memory", "Error");

                Environment.Exit(0);
            }

            if (emuState->CpuType == 1)
            {
                SetCPUToHD6309();
            }
            else
            {
                SetCPUToMC6809();
            }

            _tc1014.MC6883Reset();  //Captures internal rom pointer for CPU Interrupt Vectors
            _mc6821.MC6821_PiaReset();

            _cpu.CPUInit();
            _cpu.CPUReset();    // Zero all CPU Registers and sets the PC to VRESET

            GimeReset();

            _pakInterface.UpdateBusPointer();

            emuState->TurboSpeedFlag = 1;

            _pakInterface.ResetBus();

            _coco.SetClockSpeed(1);
        }

        public void GimeReset()
        {
            _graphics.ResetGraphicsState();
            _graphics.MakeRGBPalette();

            int paletteType = _config.GetPaletteType();

            _graphics.MakeCMPPalette(paletteType);

            _coco.CocoReset();

            _audio.ResetAudio();
        }


        public void SetCPUToHD6309()
        {
            Library.Emu.SetCPUToHD6309();
        }

        public void SetCPUToMC6809()
        {
            Library.Emu.SetCPUToMC6809();
        }
    }
}
