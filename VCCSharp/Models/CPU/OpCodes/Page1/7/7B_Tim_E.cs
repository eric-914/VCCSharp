using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// TIM
    /// 🚫 6309 ONLY 🚫
    /// Bit Test Immediate Value with Memory Byte
    /// EXTENDED
    /// TEMP ← (M) AND IMM8
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// TIM #i8;EA      DIRECT              0B          6           3 
    ///                 INDEXED             6B          7+          3+ 
    ///                 EXTENDED            7B          7           4
    ///                 
    /// I8 : 8-bit Immediate value
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ 0  ]
    /// </summary>
    /// <remarks>
    /// The TIM instruction logically ANDs the contents of a byte in memory with an 8-bit immediate value. 
    /// The resulting value is tested and then discarded. 
    /// The Condition Codes are updated to reflect the results of the test as follows:
    ///     N The Negative flag is set equal to bit 7 of the resulting value.
    ///     Z The Zero flag is set if the resulting value was zero; cleared otherwise.
    ///     V The Overflow flag is cleared by this instruction.
    ///     C The Carry flag is not affected by this instruction.
    ///     
    /// TIM can be used as a space-saving optimization for a pair of equivalent 6809 instructions, and to perform a bit test without having to utilize a register. 
    /// However, it is slower than the 6809 equivalent:
    /// 
    ///     6809: (4 instruction bytes; 7 cycles):
    ///         LDA #$3F
    ///         BITA 4,U
    ///     6309: (3 instruction bytes; 8 cycles):
    ///         TIM #$3F;4,U
    ///         
    /// Note that the assembler syntax used for the TIM operand is non-typical. 
    /// Some assemblers may require a comma (,) rather than a semicolon (;) between the immediate operand and the address operand.
    /// The object code format for the TIM instruction is:
    ///     ╭────────┬─────────────┬─────────────────────────╮
    ///     │ OPCODE │ IMMED VALUE │ ADDRESS / INDEX BYTE(S) │
    ///     ╰────────┴─────────────┴─────────────────────────╯
    /// 
    /// See Also: AIM, AND, EIM, OIM
    /// </remarks>
    public class _7B_Tim_E : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            byte value = cpu.MemRead8(cpu.PC_REG++);
            ushort address = cpu.MemRead16(cpu.PC_REG);
            byte mask = cpu.MemRead8(address);

            value &= mask;

            cpu.CC_N = NTEST8(value);
            cpu.CC_Z = ZTEST(value);
            cpu.CC_V = false;

            cpu.PC_REG += 2;

            return 7;
        }
    }
}
