﻿using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>102B/LBMI/RELATIVE</code>
/// Long Branch If Minus
/// <code>IF (N) then PC’ ← PC + IMM</code>
/// </summary>
/// <remarks>
/// The <c>LBMI</c> tests the <c>CC</c> register:
/// If the Negative (<c>N</c>) flag is set causes a relative branch. 
/// If the <c>N</c> flag is clear the CPU continues executing the next instruction in sequence.
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// None of the Condition Code flags are affected by this instruction.
/// 
/// When used following an operation on signed (twos-complement) binary values, the LBMI instruction will branch if the resulting value is negative. 
/// It is generally preferable to use the LBLT instruction following such an operation because the sign bit may be invalid due to a twos-complement overflow.
/// 
/// The branch address is calculated by adding the current value of the PC register (after the LBMI instruction bytes have been fetched) with the 16-bit twos-complement value contained in the third and fourth bytes of the instruction. 
/// Long branch instructions permit a relative jump to any location within the 64K address space. 
/// The smaller, faster BMI instruction can be used instead when the destination address is within -126 to +129 bytes of the address of the branch instruction.
/// 
/// Cycles (5 (6*))
/// Byte Count (4)
/// *The 6809 requires 6 cycles only if the branch is taken.
/// 
/// See Also: BMI, LBLT, LBPL
internal class _102B_LBmi_R : OpCode, IOpCode
{
    public int CycleCount => 5;

    public void Exec()
    {
        if (CC_N)
        {
            PC += (ushort)(short)M16[PC];

            Cycles += 1;
        }

        PC += 2;
    }
}
