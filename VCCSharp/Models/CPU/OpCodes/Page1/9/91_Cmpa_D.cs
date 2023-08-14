using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //CMPA
    //Compare memory from accumulator 
    //DIRECT
    public class _91_Cmpa_D : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);
            byte value = cpu.MemRead8(address);
            byte compare = (byte)(cpu.A_REG - value);

            cpu.CC_C = compare > cpu.A_REG;
            cpu.CC_V = OVERFLOW8(cpu.CC_C, value, compare, cpu.A_REG);
            cpu.CC_N = NTEST8(compare);
            cpu.CC_Z = ZTEST(compare);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 4);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._43);
    }
}
