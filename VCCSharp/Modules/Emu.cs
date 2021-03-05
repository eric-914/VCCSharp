using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public class Emu
    {
        public unsafe EmuState* GetEmuState()
        {
            return Library.Emu.GetEmuState();
        }

        public unsafe void SetEmuState(EmuState* emuState)
        {
            Library.Emu.SetEmuState(emuState);
        }
    }
}
