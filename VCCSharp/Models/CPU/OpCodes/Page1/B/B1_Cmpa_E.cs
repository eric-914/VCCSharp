using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1.B
{
    //CMPA
    //Compare memory from accumulator 
    //EXTENDED
    public class B1_Cmpa_E : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.MemRead16(cpu.PC_REG);
            byte value = cpu.MemRead8(address);
            byte compare = (byte)(cpu.A_REG - value);

            cpu.CC_C = compare > cpu.A_REG;
            cpu.CC_V = OVERFLOW8(cpu.CC_C, value, compare, cpu.A_REG);
            cpu.CC_N = NTEST8(compare);
            cpu.CC_Z = ZTEST(compare);

            cpu.PC_REG += 2;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 5);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._54);
    }
}
