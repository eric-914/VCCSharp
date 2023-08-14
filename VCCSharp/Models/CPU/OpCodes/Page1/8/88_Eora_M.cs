using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //EORA
    //Exclusive or memory with accumulator
    //IMMEDIATE
    public class _88_Eora_M : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            cpu.A_REG ^= cpu.MemRead8(cpu.PC_REG++);

            cpu.CC_N = NTEST8(cpu.A_REG);
            cpu.CC_Z = ZTEST(cpu.A_REG);
            cpu.CC_V = false;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 2);
        public int Exec(IHD6309 cpu) => Exec(cpu, 2);
    }
}
