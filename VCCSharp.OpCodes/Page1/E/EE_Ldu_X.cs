using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>EE/LDU/INDEXED</code>
/// Load Data into 16-Bit Register <c>U</c>
/// <code>U’ ← IMM16|(M:M+1)</code>
/// </summary>
/// <remarks>
/// The <c>LDU</c> instruction loads the contents from a pair of memory bytes (in big-endian order) into the 16-bit <c>U</c> accumulator.
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ 0  ]
///   
/// The Condition Codes are affected as follows.
///         N The Negative flag is set equal to the new value of bit 15 of the register.
///         Z The Zero flag is set if the new register value is zero; cleared otherwise.
///         V The Overflow flag is always cleared.
///         
/// Cycles (5+ / 2+)
/// Byte Count (2)
/// 
/// See Also: LD (8-bit), LDQ, LEA
internal class _EE_Ldu_X : OpCode, IOpCode
{
    public int Exec()
    {
        ushort address = INDEXED[PC++];

        U = M16[address];

        CC_N = U.Bit15();
        CC_Z = U == 0;
        CC_V = false;

        return 5;
    }
}
