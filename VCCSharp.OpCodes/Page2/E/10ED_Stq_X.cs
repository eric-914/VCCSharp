using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>10ED/STQ/INDEXED</code>
/// Store <c>Q</c> accumulator to memory
/// <code>(M:M+3)’ ← Q</code>
/// </summary>
/// <remarks>
/// The <c>STQ</c> instruction stores the contents of the 32-bit accumulators <c>S</c> into 4 sequential bytes in big-endian order.
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ 0  ]
///   
/// The Condition Codes are affected as follows.
///         N The Negative flag is set equal to the value of bit 31 of Accumulator Q.
///         Z The Zero flag is set if the value of Accumulator Q is zero; cleared otherwise.
///         V The Overflow flag is always cleared.
/// 
/// Cycles (8+)
/// Byte Count (3+)
///         
/// See Also: ST (8-bit), ST (16-bit)
internal class _10ED_Stq_X : OpCode6309, IOpCode
{
    public int CycleCount => 8;

    public int Exec()
    {
        Cycles = CycleCount;

        ushort address = INDEXED[PC++];

        M32[address] = Q;

        CC_N = Q.Bit31();
        CC_Z = Q == 0;
        CC_V = false;

        return Cycles;
    }
}
