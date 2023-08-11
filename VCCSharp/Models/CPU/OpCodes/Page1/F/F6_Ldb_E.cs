using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1.F
{
    public class F6_Ldb_E : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.MemRead16(cpu.PC_REG);

            cpu.B_REG = cpu.MemRead8(address);

            cpu.CC_Z = ZTEST(cpu.B_REG);
            cpu.CC_N = NTEST8(cpu.B_REG);
            cpu.CC_V = false;

            cpu.PC_REG += 2;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 5);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._54);
    }
}
