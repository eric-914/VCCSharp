using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page3;

/// <summary>
/// <code>1196/LDE/DIRECT</code>
/// Load Data into 8-Bit Accumulator <c>E</c>
/// <code>E’ ← IMM8|(M)</code>
/// </summary>
/// <remarks>
/// The <c>LDE</c> instruction loads the contents of a memory byte into the 8-bit <c>E</c> accumulator.
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ 0  ]
/// 
/// The Condition Codes are affected as follows.
///         N The Negative flag is set equal to the new value of bit 7 of the accumulator.
///         Z The Zero flag is set if the new accumulator value is zero; cleared otherwise.
///         V The Overflow flag is always cleared.
/// 
/// Cycles (5 / 4)
/// Byte Count (3)
/// 
/// See Also: LD (16-bit), LDQ
internal class _1196_Lde_D : OpCode6309, IOpCode
{
    internal _1196_Lde_D(HD6309.IState cpu) : base(cpu) { }

    public int Exec()
    {
        ushort address = DIRECT[PC++];

        E = M8[address];

        CC_N = E.Bit7();
        CC_Z = E == 0;
        CC_V = false;

        return Cycles._54;
    }
}
