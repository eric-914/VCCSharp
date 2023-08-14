using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //RTS
    //Return from subroutine
    //INHERENT
    public class _39_Rts_I : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            cpu.PC_H = cpu.MemRead8(cpu.S_REG++);
            cpu.PC_L = cpu.MemRead8(cpu.S_REG++);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 5);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._51);
    }
}
