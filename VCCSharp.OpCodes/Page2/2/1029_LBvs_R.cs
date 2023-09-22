using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>1029/LBVS/RELATIVE</code>
/// Long Branch If Overflow Set
/// <code>IF (V) then PC’ ← PC + IMM</code>
/// </summary>
/// <remarks>
/// The <c>LBVS</c> tests the <c>CC</c> register :
/// If the Overflow (<c>V</c>) flag is set causes a relative branch. 
/// If the <c>V</c> flag is clear the CPU continues executing the next instruction in sequence. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// None of the Condition Code flags are affected by this instruction.
/// 
/// When used following an operation on signed (twos-complement) binary values, the LBVS instruction will branch if an overflow occurred.
/// 
/// The branch address is calculated by adding the current value of the PC register (after the LBVS instruction bytes have been fetched) with the 16-bit twos-complement value contained in the third and fourth bytes of the instruction. 
/// Long branch instructions permit a relative jump to any location within the 64K address space. 
/// The smaller, faster BVS instruction can be used instead when the destination address is within -126 to +129 bytes of the address of the branch instruction.
/// 
/// Cycles (5 (6*))
/// Byte Count (4)
/// *The 6809 requires 6 cycles only if the branch is taken.
/// 
/// See Also: BVS, LBVC
internal class _1029_LBvs_R : OpCode, IOpCode
{
    public int CycleCount => 5;

    public void Exec()
    {
        if (CC_V)
        {
            PC += (ushort)(short)M16[PC];

            Cycles += 1;
        }

        PC += 2;
    }
}
