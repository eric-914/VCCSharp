﻿using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>1022/LBHI/RELATIVE</code>
/// Long Branch If Higher
/// <code>IF !(C|Z) then PC’ ← PC + IMM</code>
/// </summary>
/// <remarks>
/// The <c>LBHI</c> tests the <c>CC</c> register:
/// If the Zero (<c>Z</c>) and Carry (<c>C</c>) flags are both clear causes a relative branch. 
/// If either the <c>Z</c> or <c>C</c> flags are set then the CPU continues executing the next instruction in sequence. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// None of the Condition Code flags are affected by this instruction.
/// 
/// When used following a subtract or compare of unsigned binary values, the LBHI instruction will branch if the source value was higher than the original destination value.
/// 
/// LBHI is generally not useful following INC, DEC, LD, ST or TST instructions since none of those affect the Carry flag.
/// 
/// The branch address is calculated by adding the current value of the PC register (after the LBHI instruction bytes have been fetched) with the 16-bit twos-complement value contained in the third and fourth bytes of the instruction. 
/// Long branch instructions permit a relative jump to any location within the 64K address space. 
/// The smaller, faster BHI instruction can be used instead when the destination address is within -126 to +129 bytes of the address of the branch instruction.
/// 
/// Cycles (5 (6*))
/// Byte Count (4)
/// *The 6809 requires 6 cycles only if the branch is taken.
/// 
/// See Also: BHI, LBGT, LBLS
internal class _1022_LBhi_R : OpCode, IOpCode
{
    public int CycleCount => 5;

    public void Exec()
    {
        if (!(CC_C | CC_Z))
        {
            PC += (ushort)(short)M16[PC];

            Cycles += 1;
        }

        PC += 2;
    }
}
