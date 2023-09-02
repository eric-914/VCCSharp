using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>5F/CLRB/INHERENT</code>
/// Clear Accumulator <c>B</c>
/// <code>B’ ← 0</code>
/// </summary>
/// <remarks>
/// The <c>CLRB</c> instruction clears (sets to zero) the <c>B</c> accumulator. 
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
/// Cycles (2 / 1)
/// Byte Count (1)
/// 
/// See Also: CLR (memory), LD
internal class _5F_Clrb_I : OpCode, IOpCode
{
    public int Exec()
    {
        B = 0;

        CC_N = false;
        CC_Z = true;
        CC_V = false;
        CC_C = false;

        return DynamicCycles._21;
    }
}
