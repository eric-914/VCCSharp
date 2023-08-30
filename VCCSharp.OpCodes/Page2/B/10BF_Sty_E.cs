using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>10BF/STY/EXTENDED</code>
/// Store <c>Y</c> accumulator to memory
/// <code>(M:M+1)’ ← Y</code>
/// </summary>
/// <remarks>
/// The <c>STY</c> instruction stores the contents of the 16-bit accumulators <c>Y</c> to a pair of memory bytes in big-endian order.
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
internal class _10BF_Sty_E : OpCode, IOpCode
{
    internal _10BF_Sty_E(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        ushort address = M16[PC += 2];

        M16[address] = Y;

        CC_N = Y.Bit15();
        CC_Z = Y == 0;
        CC_V = false;

        return Cycles._76;
    }
}
