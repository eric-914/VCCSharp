using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>108E/LDY/IMMEDIATE</code>
/// Load Data into 16-Bit Register <c>Y</c>
/// <code>Y’ ← IMM16|(M:M+1)</code>
/// </summary>
/// <remarks>
/// The <c>LDY</c> instruction loads the contents from a pair of memory bytes (in big-endian order) into the 16-bit <c>Y</c> accumulator.
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
internal class _108E_Ldy_M : OpCode, IOpCode
{
    internal _108E_Ldy_M(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        Y = M16[PC += 2];

        CC_N = Y.Bit15();
        CC_Z = Y == 0;
        CC_V = false;

        return 4;
    }
}
