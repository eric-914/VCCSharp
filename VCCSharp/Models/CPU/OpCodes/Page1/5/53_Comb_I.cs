using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //COMB
    //Complement accumulator or memory location
    //INHERENT
    public class _53_Comb_I : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            cpu.B_REG = (byte)(0xFF - cpu.B_REG);

            cpu.CC_Z = ZTEST(cpu.B_REG);
            cpu.CC_N = NTEST8(cpu.B_REG);
            cpu.CC_C = true;
            cpu.CC_V = false;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 2);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._21);
    }
}
