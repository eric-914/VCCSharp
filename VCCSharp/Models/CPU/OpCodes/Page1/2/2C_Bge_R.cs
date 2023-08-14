using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //BGE
    //Branch if greater than or equal (signed)
    //RELATIVE
    public class _2C_Bge_R : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            if (!(cpu.CC_N ^ cpu.CC_V))
            {
                cpu.PC_REG += (ushort)(sbyte)cpu.MemRead8(cpu.PC_REG);
            }

            cpu.PC_REG++;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 3);
        public int Exec(IHD6309 cpu) => Exec(cpu, 3);
    }
}
