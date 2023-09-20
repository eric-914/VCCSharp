using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>102E/LBGT/RELATIVE</code>
/// Long Branch If Greater Than Zero
/// <code>IF !(C|(N^V)) then PC’ ← PC + IMM</code>
/// </summary>
/// <remarks>
/// The <c>LBGT</c> tests the <c>CC</c>:
/// If the Zero (<c>Z</c>) flag in is clear AND the values of the Negative (<c>N</c>) and Overflow (<c>V</c>) flags are equal (both set OR both clear) causes a relative branch. 
/// If the <c>N</c> and <c>V</c> flags do not have the same value or if the <c>Z</c> flag is set then the CPU continues executing the next instruction in sequence. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// None of the Condition Code flags are affected by this instruction.
/// 
/// When used following a subtract or compare of signed (twos-complement) values, the LBGT instruction will branch if the source value was greater than the original destination value.
/// 
/// The branch address is calculated by adding the current value of the PC register (after the LBGT instruction bytes have been fetched) with the 16-bit twos-complement value contained in the third and fourth bytes of the instruction. 
/// Long branch instructions permit a relative jump to any location within the 64K address space. 
/// The smaller, faster BGT instruction can be used instead when the destination address is within -126 to +129 bytes of the address of the branch instruction.
/// 
/// Cycles (5 (6*))
/// Byte Count (4)
/// *The 6809 requires 6 cycles only if the branch is taken.
/// 
/// See Also: BGT, LBHI, LBLE
internal class _102E_LBgt_R : OpCode, IOpCode
{
    public int CycleCount => 5;

    public int Exec()
    {
        Cycles = CycleCount;

        if (!(CC_Z | (CC_N ^ CC_V)))
        {
            PC += (ushort)(short)M16[PC];

            Cycles += 1;
        }

        PC += 2;

        return Cycles;
    }
}
