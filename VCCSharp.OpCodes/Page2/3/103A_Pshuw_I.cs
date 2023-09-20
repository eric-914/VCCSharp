using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>103A/PSHUW/INHERENT</code>
/// Push Accumulator <c>W</c> onto the User <c>(U)</c> Stack
/// <code>
///       U’ ← U - 2
/// (U:U+1)’ ← ACCW
/// </code>
/// </summary>   
/// <remarks>
/// The <c>PSHSUW</c> instruction pushes the contents of the <c>W</c> accumulator (<c>E</c> and <c>F</c>) onto the User Stack (<c>U</c>). 
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// None of the Condition Code flags are affected by this instruction.
/// 
/// The PSHUW instruction first decrements user stack pointer (U) by one and stores the low-order byte (accumulator F) at the address pointed to by U. 
/// The stack pointer is then decremented by one again, and the high-order byte (accumulator E) is stored.
/// 
/// This instruction was included in the 6309 instruction set to supplement the PSHU instruction which does not support the W accumulator.
/// 
/// To push either half of the W accumulator onto the user stack, you could use the instructions STE ,-U or STF ,-U, however these instructions will set the Condition Code flags to reflect the pushed value.
///  
/// Cycles (6)
/// Byte Count (2)
/// 
/// See Also: PSH, PSHSW, PULSW, PULUW
internal class _103A_Pshuw_I : OpCode6309, IOpCode
{
    public int CycleCount => 6;

    public int Exec()
    {
        M8[--U] = F;
        M8[--U] = E;

        return CycleCount;
    }
}
