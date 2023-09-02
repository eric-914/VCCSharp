using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>1026/LBNE/RELATIVE</code>
/// Long Branch If Not Equal to Zero
/// <code>IF (!Z) then PC’ ← PC + IMM</code>
/// </summary>
/// <remarks>
/// The <c>LBNE</c> tests <c>CC</c> register:
/// If the Zero (<c>Z</c>)flag is clear causes a relative branch. 
/// If the <c>Z</c> flag is set the CPU continues executing the next instruction in sequence. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// None of the Condition Code flags are affected by this instruction.
/// 
/// When used following almost any instruction that produces, tests or moves a value, the LBNE instruction will branch if that value is not equal to zero. 
/// In the case of an instruction that performs a subtract or compare, the LBNE instruction will branch if the source value was not equal to the original destination value.
/// 
/// LBNE is generally not useful following a CLR instruction since the Z flag is always set.
/// 
/// The following instructions produce or move values, but do not affect the Z flag:
///         ABX   BAND  BEOR  BIAND  BIEOR
///         BOR   BIOR  EXG   LDBT   LDMD
///         LEAS  LEAU  PSH   PUL    STBT
///         TFM   TFR
///         
/// The branch address is calculated by adding the current value of the PC register (after the LBNE instruction bytes have been fetched) with the 16-bit twos-complement value contained in the third and fourth bytes of the instruction. 
/// Long branch instructions permit a relative jump to any location within the 64K address space. 
/// The smaller, faster BNE instruction can be used instead when the destination address is within -126 to +129 bytes of the address of the branch instruction.
/// 
/// Cycles (5 (6*))
/// Byte Count (4)
/// *The 6809 requires 6 cycles only if the branch is taken.
/// 
/// See Also: BNE, LBEQ
internal class _1026_LBne_R : OpCode, IOpCode
{
    public int Exec()
    {
        int cycles = 5;

        if (!CC_Z)
        {
            PC += (ushort)(short)M16[PC];

            cycles += 1;
        }

        PC += 2;

        return cycles;
    }
}
