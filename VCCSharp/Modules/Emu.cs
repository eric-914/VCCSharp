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
    }

    public class Emu : IEmu
    {
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
            Library.Emu.SoftReset();
        }

        public unsafe void HardReset(EmuState* emuState)
        {
            Library.Emu.HardReset(emuState);
        }
    }
}
