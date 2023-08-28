using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>1038/PSHSW/INHERENT</code>
/// Push Accumulator <c>W</c> onto the Hardware <c>(S)</c> Stack
/// <code>
///       S’ ← S - 2
/// (S:S+1)’ ← ACCW
/// </code>
/// </summary>
/// <remarks>
/// The <c>PSHSW</c> instruction pushes the contents of the <c>W</c> accumulator (<c>E</c> and <c>F</c>) onto the Hardware Stack (<c>S</c>). 
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// None of the Condition Code flags are affected by this instruction.
/// 
/// The PSHSW instruction first decrements hardware stack pointer (S) by one and stores the low-order byte (Accumulator F) at the address pointed to by S. 
/// The stack pointer is then decremented by one again, and the high-order byte (Accumulator E) is stored.
/// 
/// This instruction was included in the 6309 instruction set to supplement the PSHS instruction which does not support the W accumulator.
/// 
/// To push either half of the W accumulator onto the hardware stack, you could use the instructions STE ,-S or STF ,-S, however these instructions will set the Condition Code flags to reflect the pushed value.
/// 
/// Cycles (6)
/// Byte Count (2)
/// 
/// See Also: PSH, PSHUW, PULSW, PULUW
internal class _1038_Pshsw : OpCode6309, IOpCode
{
    internal _1038_Pshsw(HD6309.IState cpu) : base(cpu) { }

    public int Exec()
    {
        M8[--S] = F;
        M8[--S] = E;

        return 6;
    }
}
