using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>102D/LBLT/RELATIVE</code>
/// Long Branch If Less Than Zero
/// <code>IF (V^N) then PC’ ← PC + IMM</code>
/// </summary>
/// <remarks>
/// The <c>LBLT</c> tests the <c>CC</c> register:
/// If Negative (<c>N</c>) and Overflow (<c>V</c>) flags are not equal causes a relative branch. 
/// If the <c>N</c> and <c>V</c> flags have the same value then the CPU continues executing the next instruction in sequence. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// None of the Condition Code flags are affected by this instruction.
/// 
/// When used following a subtract or compare of signed (twos-complement) values, the LBLT instruction will branch if the source value was less than the original destination value.
/// 
/// The branch address is calculated by adding the current value of the PC register (after the LBLT instruction bytes have been fetched) with the 16-bit twos-complement value contained in the third and fourth bytes of the instruction. 
/// Long branch instructions permit a relative jump to any location within the 64K address space. 
/// The smaller, faster BLT instruction can be used instead when the destination address is within -126 to +129 bytes of the address of the branch instruction.
/// 
/// Cycles (5 (6*))
/// Byte Count (4)
/// *The 6809 requires 6 cycles only if the branch is taken.
/// 
/// See Also: BLT, LBGE, LBLO
internal class _102D_LBlt_R : OpCode, IOpCode
{
    internal _102D_LBlt_R(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        int cycles = 5;

        if (CC_V ^ CC_N)
        {
            PC += (ushort)(short)M16[PC];

            cycles += 1;
        }

        PC += 2;

        return cycles;
    }
}
