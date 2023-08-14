using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //TSTA
    //Test accumulator or memory location 
    //INHERENT
    public class _4D_Tsta_I : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            cpu.CC_Z = ZTEST(cpu.A_REG);
            cpu.CC_N = NTEST8(cpu.A_REG);
            cpu.CC_V = false;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 2);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._21);
    }
}
