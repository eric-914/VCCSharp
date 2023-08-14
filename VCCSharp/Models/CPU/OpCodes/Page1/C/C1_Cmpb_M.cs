using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1.C
{
    //CMPB
    //Compare memory from accumulator 
    //IMMEDIATE
    public class C1_Cmpb_M : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            byte value = cpu.MemRead8(cpu.PC_REG++);
            byte compare = (byte)(cpu.B_REG - value);

            cpu.CC_C = compare > cpu.B_REG;
            cpu.CC_V = OVERFLOW8(cpu.CC_C, value, compare, cpu.B_REG);
            cpu.CC_N = NTEST8(compare);
            cpu.CC_Z = ZTEST(compare);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 2);
        public int Exec(IHD6309 cpu) => Exec(cpu, 2);
    }
}
