using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>10FC/LDQ/EXTENDED</code>
/// Load Data into 32-Bit Register <c>Q</c>
/// <code>Q’ ← IMM16|(M:M+3)</code>
/// </summary>
/// <remarks>
/// The <c>LDQ</c> instruction loads the contents from a quad set of memory bytes (in big-endian order) into the 32-bit <c>Q</c> stack pointer.
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
/// Cycles (9 / 8)
/// Byte Count (4)
///         
/// See Also: LD (8-bit), LD (16-bit)
internal class _10FC_Ldq_E : OpCode6309, IOpCode
{
    public int CycleCount => DynamicCycles._98;

    public int Exec()
    {
        ushort address = M16[PC]; PC += 2;

        Q = M32[address];

        CC_N = Q.Bit31();
        CC_Z = Q == 0;
        CC_V = false;

        return CycleCount;
    }
}
