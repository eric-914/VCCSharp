using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //CLRB
    //Clear accumulator or memory location
    //INHERENT
    public class _5F_Clrb_I : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            cpu.B_REG = 0;

            cpu.CC_C = false;
            cpu.CC_N = false;
            cpu.CC_V = false;
            cpu.CC_Z = true;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 2);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._21);
    }
}
