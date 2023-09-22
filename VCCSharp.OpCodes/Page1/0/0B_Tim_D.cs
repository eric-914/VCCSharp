using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>0B/TIM/DIRECT</code>
/// Bit Test Immediate Value with Memory Byte
/// <code>TEMP ← (M) AND IMM8</code>
/// </summary>
/// <remarks>
/// The <c>TIM</c> instruction logically ANDs the contents of a byte in memory with an 8-bit immediate value. 
/// The resulting value is tested and then discarded. 
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ 0  ]
/// 
/// The Condition Codes are updated to reflect the results of the test as follows:
///         N The Negative flag is set equal to bit 7 of the resulting value.
///         Z The Zero flag is set if the resulting value was zero; cleared otherwise.
///         V The Overflow flag is cleared by this instruction.
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
/// Cycles (6)
/// Byte Count (3)
/// 
/// TIM #i8;EA
/// I8 : 8-bit Immediate value
/// 
/// See Also: AIM, AND, EIM, OIM
internal class _0B_Tim_D : OpCode6309, IOpCode
{
    public int CycleCount => 6;

    public void Exec()
    {
        byte value = M8[PC++];
        ushort address = DIRECT[PC++];
        byte mask = M8[address];

        byte result = (byte)(value & mask);

        CC_N = result.Bit7();
        CC_Z = result == 0;
        CC_V = false;
    }
}
