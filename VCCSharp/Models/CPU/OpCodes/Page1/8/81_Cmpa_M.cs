using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //CMPA
    //Compare memory from accumulator 
    public class _81_Cmpa_M : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            byte value = cpu.MemRead8(cpu.PC_REG++);
            byte mask = (byte)(cpu.A_REG - value);

            cpu.CC_C = mask > cpu.A_REG;
            cpu.CC_V = OVERFLOW8(cpu.CC_C, value, mask, cpu.A_REG);
            cpu.CC_N = NTEST8(mask);
            cpu.CC_Z = ZTEST(mask);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 2);
        public int Exec(IHD6309 cpu) => Exec(cpu, 2);
    }
}
