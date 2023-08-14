using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //ADCA
    //Add memory to accumulator with carry
    //IMMEDIATE
    public class _89_Adca_M : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            byte value = cpu.MemRead8(cpu.PC_REG++);
            ushort mask = (ushort)(cpu.A_REG + value + (cpu.CC_C ? 1 : 0));

            cpu.CC_C = (mask & 0x100) >> 8 != 0;
            cpu.CC_V = OVERFLOW8(cpu.CC_C, value, mask, cpu.A_REG);
            cpu.CC_H = ((cpu.A_REG ^ mask ^ value) & 0x10) >> 4 != 0;

            cpu.A_REG = (byte)mask;

            cpu.CC_N = NTEST8(cpu.A_REG);
            cpu.CC_Z = ZTEST(cpu.A_REG);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 2);
        public int Exec(IHD6309 cpu) => Exec(cpu, 2);
    }
}
