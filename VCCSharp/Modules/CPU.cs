using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    public interface ICPU
    {
        void CPUReset();
    }

    public class CPU : ICPU
    {
        public void CPUReset()
        {
            Library.CPU.CPUReset();
        }
    }
}
