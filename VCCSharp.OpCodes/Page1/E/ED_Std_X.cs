using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>ED/STD/INDEXED</code>
/// Store 16-Bit Register <c>D</c> to Memory
/// <code>(M:M+1)’ ← D</code>
/// </summary>
/// <remarks>
/// The <c>STD</c> instruction stores the contents of the 16-bit <c>D</c> accumulator to a pair of memory bytes in big-endian order.
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
/// Cycles (5+)
/// Byte Count (2+)
/// 
/// See Also: ST (8-bit), STQ
internal class _ED_Std_X : OpCode, IOpCode
{
    public int Exec()
    {
        Cycles = 5;

        ushort address = INDEXED[PC++];

        M16[address] = D;

        CC_N = D.Bit15();
        CC_Z = D == 0;
        CC_V = false;

        return Cycles;
    }
}
