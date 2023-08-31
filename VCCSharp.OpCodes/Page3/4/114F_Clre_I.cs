using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page3;

/// <summary>
/// <code>114F/CLRE/INHERENT</code>
/// Clear Accumulator <c>E</c>
/// <code>E’ ← 0</code>
/// </summary>
/// <remarks>
/// The <c>CLRE</c> instruction clears (sets to zero) the <c>E</c> accumulator. 
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        0 1 0 0]
/// 
/// The Condition Code flags are also modified as follows:
///         N The Negative flag is cleared.
///         Z The Zero flag is set.
///         V The Overflow flag is cleared.
///         C The Carry flag is cleared.
/// 
/// The CLRA and CLRB instructions provide the smallest, fastest way to clear the Carry flag in the CC register.
/// 
/// Cycles (3 / 2)
/// Byte Count (2)
/// 
/// See Also: CLR (memory), LD
internal class _114F_Clre_I : OpCode6309, IOpCode
{
    internal _114F_Clre_I(HD6309.IState cpu) : base(cpu) { }

    public int Exec()
    {
        E = 0;

        CC_N = false;
        CC_Z = true;
        CC_V = false;
        CC_C = false;

        return Cycles._32;
    }
}
