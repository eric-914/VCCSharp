using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page3;

/// <summary>
/// <code>11F7/STF/EXTENDED</code>
/// Store 8-Bit Accumulator <c>F</c> to Memory
/// <code>(M)’ ← F</code>
/// </summary>
/// <remarks>
/// The <c>STF</c> instruction stores the contents of the 8-bit <c>F</c> accumulator to a byte in memory.
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ 0  ]
/// 
/// The Condition Codes are affected as follows.
///         N The Negative flag is set equal to the value of bit 7 of the accumulator.
///         Z The Zero flag is set if the accumulator’s value is zero; cleared otherwise.
///         V The Overflow flag is always cleared.
///         
/// Cycles 6 / 5)
/// Byte Count (4)
///         
/// See Also: ST (16-bit), STQ
internal class _11F7_Stf_E : OpCode6309, IOpCode
{
    public int Exec()
    {
        ushort address = M16[PC]; PC += 2;

        M8[address] = F;

        CC_N = F.Bit7();
        CC_Z = F == 0;
        CC_V = false;

        return DynamicCycles._65;
    }
}
