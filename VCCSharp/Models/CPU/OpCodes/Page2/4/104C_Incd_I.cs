using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    //INHERENT
    public class _104C_Incd_I : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            cpu.D_REG++;
            cpu.CC_Z = ZTEST(cpu.D_REG);
            cpu.CC_V = cpu.D_REG == 0x8000;
            cpu.CC_N = NTEST16(cpu.D_REG);

            return Cycles._32;
        }
    }
}
