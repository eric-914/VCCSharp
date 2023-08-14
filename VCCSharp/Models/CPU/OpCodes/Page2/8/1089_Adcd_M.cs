using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    /// <summary>
    /// ADCD
    /// --> 6309 ONLY <--
    /// Add Memory Word plus Carry with Accumulator D
    /// IMMEDIATE
    /// ACCD’ ← ACCD + (M:M+1) + C
    /// SOURCE FORM   ADDRESSING MODE     OPCODE       CYCLES      BYTE COUNT
    /// ADCD          IMMEDIATE           1089         5 / 4       4
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ ↕ ↕]
    /// </summary>
    /// <remarks>
    /// The ADCD instruction adds the contents of a double-byte value in memory plus the valueof the Carry flag with Accumulator D. 
    /// The 16 bit result is placed back into Accumulator D.
    ///     H The Half-Carry flag is not affected by the ADCD instruction.
    ///     N The Negative flag is set equal to the new value of bit 15 of the accumulator.
    ///     Z The Zero flag is set if the new Accumulator D value is zero; cleared otherwise.
    ///     V The Overflow flag is set if an overflow occurred; cleared otherwise.
    ///     C The Carry flag is set if a carry out of bit 15 occurred; cleared otherwise.
    /// The ADCD instruction is most often used to perform addition of subsequent words of a multi-byte addition. 
    /// This allows the carry from a previous ADD or ADC instruction to be included when doing addition for the next higher-order word.
    /// The following instruction sequence is an example showing how 32-bit addition can be performed on a 6309 microprocessor:
    ///     LDQ VAL1 ; Q = first 32-bit value
    ///     ADDW VAL2+2 ; Add lower 16 bits of second value
    ///     ADCD VAL2 ; Add upper 16 bits plus Carry
    ///     STQ RESULT ; Store 32-bit result    
    ///     
    /// See Also: ADC (8-bit), ADCR
    /// </remarks>
    public class _1089_Adcd_M : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort value = cpu.MemRead16(cpu.PC_REG);
            ushort sum = (ushort)(cpu.D_REG + value + (cpu.CC_C ? 1 : 0)); //uint?

            cpu.CC_C = (sum & 0x10000) >> 16 != 0;
            cpu.CC_V = OVERFLOW16(cpu.CC_C, value, sum, cpu.D_REG);
            cpu.CC_H = ((cpu.D_REG ^ sum ^ value) & 0x100) >> 8 != 0;
            cpu.D_REG = sum;
            cpu.CC_N = NTEST16(cpu.D_REG);
            cpu.CC_Z = ZTEST(cpu.D_REG);

            cpu.PC_REG += 2;

            return Cycles._54;
        }
    }
}
