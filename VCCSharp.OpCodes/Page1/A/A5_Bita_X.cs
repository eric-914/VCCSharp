using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>A5/BITA/INDEXED</code>
/// Bit Test Accumulator <c>A</c> with Memory Byte Value
/// <code>TEMP ← A AND (M)</code>
/// </summary>
/// <remarks>
/// The <c>BITA</c> instruction logically ANDs the contents of a byte in memory with Accumulator <c>A</c>.
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ 0  ]
/// 
/// The 8-bit result is tested to set or clear the appropriate flags in the CC register. 
/// Neither the accumulator nor the memory byte are modified.
///         N The Negative flag is set equal to bit 7 of the resulting value.
///         Z The Zero flag is set if the resulting value was zero; cleared otherwise.
///         V The Overflow flag is cleared by this instruction.
/// 
/// The BIT instructions are used for testing bits. 
/// Consider the following example:
///     ANDA #%00000100 ;Sets Z flag if bit 2 is not set
///     
/// BIT instructions differ from AND instructions only in that they do not modify the specified accumulator.
/// 
/// Cycles (4+)
/// Byte Count (2+)
/// 
/// See Also: AND (8-bit), BITD, BITMD
internal class _A5_Bita_X : OpCode, IOpCode
{
    public int CycleCount => 4;

    public int Exec()
    {
        Cycles = CycleCount;

        ushort address = INDEXED[PC++];
        byte value = M8[address];

        byte result = (byte)(A & value);

        CC_N = result.Bit7();
        CC_Z = result == 0;
        CC_V = false;

        return Cycles;
    }
}
