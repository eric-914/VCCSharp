using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    public class CoCo
    {
        public void SetClockSpeed(ushort cycles)
        {
            Library.CoCo.SetClockSpeed(cycles);
        }
    }
}
