using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>10BE/LDY/EXTENDED</code>
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
/// Cycles (7 / 6)
/// Byte Count (4)
///         
/// See Also: LD (8-bit), LDQ, LEA
internal class _10BE_Ldy_E : OpCode, IOpCode
{
    public int Exec()
    {
        ushort address = M16[PC += 2];

        Y = M16[address];

        CC_N = Y.Bit15();
        CC_Z = Y == 0;
        CC_V = false;

        return DynamicCycles._76;
    }
}
