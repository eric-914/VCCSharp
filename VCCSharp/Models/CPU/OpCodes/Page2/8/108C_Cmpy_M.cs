using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    public class _108C_Cmpy_M : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort value = cpu.MemRead16(cpu.PC_REG);
            ushort compare = (ushort)(cpu.Y_REG - value);

            cpu.CC_C = compare > cpu.Y_REG;
            cpu.CC_V = OVERFLOW16(cpu.CC_C, value, compare, cpu.Y_REG);
            cpu.CC_N = NTEST16(compare);
            cpu.CC_Z = ZTEST(compare);

            cpu.PC_REG += 2;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 5);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._54);
    }
}
