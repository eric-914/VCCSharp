using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>104F/CLRD/INHERENT</code>
/// Load Zero into Accumulator <c>D</c>
/// <code>D’ ← 0</code>
/// </summary>
/// <remarks>
/// The <c>CLRD</c> instructions clears (sets to zero) the <c>D</c> accumulator. 
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        0 0 0 0]
/// 
/// The Condition Code flags are also modified as follows:
///         N The Negative flag is cleared.
///         Z The Zero flag is set.
///         V The Overflow flag is cleared.
///         C The Carry flag is cleared.
///     
/// Clearing the Q accumulator can be accomplished by executing both CLRD and CLRW.
/// 
/// To clear any of the Index Registers (X, Y, U or S), you can use either an Immediate Mode LD instruction or, on 6309 processors only, a TFR or EXG instruction which specifies the Zero register (0) as the source.
/// 
/// Cycles (3 / 2)
/// Byte Count (2)
/// 
/// See Also: CLR (memory), LD
internal class _104F_Clrd_I : OpCode6309, IOpCode
{
    public int Exec()
    {
        CC_N = false;
        CC_Z = true;
        CC_V = false;
        CC_C = false;

        D = 0;

        return DynamicCycles._32;
    }
}
