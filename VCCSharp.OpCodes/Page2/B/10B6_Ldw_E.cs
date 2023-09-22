using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>10B6/LDW/EXTENDED</code>
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
/// Cycles (7 / 6)
/// Byte Count (4)
///         
/// See Also: LD (8-bit), LDQ, LEA
internal class _10B6_Ldw_E : OpCode6309, IOpCode
{
    public int CycleCount => DynamicCycles._76;

    public void Exec()
    {
        ushort address = M16[PC]; PC += 2;

        W = M16[address];

        CC_N = W.Bit15();
        CC_Z = W == 0;
        CC_V = false;
    }
}
