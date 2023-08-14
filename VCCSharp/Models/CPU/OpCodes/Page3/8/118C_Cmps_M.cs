using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3
{
    //CMPS
    //Compare memory from stack pointer
    //IMMEDIATE
    public class _118C_Cmps_M : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            var value = cpu.MemRead16(cpu.PC_REG);
            var compare = (ushort)(cpu.S_REG - value);

            cpu.CC_C = compare > cpu.S_REG;
            cpu.CC_V = OVERFLOW16(cpu.CC_C, value, compare, cpu.S_REG);
            cpu.CC_N = NTEST16(compare);
            cpu.CC_Z = ZTEST(compare);
            
            cpu.PC_REG += 2;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 5);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._54);
    }
}
