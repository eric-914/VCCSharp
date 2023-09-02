using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>1086/LDW/IMMEDIATE</code>
/// Load Data into 16-Bit Register <c>W</c>
/// <code>W’ ← IMM16|(M:M+1)</code>
/// </summary>
/// <remarks>
/// The <c>LDW</c> instruction loads the contents from a pair of memory bytes (in big-endian order) into the 16-bit <c>W</c> accumulator.
/// <code>🚫 6309 ONLY 🚫</code>
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
internal class _1086_Ldw_M : OpCode6309, IOpCode
{
    internal _1086_Ldw_M(HD6309.IState cpu) : base(cpu) { }

    public int Exec()
    {
        W = M16[PC += 2];

        CC_N = W.Bit15();
        CC_Z = W == 0;
        CC_V = false;

        return DynamicCycles._54;
    }
}
