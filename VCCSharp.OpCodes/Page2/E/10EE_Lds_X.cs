using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>10EE/LDS/INDEXED</code>
/// Load Data into 16-Bit Register <c>S</c>
/// <code>S’ ← IMM16|(M:M+1)</code>
/// </summary>
/// <remarks>
/// The <c>LDS</c> instruction loads the contents from a pair of memory bytes (in big-endian order) into the 16-bit <c>S</c> stack pointer.
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
/// Cycles (6+)
/// Byte Count (3+)
///         
/// See Also: LD (8-bit), LDQ, LEA
internal class _10EE_Lds_X : OpCode, IOpCode
{
    public int CycleCount => 6;

    public void Exec()
    {
        ushort address = INDEXED[PC++];

        S = M16[address];

        CC_N = S.Bit15();
        CC_Z = S == 0;
        CC_V = false;
    }
}
