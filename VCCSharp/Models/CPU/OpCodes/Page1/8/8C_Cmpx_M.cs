using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //CMPX
    //Compare memory from index register
    public class _8C_Cmpx_M : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort value = cpu.MemRead16(cpu.PC_REG);
            ushort compare = (ushort)(cpu.X_REG - value);

            cpu.CC_C = compare > cpu.X_REG;
            cpu.CC_V = OVERFLOW16(cpu.CC_C, value, compare, cpu.X_REG);
            cpu.CC_N = NTEST16(compare);
            cpu.CC_Z = ZTEST(compare);

            cpu.PC_REG += 2;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 4);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._43);
    }
}
