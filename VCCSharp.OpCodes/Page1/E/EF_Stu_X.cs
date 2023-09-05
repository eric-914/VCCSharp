using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>EF/STU/INDEXED</code>
/// Store 16-Bit Register <c>U</c> to Memory
/// <code>(M:M+1)’ ← U</code>
/// </summary>
/// <remarks>
/// The <c>STU</c> instruction stores the contents of the 16-bit <c>U</c> accumulator to a pair of memory bytes in big-endian order.
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
/// Cycles (5+ / 2+)
/// Byte Count (2)
/// 
/// See Also: ST (8-bit), STQ
internal class _EF_Stu_X : OpCode, IOpCode
{
    public int Exec()
    {
        Cycles = 5;

        ushort address = INDEXED[PC++];

        M16[address] = U;

        CC_N = U.Bit15();
        CC_Z = U == 0;
        CC_V = false;

        return Cycles;
    }
}
