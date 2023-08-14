using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    //INHERENT
    public class _105C_Incw_I : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            cpu.W_REG++;
            cpu.CC_Z = ZTEST(cpu.W_REG);
            cpu.CC_V = cpu.W_REG == 0x8000;
            cpu.CC_N = NTEST16(cpu.W_REG);

            return Cycles._32;
        }
    }
}
