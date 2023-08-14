using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1.C
{
    //ADDB
    //Add memory to accumulator
    //IMMEDIATE
    public class CB_Addb_M : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            byte value = cpu.MemRead8(cpu.PC_REG++);
            ushort sum = (ushort)(cpu.B_REG + value);

            cpu.CC_C = (sum & 0x100) >> 8 != 0;
            cpu.CC_H = ((cpu.B_REG ^ value ^ sum) & 0x10) >> 4 != 0;
            cpu.CC_V = OVERFLOW8(cpu.CC_C, value, sum, cpu.B_REG);

            cpu.B_REG = (byte)sum;

            cpu.CC_N = NTEST8(cpu.B_REG);
            cpu.CC_Z = ZTEST(cpu.B_REG);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 2);
        public int Exec(IHD6309 cpu) => Exec(cpu, 2);
    }
}
