using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    public class _8D_Bsr_R : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            var _postByte = cpu.MemRead8(cpu.PC_REG++);

            cpu.S_REG--;

            cpu.MemWrite8(cpu.PC_L, cpu.S_REG--);
            cpu.MemWrite8(cpu.PC_H, cpu.S_REG);

            cpu.PC_REG += (ushort)(sbyte)(_postByte);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 7);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._76);
    }
}
