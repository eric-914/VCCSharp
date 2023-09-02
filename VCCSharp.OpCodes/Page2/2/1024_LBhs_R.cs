using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>1024/LBHS/RELATIVE</code>
/// Long Branch If Higher or Same
/// <code>IF (!C) then PC’ ← PC + IMM</code>
/// </summary>
/// <remarks>
/// The <c>LBHS</c> tests the <c>CC</c> register:
/// If the Carry (<c>C</c>) flag is clear causes a relative branch. 
/// If the <c>C</c> is set the CPU continues executing the next instruction in sequence. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// None of the Condition Code flags are affected by this instruction.
/// 
/// When used following a subtract or compare of unsigned binary values, the LBCC instruction will branch if the source value was higher or the same as the original destination value. 
/// For this reason, 6809/6309 assemblers will accept LBHS as an alternate mnemonic for LBCC.
/// 
/// LBCC is generally not useful following INC, DEC, LD, ST or TST instructions since none of those affect the Carry flag. 
/// Also, the LBCC instruction will always branch following a CLR instruction and never branch following a COM instruction due to the way those instructions affect the Carry flag.
/// 
/// The branch address is calculated by adding the current value of the PC register (after the LBCC instruction bytes have been fetched) with the 16-bit twos-complement value contained in the third and fourth bytes of the instruction. 
/// Long branch instructions permit a relative jump to any location within the 64K address space. 
/// The smaller, faster BCC instruction can be used instead when the destination address is within -126 to +129 bytes of the address of the branch instruction.
/// 
/// Cycles (5 (6*))
/// Byte Count (4)
/// *The 6809 requires 6 cycles only if the branch is taken.
/// 
/// See Also: BHS, LBGE, LBLO
/// See Also: BCC, LBCS, LBGE
internal class _1024_LBhs_R : OpCode, IOpCode
{
    public int Exec()
    {
        int cycles = 6;

        if (!CC_C)
        {
            PC += (ushort)(short)M16[PC];

            cycles += 1;
        }

        PC += 2;

        return cycles;
    }
}
