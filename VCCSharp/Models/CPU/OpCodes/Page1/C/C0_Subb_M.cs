using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1.C
{
    /// <summary>
    /// SUBB
    /// Subtract memory from accumulator
    /// Subtract from value in 8-Bit Accumulator
    /// IMMEDIATE
    /// r’ ← r - IMM8|(M)
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// SUBB            IMMEDIATE           C0          2           2 
    ///   [E F H I N Z V C]
    ///   [    ~   ↕ ↕ ↕ ↕]
    /// </summary>
    public class C0_Subb_M : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            byte value = cpu.MemRead8(cpu.PC_REG++);
            ushort difference = (ushort)(cpu.B_REG - value);

            cpu.CC_C = (difference & 0x100) >> 8 != 0;
            cpu.CC_V = OVERFLOW8(cpu.CC_C, value, difference, cpu.B_REG);

            cpu.B_REG = (byte)difference;

            cpu.CC_Z = ZTEST(cpu.B_REG);
            cpu.CC_N = NTEST8(cpu.B_REG);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 2);
        public int Exec(IHD6309 cpu) => Exec(cpu, 2);
    }
}
