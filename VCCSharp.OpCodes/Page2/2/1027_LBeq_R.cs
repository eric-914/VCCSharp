using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>1027/LBEQ/RELATIVE</code>
/// Long Branch If Equal to Zero
/// <code>IF (Z) then PC’ ← PC + IMM</code>
/// </summary>
/// <remarks>
/// The <c>LBEQ</c> tests the <c>CC</c> register:
/// If the Zero (<c>Z</c>) flag is set causes a relative branch. 
/// If the <c>Z</c> flag is clear the CPU continues executing the next instruction in sequence.
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// None of the Condition Code flags are affected by this instruction.
/// 
/// When used following almost any instruction that produces, tests or moves a value, the LBEQ instruction will branch if that value is equal to zero. 
/// In the case of an instruction that performs a subtract or compare, the LBEQ instruction will branch if the source value was equal to the original destination value.
/// 
/// LBEQ is generally not useful following a CLR instruction since the Z flag is always set.
/// 
/// The following instructions produce or move values, but do not affect the Z flag:
///         ABX   BAND  BEOR  BIAND  BIEOR
///         BOR   BIOR  EXG   LDBT   LDMD
///         LEAS  LEAU  PSH   PUL    STBT
///         TFR
///         
/// The branch address is calculated by adding the current value of the PC register (after the LBEQ instruction bytes have been fetched) with the 16-bit twos-complement value contained in the third and fourth bytes of the instruction. 
/// Long branch instructions permit a relative jump to any location within the 64K address space. 
/// The smaller, faster BEQ instruction can be used instead when the destination address is within -126 to +129 bytes of the address of the branch instruction.
/// 
/// Cycles (5 (6*))
/// Byte Count (4)
/// *The 6809 requires 6 cycles only if the branch is taken.
/// 
/// See Also: BEQ, LBNE
internal class _1027_LBeq_R : OpCode, IOpCode
{
    public int CycleCount => 5;

    public int Exec()
    {
        Cycles = CycleCount;

        if (CC_Z)
        {
            PC += (ushort)(short)M16[PC];

            Cycles += 1;
        }

        PC += 2;

        return Cycles;
    }
}
