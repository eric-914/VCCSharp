using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    public interface ICoCo
    {
        void SetClockSpeed(ushort cycles);
    }

    public class CoCo : ICoCo
    {
        public void SetClockSpeed(ushort cycles)
        {
            Library.CoCo.SetClockSpeed(cycles);
        }
    }
}
