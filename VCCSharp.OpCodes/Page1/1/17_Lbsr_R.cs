using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>170/LBSR/RELATIVE</code>
/// Long Branch to Subroutine
/// <code>
///      S’ ← S - 2
/// (S:S+1) ← PC
///     PC’ ← PC + IMM
/// </code>
/// </summary>
/// <remarks>
/// This instruction pushes the value of the PC register (after the LBSR instruction bytes have been fetched) onto the hardware stack and then performs an unconditional relative branch. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// None of the Condition Code flags are affected.
/// 
/// By pushing the PC value onto the stack, the called subroutine can "return" to this address after it has completed.
/// 
/// The LBSR instruction is similar in function to the JSR instruction. 
/// The primary difference is that LBSR uses the Relative Addressing mode which allows the code to be position-independent.
/// 
/// The branch address is calculated by adding the current value of the PC register (after the LBSR instruction bytes have been fetched) with the 16-bit twos-complement value contained in the second and third bytes of the instruction. 
/// Long branch instructions permit a relative jump to any location within the 64K address space. 
/// The smaller, faster BSR instruction can be used instead when the destination address is within -126 to +129 bytes of the address of the branch instruction.
/// 
/// Cycles (9 / 7)
/// Byte Count (3)
/// 
/// See Also: BSR, JSR, PULS, RTS
internal class _17_Lbsr_R : OpCode, IOpCode
{
    internal _17_Lbsr_R(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        ushort word = M16[PC];

        PC += 2;

        M8[--S] = PC_L;
        M8[--S] = PC_H;

        PC += word;

        return Cycles._97;
    }
}
