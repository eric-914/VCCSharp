using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>10DF/STS/DIRECT</code>
/// Store <c>S</c> accumulator to memory
/// <code>(M:M+1)’ ← S</code>
/// </summary>
/// <remarks>
/// The <c>STS</c> instruction stores the contents of the 16-bit accumulators <c>S</c> to a pair of memory bytes in big-endian order.
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
/// Cycles (6 / 5)
/// Byte Count (3)
///         
/// See Also: ST (8-bit), STQ
internal class _10DF_Sts_D : OpCode, IOpCode
{
    internal _10DF_Sts_D(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        ushort address = DIRECT[PC++];

        M16[address] = S;

        CC_N = S.Bit15();
        CC_Z = S == 0;
        CC_V = false;

        return DynamicCycles._65;
    }
}
