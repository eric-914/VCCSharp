using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    public class _6F_Clr_X : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            cpu.MemWrite8(0, cpu.INDADDRESS(cpu.PC_REG++));

            cpu.CC_C = false;
            cpu.CC_N = false;
            cpu.CC_V = false;
            cpu.CC_Z = true;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 6);
        public int Exec(IHD6309 cpu) => Exec(cpu, 6);
    }
}
