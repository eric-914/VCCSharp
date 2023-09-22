using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>1028/LBVC/RELATIVE</code>
/// Long Branch If Overflow Clear
/// <code>IF (!V) then PC’ ← PC + IMM</code>
/// </summary>
/// <remarks>
/// The <c>LBVC</c> tests the <c>CC</c> register:
/// If the Overflow (<c>V</c>) flag is clear causes a relative branch. 
/// If the <c>V</c> flag is set the CPU continues executing the next instruction in sequence. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// None of the Condition Code flags are affected by this instruction.
/// 
/// When used following an operation on signed (twos-complement) binary values, the LBVC instruction will branch if there was no overflow.
/// 
/// The branch address is calculated by adding the current value of the PC register (after the LBVC instruction bytes have been fetched) with the 16-bit twos-complement value contained in the third and fourth bytes of the instruction. 
/// Long branch instructions permit a relative jump to any location within the 64K address space. 
/// The smaller, faster BVC instruction can be used instead when the destination address is within -126 to +129 bytes of the address of the branch instruction.
/// 
/// Cycles (5 (6*))
/// Byte Count (4)
/// *The 6809 requires 6 cycles only if the branch is taken.
/// 
/// See Also: BVC, LBVS
internal class _1028_LBvc_R : OpCode, IOpCode
{
    public int CycleCount => 5;

    public void Exec()
    {
        if (!CC_V)
        {
            PC += (ushort)(short)M16[PC];

            Cycles += 1;
        }

        PC += 2;
    }
}
