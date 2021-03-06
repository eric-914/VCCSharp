using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface ICoCo
    {
        void SetClockSpeed(ushort cycles);
        unsafe float RenderFrame(EmuState* emuState);
    }

    public class CoCo : ICoCo
    {
        public void SetClockSpeed(ushort cycles)
        {
            Library.CoCo.SetClockSpeed(cycles);
        }

        public unsafe float RenderFrame(EmuState* emuState)
        {
            return Library.CoCo.RenderFrame(emuState);
        }
    }
}
