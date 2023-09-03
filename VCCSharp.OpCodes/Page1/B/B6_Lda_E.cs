using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>B6/LDA/EXTENDED</code>
/// Load Data into 8-Bit Accumulator <c>A</c>
/// <code>A’ ← IMM8|(M)</code>
/// </summary>
/// <remarks>
/// The <c>LDA</c> instruction loads the contents of a memory byte into the 8-bit <c>A</c> accumulator.
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ 0  ]
/// 
/// The Condition Codes are affected as follows.
///         N The Negative flag is set equal to the new value of bit 7 of the accumulator.
///         Z The Zero flag is set if the new accumulator value is zero; cleared otherwise.
///         V The Overflow flag is always cleared.
/// 
/// Cycles (5 / 4)
/// Byte Count (3)
/// 
/// See Also: LD (16-bit), LDQ
internal class _B6_Lda_E : OpCode, IOpCode
{
    public int Exec()
    {
        ushort address = M16[PC]; PC += 2;

        A = M8[address];

        CC_Z = A == 0;
        CC_N = A.Bit7();
        CC_V = false;

        return DynamicCycles._54;
    }
}
