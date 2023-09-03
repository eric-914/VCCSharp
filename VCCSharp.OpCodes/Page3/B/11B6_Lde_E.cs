using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page3;

/// <summary>
/// <code>11B6/LDE/EXTENDED</code>
/// Load Data into 8-Bit Accumulator <c>E</c>
/// <code>E’ ← IMM8|(M)</code>
/// </summary>
/// <remarks>
/// The <c>LDE</c> instruction loads the contents of a memory byte into the 8-bit <c>E</c> accumulator.
/// <code>🚫 6309 ONLY 🚫</code>
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
/// Cycles (6 / 5)
/// Byte Count (4)
/// 
/// See Also: LD (16-bit), LDQ
internal class _11B6_Lde_E : OpCode6309, IOpCode
{
    public int Exec()
    {
        ushort address = M16[PC += 2];

        E = M8[address];

        CC_N = E.Bit7();
        CC_Z = E == 0;
        CC_V = false;

        return DynamicCycles._65;
    }
}
