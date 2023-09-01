using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>103B/PULUW/INHERENT</code>
/// Pull Accumulator <c>W</c> from the User <c>(U)</c> Stack
/// <code>
/// ACCW’ ← (U:U+1)
///    U’ ← U + 2
/// </code>
/// </summary>
/// <remarks>
/// The <c>PULUW</c> instruction pulls a value for the <c>W</c> accumulator (<c>E</c> and <c>F</c>) from the User Stack (<c>U</c>).
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// None of the Condition Code flags are affected by this instruction.
/// 
/// The PULUW instruction first loads the high-order byte (Accumulator E) with the value stored at the address pointed to by the user stack pointer (U) and increments the stack pointer by one. 
/// Next, the low-order byte (Accumulator F) is loaded and the stack pointer is again incremented by one.
/// 
/// This instruction was included in the 6309 instruction set to supplement the PULU instruction which does not support the W accumulator.
/// 
/// To pull either half of the W accumulator from the user stack, you could use the instructions LDE ,U+ or LDF ,U+, however these instructions will set the Condition Code flags to reflect the pulled value.
/// 
/// Cycles (6)
/// Byte Count (2)
/// 
/// See Also: PSHSW, PSHUW, PUL, PULSW
internal class _103B_Puluw_I : OpCode6309, IOpCode
{
    internal _103B_Puluw_I(HD6309.IState cpu) : base(cpu) { }

    public int Exec()
    {
        E = M8[U++];
        F = M8[U++];

        return 6;
    }
}
