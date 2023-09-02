using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page3;

/// <summary>
/// <code>11F6/LDF/EXTENDED</code>
/// Load Data into 8-Bit Accumulator <c>F</c>
/// <code>F’ ← IMM8|(M)</code>
/// </summary>
/// <remarks>
/// The <c>LDF</c> instruction loads the contents of a memory byte into the 8-bit <c>F</c> accumulator.
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
internal class _11F6_Ldf_E : OpCode6309, IOpCode
{
    public int Exec()
    {
        ushort address = M16[PC += 2];

        F = M8[address];

        CC_N = F.Bit7();
        CC_Z = F == 0;
        CC_V = false;

        return DynamicCycles._65;
    }
}
