using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>1039/PULSW/INHERENT</code>
/// Pull Accumulator <c>W</c> from the Hardware <c>(S)</c> Stack
/// <code>
/// ACCW’ ← (S:S+1)
///    S’ ← S + 2
/// </code>
/// </summary>
/// <remarks>
/// The <c>PULSW</c> instruction pulls a value for the <c>W</c> accumulator (<c>E</c> and <c>F</c>) from the Hardware Stack (<c>S</c>). 
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// None of the Condition Code flags are affected by this instruction.
/// 
/// The PULSW instruction first loads the high-order byte (Accumulator E) with the value stored at the address pointed to by the hardware stack pointer (S) and increments the stack pointer by one. 
/// Next, the low-order byte (Accumulator F) is loaded and the stack pointer is again incremented by one.
/// 
/// This instruction was included in the 6309 instruction set to supplement the PULS instruction which does not support the W accumulator.
/// 
/// To pull either half of the W accumulator from the hardware stack, you could use the instructions LDE ,S+ or LDF ,S+, however these instructions will set the Condition Code flags to reflect the pulled value.
/// 
/// Cycles (6)
/// Byte Count (2)
/// 
/// See Also: PSHSW, PSHUW, PUL, PULUW
internal class _1039_Pulsw_I : OpCode6309, IOpCode
{
    internal _1039_Pulsw_I(HD6309.IState cpu) : base(cpu) { }

    public int Exec()
    {
        E = M8[S++];
        F = M8[S++];

        return 6;
    }
}
