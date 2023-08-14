using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    /// <summary>
    /// ADDW
    /// --> 6309 ONLY <--
    /// Add memory to W accumulator
    /// Add Memory Word to 16-Bit Accumulator
    /// DIRECT
    /// r’ ← r + (M:M+1)
    /// SOURCE FORM   ADDRESSING MODE     OPCODE       CYCLES      BYTE COUNT
    /// ADDW          DIRECT              109B         7 / 5       3
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ ↕ ↕]
    /// </summary>
    /// <remarks>
    /// These instructions add the contents of a double-byte value in memory with one of the 16-bit accumulators (D,W). 
    /// The 16-bit result is placed back into the specified accumulator.
    ///     H The Half-Carry flag is not affected by these instructions.
    ///     N The Negative flag is set equal to the new value of bit 15 of the accumulator.
    ///     Z The Zero flag is set if the new accumulator value is zero; cleared otherwise.
    ///     V The Overflow flag is set if an overflow occurred; cleared otherwise.
    ///     C The Carry flag is set if a carry out of bit 15 occurred; cleared otherwise.
    /// The 16-bit ADD instructions are used for double-byte addition, and for addition of the least-significant word of multi-byte additions. 
    /// See the description of the ADCD instruction for an example of how 32-bit addition can be performed on a 6309 processor.
    /// 
    /// See Also: ADD (8-bit), ADDR
    /// </remarks>
    public class _109B_Addw_D : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);
            ushort value = cpu.MemRead16(address);
            uint sum = (uint)(cpu.W_REG + value);

            cpu.CC_C = (sum & 0x10000) >> 16 != 0;
            cpu.CC_V = OVERFLOW16(cpu.CC_C, sum, value, cpu.W_REG);

            cpu.W_REG = (ushort)sum;

            cpu.CC_Z = ZTEST(cpu.W_REG);
            cpu.CC_N = NTEST16(cpu.W_REG);

            return Cycles._75;
        }
    }
}
