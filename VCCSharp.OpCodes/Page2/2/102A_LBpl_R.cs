using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>102A/LBPL/RELATIVE</code>
/// Long Branch If Plus
/// <code>IF (!N) then PC’ ← PC + IMM</code>
/// </summary>
/// <remarks>
/// The <c>LBPL</c> tests the <c>CC</c> register:
/// If the Negative (<c>N</c>) flag is clear causes a relative branch. 
/// If the <c>N</c> flag is set the CPU continues executing the next instruction in sequence. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// None of the Condition Code flags are affected by this instruction. 
/// 
/// When used following an operation on signed (twos-complement) binary values, the LBPL instruction will branch if the resulting value is positive. 
/// It is generally preferable to use the LBGE instruction following such an operation because the sign bit may be invalid due to a twos-complement overflow.
/// 
/// The branch address is calculated by adding the current value of the PC register (after the LBPL instruction bytes have been fetched) with the 16-bit twos-complement value contained in the third and fourth bytes of the instruction. 
/// Long branch instructions permit a relative jump to any location within the 64K address space. 
/// The smaller, faster BPL instruction can be used instead when the destination address is within -126 to +129 bytes of the address of the branch instruction.
/// 
/// Cycles (5 (6*))
/// Byte Count (4)
/// *The 6809 requires 6 cycles only if the branch is taken.
/// 
/// See Also: BPL, LBGE, LBMI
internal class _102A_LBpl_R : OpCode, IOpCode
{
    public int Exec()
    {
        int cycles = 5;

        if (!CC_N)
        {
            PC += (ushort)(short)M16[PC];

            cycles += 1;
        }

        PC += 2;

        return cycles;
    }
}
