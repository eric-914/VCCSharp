using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>102C/LBGE/RELATIVE</code>
/// Long Branch If Greater than or Equal to Zero
/// <code>IF (N^V) then PC’ ← PC + IMM</code>
/// </summary>
/// <remarks>
/// The <c>LBGE</c> tests the <c>CC</c> register:
/// If the Negative (<c>N</c>) and Overflow (<c>V</c>) flags are both set OR both are clear causes a relative branch. 
/// If the <c>N</c> and <c>V</c> flags do not have the same value then the CPU continues executing the next instruction in sequence.
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// None of the Condition Code flags are affected by this instruction.
/// 
/// When used following a subtract or compare of signed (twos-complement) values, the LBGE instruction will branch if the source value was greater than or equal to the original destination value.
/// 
/// The branch address is calculated by adding the current value of the PC register (after the LBGE instruction bytes have been fetched) with the 16-bit twos-complement value contained in the third and fourth bytes of the instruction. 
/// Long branch instructions permit a relative jump to any location within the 64K address space. 
/// The smaller, faster BGE instruction can be used instead when the destination address is within -126 to +129 bytes of the address of the branch instruction.
/// 
/// Cycles (5 (6*))
/// Byte Count (4)
/// *The 6809 requires 6 cycles only if the branch is taken.
/// 
/// See Also: BGE, LBHS, LBLT
internal class _102C_LBge_R : OpCode, IOpCode
{
    public int CycleCount => 5;

    public int Exec()
    {
        Cycles = CycleCount;

        if (!(CC_N ^ CC_V))
        {
            PC += (ushort)(short)M16[PC];

            Cycles += 1;
        }

        PC += 2;

        return Cycles;
    }
}
