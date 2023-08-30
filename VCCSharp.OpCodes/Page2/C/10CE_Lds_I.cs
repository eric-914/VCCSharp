using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>10CE/LDS/IMMEDIATE</code>
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
/// Cycles (4)
/// Byte Count (4)
///         
/// See Also: LD (8-bit), LDQ, LEA
internal class _10CE_Lds_I : OpCode, IOpCode
{
    internal _10CE_Lds_I(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        S = M16[PC += 2];

        CC_N = S.Bit15();
        CC_Z = S == 0;
        CC_V = false;

        return 4;
    }
}
