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
        byte SetCPUMultiplier(byte multiplier);
        void SetEmuRunning(bool flag);
    }

    public class Emu : IEmu
    {
        private readonly IModules _modules;

        public Emu(IModules modules)
        {
            _modules = modules;
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
            _modules.TC1014.MC6883Reset();
            _modules.MC6821.MC6821_PiaReset();

            _modules.CPU.CPUReset();

            GimeReset();
            _modules.TC1014.MmuReset();
            _modules.TC1014.CopyRom();
            _modules.PAKInterface.ResetBus();

            unsafe
            {
                EmuState* emuState = GetEmuState();

                emuState->TurboSpeedFlag = 1;
            }
        }

        public unsafe void HardReset(EmuState* emuState)
        {
            if (_modules.TC1014.MmuInit(emuState->RamSize) == Define.FALSE)
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

            _modules.TC1014.MC6883Reset();  //Captures internal rom pointer for CPU Interrupt Vectors
            _modules.MC6821.MC6821_PiaReset();

            _modules.CPU.CPUInit();
            _modules.CPU.CPUReset();    // Zero all CPU Registers and sets the PC to VRESET

            GimeReset();

            _modules.PAKInterface.UpdateBusPointer();

            emuState->TurboSpeedFlag = 1;

            _modules.PAKInterface.ResetBus();

            _modules.CoCo.SetClockSpeed(1);
        }

        private void GimeReset()
        {
            _modules.Graphics.ResetGraphicsState();
            _modules.Graphics.MakeRGBPalette();

            int paletteType = _modules.Config.GetPaletteType();

            _modules.Graphics.MakeCMPPalette(paletteType);

            _modules.CoCo.CocoReset();

            _modules.Audio.ResetAudio();
        }
        
        public void SetEmuRunning(bool flag)
        {
            unsafe
            {
                GetEmuState()->EmulationRunning = flag ? Define.TRUE : Define.FALSE;
            }
        }

        private void SetCPUToHD6309()
        {
            Library.Emu.SetCPUToHD6309();
        }

        private void SetCPUToMC6809()
        {
            Library.Emu.SetCPUToMC6809();
        }

        public byte SetCPUMultiplier(byte multiplier)
        {
            return Library.Emu.SetCPUMultiplier(multiplier);
        }
    }
}
