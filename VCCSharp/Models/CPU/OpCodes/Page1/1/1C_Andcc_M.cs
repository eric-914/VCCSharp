using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //ANDCC
    //AND condition code register
    //IMMEDIATE
    public class _1C_Andcc_M : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            byte value = cpu.MemRead8(cpu.PC_REG++);
            byte mask = cpu.CC;

            mask = (byte)(mask & value);

            cpu.CC = mask;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 3);
        public int Exec(IHD6309 cpu) => Exec(cpu, 3);
    }
}
