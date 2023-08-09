using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    public class _14_Sexw_I : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu)
        {
            throw new NotImplementedException();
        }

        public int Exec(IHD6309 cpu)
        {
            cpu.D_REG = (cpu.W_REG & 32768) != 0 ? (ushort)0xFFFF : (ushort)0;

            cpu.CC_Z = ZTEST(cpu.Q_REG);
            cpu.CC_N = NTEST16(cpu.D_REG);

            return 4;
        }
    }
}
