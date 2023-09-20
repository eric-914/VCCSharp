using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>1023/LBLS/RELATIVE</code>
/// Long Branch If Lower or Same
/// <code>IF (C | Z) then PC’ ← PC + IMM</code>
/// </summary>
/// <remarks>
/// The <c>LBLS</c> tests the <c>CC</c> register:
/// If the Zero (<c>Z</c>) and Carry (<c>C</c>) flags are either set causes a relative branch. 
/// If both the <c>Z</c> and <c>C</c> flags are clear then the CPU continues executing the next instruction in sequence. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// None of the Condition Code flags are affected by this instruction.
/// 
/// When used following a subtract or compare of unsigned binary values, the LBLS instruction will branch if the source value was lower than or the same as the original destination value.
/// 
/// LBLS is generally not useful following INC, DEC, LD, ST or TST instructions since none of those affect the Carry flag.
/// 
/// The branch address is calculated by adding the current value of the PC register (after the LBLS instruction bytes have been fetched) with the 16-bit twos-complement value contained in the third and fourth bytes of the instruction. 
/// Long branch instructions permit a relative jump to any location within the 64K address space. 
/// The smaller, faster BLS instruction can be used instead when the destination address is within -126 to +129 bytes of the address of the branch instruction.
/// 
/// Cycles (5 (6*))
/// Byte Count (4)
/// *The 6809 requires 6 cycles only if the branch is taken.
/// 
/// See Also: BLS, LBHI, LBLE
internal class _1023_LBls_R : OpCode, IOpCode
{
    public int CycleCount => 5;

    public int Exec()
    {
        Cycles = CycleCount;

        if (CC_C | CC_Z)
        {
            PC += (ushort)(short)M16[PC];

            Cycles += 1;
        }

        PC += 2;

        return Cycles;
    }
}
