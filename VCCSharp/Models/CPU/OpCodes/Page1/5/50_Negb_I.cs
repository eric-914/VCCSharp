using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    public class _50_Negb_I : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            byte negative = (byte)(0 - cpu.B_REG);

            cpu.CC_C = negative > 0;
            cpu.CC_V = cpu.B_REG == 0x80;
            cpu.CC_N = NTEST8(negative);
            cpu.CC_Z = ZTEST(negative);

            cpu.B_REG = negative;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 2);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._21);
    }
}
