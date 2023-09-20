using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>10B7/STW/EXTENDED</code>
/// Store <c>W</c> accumulator to memory
/// <code>(M:M+1)’ ← W</code>
/// </summary>
/// <remarks>
/// The <c>STW</c> instruction stores the contents of the 16-bit accumulators <c>W</c> to a pair of memory bytes in big-endian order.
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ 0  ]
/// 
/// The Condition Codes are affected as follows:
///         N The Negative flag is set equal to the value in bit 15 of the register.
///         Z The Zero flag is set if the register value is zero; cleared otherwise.
///         V The Overflow flag is always cleared.
///         
/// Cycles (7 / 6)
/// Byte Count (4)
///         
/// See Also: ST (8-bit), STQ
internal class _10B7_Stw_E : OpCode6309, IOpCode
{
    public int CycleCount => DynamicCycles._76;

    public int Exec()
    {
        ushort address = M16[PC]; PC += 2;

        M16[address] = W;

        CC_N = W.Bit15();
        CC_Z = W == 0;
        CC_V = false;

        return CycleCount;
    }
}
