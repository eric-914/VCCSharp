using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3
{
    /// <summary>
    /// SUBE
    /// 🚫 6309 ONLY 🚫
    /// Subtract memory from accumulator
    /// Subtract from value in 8-Bit Accumulator
    /// IMMEDIATE
    /// r’ ← r - IMM8|(M)
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// SUBE            IMMEDIATE           1180        3           3 
    ///   [E F H I N Z V C]
    ///   [    ~   ↕ ↕ ↕ ↕]
    /// </summary>
    public class _1180_Sube_M : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            byte value = cpu.MemRead8(cpu.PC_REG++);
            ushort difference = (ushort)(cpu.E_REG - value);

            cpu.CC_C = (difference & 0x100) >> 8 != 0;
            cpu.CC_V = OVERFLOW8(cpu.CC_C, value, difference, cpu.E_REG);

            cpu.E_REG = (byte)difference;

            cpu.CC_Z = ZTEST(cpu.E_REG);
            cpu.CC_N = NTEST8(cpu.E_REG);

            return 3;
        }
    }
}
