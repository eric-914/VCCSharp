using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    /// <summary>
    /// SUBW
    /// 🚫 6309 ONLY 🚫
    /// Subtract memory from D accumulator
    /// Subtract from value in 16-Bit Accumulator
    /// IMMEDIATE
    /// r’ ← r - IMM16|(M:M+1)
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// SUBW            IMMEDIATE           1080        5 / 4       4 
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ ↕ ↕]
    /// </summary>
    /// <remarks>
    /// These instructions subtract either a 16-bit immediate value or the contents of a doublebyte value in memory from one of the 16-bit accumulators (D,W). 
    /// The 16-bit result is placed back into the specified accumulator. 
    /// Note that since subtraction is performed, the purpose of the Carry flag is to represent a Borrow.
    ///         H The Half-Carry flag is not affected by these instructions.
    ///         N The Negative flag is set equal to the new value of bit 15 of the accumulator.
    ///         Z The Zero flag is set if the new accumulator value is zero; cleared otherwise.
    ///         V The Overflow flag is set if an overflow occurred; cleared otherwise.
    ///         C The Carry flag is set if a borrow out of bit 7 was needed; cleared otherwise.
    ///         
    /// The 16-bit SUB instructions are used for 16-bit subtraction, and for subtraction of the least-significant word of multi-byte subtractions. 
    /// See the description of the SBCD instruction for an example of how 32-bit subtraction can be performed on a 6309.
    /// 
    /// See Also: SUB (8-bit), SUBR
    /// </remarks>
    public class _1080_Subw_M : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort value = cpu.MemRead16(cpu.PC_REG);
            ushort difference = (ushort)(cpu.W_REG - value);

            cpu.CC_C = difference > cpu.W_REG;
            cpu.CC_V = OVERFLOW16(cpu.CC_C, difference, cpu.W_REG, value);
            cpu.CC_N = NTEST16(difference);
            cpu.CC_Z = ZTEST(difference);
            cpu.W_REG = difference;

            cpu.PC_REG += 2;

            return Cycles._54;
        }
    }
}
