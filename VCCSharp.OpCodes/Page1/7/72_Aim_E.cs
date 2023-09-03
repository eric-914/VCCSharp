using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>72/AIM/EXTENDED</code>
/// Logical AND of Immediate Value with Memory Byte
/// <code>M’ ← (M) AND IMM</code>
/// </summary>
/// <remarks>
/// The <c>AIM</c> instruction logically ANDs the contents of a byte in memory with an 8-bit immediate value. 
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ 0  ]
///   
/// The resulting value is placed back into the designated memory location.
///         N The Negative flag is set equal to the new value of bit 7 of the memory byte.
///         Z The Zero flag is set if the new value of the memory byte is zero; cleared otherwise.
///         V The Overflow flag is cleared by this instruction.
///
/// AIM is one of the more useful additions to the 6309 instruction set. 
/// It takes three separate instructions to perform the same operation on a 6809:
/// 
///     6809 (6 instruction bytes; 12 cycles):
///         LDA #$3F
///         ANDA 4,U
///         STA 4,U
///     6309 (3 instruction bytes; 8 cycles):
///         AIM #$3F;4,U
///     
/// Note that the assembler syntax used for the AIM operand is non-typical. 
/// Some assemblers may require a comma (,) rather than a semicolon (;) between the immediate operand and the address operand.
/// 
/// The object code format for the AIM instruction is:
///     ╭────────┬─────────────┬─────────────────────────╮
///     │ OPCODE │ IMMED VALUE │ ADDRESS / INDEX BYTE(S) │
///     ╰────────┴─────────────┴─────────────────────────╯
///     
/// Cycles (7)
/// Byte Count (4)
/// 
/// AIM #i8;EA
/// I8 : 8-bit Immediate value
/// EA : Effective Address
/// 
/// See Also: AND, EIM, OIM, TIM
internal class _72_Aim_E : OpCode6309, IOpCode
{
    public int Exec()
    {
        byte value = M8[PC++]; PC += 2;
        ushort address = M16[PC];

        byte mask = M8[address];

        byte result = (byte)(value & mask);

        CC_N = result.Bit7();
        CC_Z = result == 0;
        CC_V = false;

        M8[address] = result;

        return 7;
    }
}
