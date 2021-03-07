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
    }

    public class Emu : IEmu
    {
        private readonly ITC1014 _tc1014;
        private readonly IMC6821 _mc6821;
        private readonly IPAKInterface _pakInterface;
        private readonly ICPU _cpu;

        public Emu(IModules modules)
        {
            _tc1014 = modules.TC1014;
            _mc6821 = modules.MC6821;
            _pakInterface = modules.PAKInterface;
            _cpu = modules.CPU;
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
            Library.Emu.HardReset(emuState);
        }

        public void GimeReset()
        {
            Library.Emu.GimeReset();
        }
    }
}
