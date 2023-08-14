using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //BRA
    //Branch always
    //RELATIVE
    public class _20_Bra_R : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            sbyte offset = (sbyte)cpu.MemRead8(cpu.PC_REG++);

            cpu.PC_REG += (ushort)offset;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 3);
        public int Exec(IHD6309 cpu) => Exec(cpu, 3);
    }
}
