using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    public interface ICPU
    {
        void CPUReset();
        void CPUInit();
        void CPUForcePC(ushort xferAddress);
        int CPUExec(int cycle);
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

        public void CPUForcePC(ushort xferAddress)
        {
            Library.CPU.CPUForcePC(xferAddress);
        }

        public int CPUExec(int cycle)
        {
            return Library.CPU.CPUExec(cycle);
        }
    }
}
