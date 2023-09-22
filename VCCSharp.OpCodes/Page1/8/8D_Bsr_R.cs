using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>8D/BSR/RELATIVE</code>
/// Branch to subroutine 
/// <code>
///      S’ ← S - 2
/// (S:S+1) ← PC
///     PC’ ← PC + IMM
/// </code>
/// </summary>
/// <remarks>
/// The <c>BSR</c> instruction pushes the value of the <c>PC</c> register (after the <c>BSR</c> instruction bytes have been fetched) onto the hardware stack and then performs an unconditional relative branch. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
///   
/// None of the Condition Code flags are affected.
/// 
/// By pushing the PC value onto the stack, the called subroutine can "return" to this address after it has completed.
/// 
/// The BSR instruction is similar in function to the JSR instruction. 
/// The significant difference is that BSR uses the Relative Addressing mode which implies that both the BSR instruction and the called subroutine may be contained in relocatable code, so long as both are contained in the same module.
/// 
/// The branch address is calculated by adding the current value of the PC register (after the BSR instruction bytes have been fetched) with the 8-bit twos-complement value contained in the second byte of the instruction. 
/// The range of the branch destination is limited to -126 to +129 bytes from the address of the BSR instruction. 
/// If a larger range is required then the LBSR instruction may be used instead.
/// 
/// Cycles (7 / 6)
/// Byte Count (2)
/// 
/// See Also: JSR, LBSR, RTS
internal class _8D_Bsr_R : OpCode, IOpCode
{
    public int CycleCount => DynamicCycles._76;

    public void Exec()
    {
        sbyte offset = (sbyte)M8[PC++];

        Push(PC);

        PC += (ushort)offset;
    }
}
