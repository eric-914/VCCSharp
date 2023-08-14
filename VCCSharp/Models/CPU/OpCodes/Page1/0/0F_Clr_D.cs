using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //CLR
    //Clear accumulator or memory location
    //DIRECT
    public class _0F_Clr_D : OpCode, IOpCode
    {
        public static int Exec(ICpuProcessor cpu, int cycles)
        {
            cpu.MemWrite8(0, cpu.DPADDRESS(cpu.PC_REG++));

            cpu.CC_Z = true;
            cpu.CC_N = false;
            cpu.CC_V = false;
            cpu.CC_C = false;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 6);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._65);
    }
}
