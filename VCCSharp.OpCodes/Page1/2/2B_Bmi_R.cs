using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>2B/BMI/RELATIVE</code>
/// Branch if minus
/// <code>IF CC.N ≠ 0 then PC’ ← PC + IMM</code>
/// </summary>
/// <remarks>
/// This instruction tests the Negative (N) flag in the CC register and, if it is set (1), causes a relative branch. 
/// If the N flag is 0, the CPU continues executing the next instruction in sequence. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// None of the Condition Code flags are affected by this instruction.
/// 
/// When used following an operation on signed (twos-complement) binary values, the BMI instruction will branch if the resulting value is negative. 
/// It is generally preferable to use the BLT instruction following such an operation because the sign bit may be invalid due to a twos-complement overflow.
/// 
/// The branch address is calculated by adding the current value of the PC register (after the BMI instruction bytes have been fetched) with the 8-bit twos-complement value contained in the second byte of the instruction. 
/// The range of the branch destination is limited to -126 to +129 bytes from the address of the BMI instruction. 
/// If a larger range is required then the LBMI instruction may be used instead.
/// 
/// Cycles (3)
/// Byte Count (2)
/// 
/// See Also: BLT, BPL, LBMI
internal class _2B_Bmi_R : OpCode, IOpCode
{
    public int Exec()
    {
        if (CC_N)
        {
            PC += (ushort)(sbyte)M8[PC];
        }

        PC++;

        return 3;
    }
}
