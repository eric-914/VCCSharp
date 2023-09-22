using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page3;

/// <summary>
/// <code>1186/LDE/IMMEDIATE</code>
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
/// Cycles (3)
/// Byte Count (3)
/// 
/// See Also: LD (16-bit), LDQ
internal class _1186_Lde_M : OpCode6309, IOpCode
{
    public int CycleCount => 3;

    public void Exec()
    {
        E = M8[PC++];

        CC_N = E.Bit7();
        CC_Z = E == 0;
        CC_V = false;
    }
}
