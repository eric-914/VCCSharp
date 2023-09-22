using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page3;

/// <summary>
/// <code>11D6/LDF/DIRECT</code>
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
/// Cycles (5 / 4)
/// Byte Count (3)
/// 
/// See Also: LD (16-bit), LDQ
internal class _11D6_Ldf_D : OpCode6309, IOpCode
{
    public int CycleCount => DynamicCycles._54;

    public void Exec()
    {
        ushort address = DIRECT[PC++];

        F = M8[address];

        CC_N = F.Bit7();
        CC_Z = F == 0;
        CC_V = false;
    }
}
