using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1.B
{
    //BSR
    //Branch to subroutine 
    //EXTENDED
    public class BD_Bsr_E : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.MemRead16(cpu.PC_REG);

            cpu.PC_REG += 2;

            cpu.S_REG--;

            cpu.MemWrite8(cpu.PC_L, cpu.S_REG--);
            cpu.MemWrite8(cpu.PC_H, cpu.S_REG);

            cpu.PC_REG = address;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 8);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._87);
    }
}
