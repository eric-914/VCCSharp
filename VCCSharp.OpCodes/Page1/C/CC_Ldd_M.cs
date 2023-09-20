using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>CC/LDD/IMMEDIATE</code>
/// Load Data into 16-Bit Register <c>D</c>
/// <code>D’ ← IMM16|(M:M+1)</code>
/// </summary>
/// <remarks>
/// The <c>LDD</c> instruction loads the contents from a pair of memory bytes (in big-endian order) into the 16-bit <c>D</c> accumulator.
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
/// Cycles (3)
/// Byte Count (3)
/// 
/// See Also: LD (8-bit), LDQ, LEA
internal class _CC_Ldd_M : OpCode, IOpCode
{
    public int CycleCount => 3;

    public int Exec()
    {
        D = M16[PC]; PC += 2;

        CC_N = D.Bit15();
        CC_Z = D == 0;
        CC_V = false;

        return CycleCount;
    }
}
