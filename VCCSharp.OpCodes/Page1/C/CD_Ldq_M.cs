using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>CD/LDQ/IMMEDIATE</code>
/// Load 32-bit Data into Accumulator <c>Q</c>
/// <code>Q’ ← IMM32|(M:M+3)</code>
/// </summary>
/// <remarks>
/// The <c>LDQ</c> instruction loads a 32-bit immediate value into the <c>Q</c> accumulator. 
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ 0  ]
/// 
/// The Condition Codes are affected as follows.
///         N The Negative flag is set equal to the new value of bit 31 of Accumulator Q.
///         Z The Zero flag is set if the new value of Accumulator Q is zero; cleared otherwise.
///         V The Overflow flag is always cleared.
///         
/// Cycles (5)
/// Byte Count (5)
/// 
/// See Also: LD (8-bit), LD (16-bit)
internal class _CD_Ldq_M : OpCode6309, IOpCode
{
    public int CycleCount => 5;

    public void Exec()
    {
        Q = M32[PC+=4];

        CC_N = Q.Bit31();
        CC_Z = Q == 0;
        CC_V = false;
    }
}
