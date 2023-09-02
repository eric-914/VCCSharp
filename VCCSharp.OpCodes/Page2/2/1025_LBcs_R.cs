using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>1025/LBCS/RELATIVE</code>
/// Long Branch If Carry Set
/// <code>IF (C) then PC’ ← PC + IMM</code>
/// </summary>
/// <remarks>
/// The <c>LBCS</c> the <c>CC</c> register:
/// If the Carry (<c>C</c>) flag is set causes a relative branch. 
/// If the <c>C</c> flag is clear the CPU continues executing the next instruction in sequence. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// None of the Condition Code flags are affected by this instruction.
/// 
/// When used following a subtract or compare of unsigned binary values, the LBCS instruction will branch if the source value was lower than the original destination value.
/// For this reason, 6809/6309 assemblers will accept LBLO as an alternate mnemonic for LBCS.
/// 
/// LBCS is generally not useful following INC, DEC, LD, ST or TST instructions since none of those affect the Carry flag. 
/// Also, the LBCS instruction will never branch following a CLR instruction and always branch following a COM instruction due to the way those instructions affect the Carry flag.
/// 
/// The branch address is calculated by adding the current value of the PC register (after the LBCS instruction bytes have been fetched) with the 16-bit twos-complement value contained in the third and fourth bytes of the instruction. 
/// Long branch instructions permit a relative jump to any location within the 64K address space. 
/// The smaller, faster BCS instruction can be used instead when the destination address is within -126 to +129 bytes of the address of the branch instruction.
/// 
/// Cycles (5 (6*))
/// Byte Count (4)
/// *The 6809 requires 6 cycles only if the branch is taken.
/// 
/// See Also: BCS, LBCC, LBLT
/// See Also: BLO, LBHS, LBLT
internal class _1025_LBcs_R : OpCode, IOpCode
{
    public int Exec()
    {
        int cycles = 5;

        if (CC_C)
        {
            PC += (ushort)(short)M16[PC];

            cycles += 1;
        }

        PC += 2;

        return cycles;
    }
}
