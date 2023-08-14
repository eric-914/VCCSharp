using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3
{
    //INHERENT
    public class _115A_Decf_I : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            cpu.F_REG--;

            cpu.CC_Z = ZTEST(cpu.F_REG);
            cpu.CC_N = NTEST8(cpu.F_REG);
            cpu.CC_V = cpu.F_REG == 0x7F;

            return Cycles._21;
        }
    }
}
