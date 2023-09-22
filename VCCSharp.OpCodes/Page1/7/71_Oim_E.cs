using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>71/OIM/EXTENDED</code>
/// Logical OR of Immediate Value with Memory Byte
/// <code>(M)’ ← (M) OR IMM</code>
/// </summary>
/// <remarks>
/// The <c>OIM</c> instruction logically ORs the contents of a byte in memory with an 8-bit immediate value. 
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
/// OIM is one of the instructions added to the 6309 which allow logical operations to be performed directly in memory instead of having to use an accumulator. 
/// It takes three separate instructions to perform the same operation on a 6809:
///     6809 (6 instruction bytes; 12 cycles):
///         LDA #$C0
///         ORA 4,U
///         STA 4,U
///     6309 (3 instruction bytes; 8 cycles):
///         OIM #$C0;4,U
///         
/// Note that the assembler syntax used for the OIM operand is non-typical. 
/// Some assemblers may require a comma (,) rather than a semicolon (;) between the immediate operand and the address operand.
/// 
/// The object code format for the OIM instruction is:
///     ╭────────┬─────────────┬─────────────────────────╮
///     │ OPCODE │ IMMED VALUE │ ADDRESS / INDEX BYTE(S) │
///     ╰────────┴─────────────┴─────────────────────────╯
///     
/// Cycles (7)
/// Byte Count (4)
/// 
/// OIM #i8;EA
/// I8 : 8-bit Immediate value
/// 
/// See Also: AIM, EIM, TIM
internal class _71_Oim_E : OpCode6309, IOpCode
{
    public int CycleCount => 7;

    public void Exec()
    {
        byte value = M8[PC++];
        ushort address = M16[PC]; PC += 2;

        byte mask = M8[address];

        byte result = (byte)(value | mask);

        CC_N = result.Bit7();
        CC_Z = result == 0;
        CC_V = false;

        M8[address] = result;
    }
}
