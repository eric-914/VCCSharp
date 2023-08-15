using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3
{
    /// <summary>
    /// ADDE
    /// 🚫 6309 ONLY 🚫
    /// Add memory to accumulator
    /// Add Memory Byte to 8-Bit Accumulator
    /// IMMEDIATE
    /// r’ ← r + (M)
    /// SOURCE FORM   ADDRESSING MODE     OPCODE       CYCLES      BYTE COUNT
    /// ADDE          IMMEDIATE           118B         3           3
    ///   [E F H I N Z V C]
    ///   [    ↕   ↕ ↕ ↕ ↕]
    /// </summary>
    /// <remarks>
    /// These instructions add the contents of a byte in memory with one of the 8-bit accumulators (A,B,E,F). 
    /// The 8-bit result is placed back into the specified accumulator.
    ///     H The Half-Carry flag is set if a carry into bit 4 occurred; cleared otherwise.
    ///     N The Negative flag is set equal to the new value of bit 7 of the accumulator.
    ///     Z The Zero flag is set if the new accumulator value is zero; cleared otherwise.
    ///     V The Overflow flag is set if an overflow occurred; cleared otherwise.
    ///     C The Carry flag is set if a carry out of bit 7 occurred; cleared otherwise.
    /// The 8-bit ADD instructions are used for single-byte addition, and for addition of the least-significant byte in multi-byte additions. 
    /// Since the 6x09 also provides a 16-bit ADD instruction, it is not necessary to use the 8-bit ADD and ADC instructions for performing 16-bit addition.
    /// 
    /// See Also: ADD (16-bit), ADDR
    /// </remarks>
    public class _118B_Adde_M : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            byte value = cpu.MemRead8(cpu.PC_REG++);
            ushort sum = (ushort)(cpu.E_REG + value);

            cpu.CC_C = (sum & 0x100) >> 8 != 0;
            cpu.CC_H = ((cpu.E_REG ^ value ^ sum) & 0x10) >> 4 != 0;
            cpu.CC_V = OVERFLOW8(cpu.CC_C, value, sum, cpu.E_REG);

            cpu.E_REG = (byte)sum;
            
            cpu.CC_N = NTEST8(cpu.E_REG);
            cpu.CC_Z = ZTEST(cpu.E_REG);

            return 3;
        }
    }
}
