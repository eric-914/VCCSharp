using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //RORB
    //Rotate accumulator or memory right
    public class _56_Rorb_I : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            byte mask = cpu.CC_C ? (byte)0x80 : (byte)0x00; //CC_C << 7;

            cpu.CC_C = (cpu.B_REG & 1) != 0;

            cpu.B_REG = (byte)((cpu.B_REG >> 1) | mask);

            cpu.CC_Z = ZTEST(cpu.B_REG);
            cpu.CC_N = NTEST8(cpu.B_REG);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 2);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._21);
    }
}
