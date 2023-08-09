using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    public class _16_Lbra_R : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            var word = cpu.MemRead16(cpu.PC_REG);

            cpu.PC_REG += 2;
            cpu.PC_REG += word;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 5);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._54);
    }
}
