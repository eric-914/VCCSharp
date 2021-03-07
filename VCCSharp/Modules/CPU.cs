using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    public interface ICPU
    {
        void CPUReset();
        void CPUInit();
    }

    public class CPU : ICPU
    {
        public void CPUReset()
        {
            Library.CPU.CPUReset();
        }

        public void CPUInit()
        {
            Library.CPU.CPUInit();
        }
    }
}
