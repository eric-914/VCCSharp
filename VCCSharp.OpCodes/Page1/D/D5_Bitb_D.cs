using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>D5/BITB/DIRECT</code>
/// Bit Test Accumulator <c>B</c> with Memory Byte Value
/// <code>TEMP ← B AND (M)</code>
/// </summary>
/// <remarks>
/// The <c>BITB</c> instruction logically ANDs the contents of a byte in memory with Accumulator <c>B</c>.
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
/// Cycles (4 / 3)
/// Byte Count (2)
///         
/// See Also: AND (8-bit), BITD, BITMD
internal class _D5_Bitb_D : OpCode, IOpCode
{
    public int CycleCount => DynamicCycles._43;

    public void Exec()
    {
        ushort address = DIRECT[PC++];
        byte value = (byte)(B & M8[address]);

        CC_N = value.Bit7();
        CC_Z = value == 0;
        CC_V = false;
    }
}
