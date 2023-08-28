using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>102F/LBLE/RELATIVE</code>
/// Long Branch If Less than or Equal to Zero
/// <code>IF C|(N^V) then PC’ ← PC + IMM</code>
/// </summary>
/// <remarks>
/// The <c>LBLE</c> tests the <c>CC</c> register:
/// If the Zero (<c>Z</c>) flag is set OR if the values of the Negative (<c>N</c>) and Overflow (<c>V</c>) flags are not equal causes a relative branch.
/// If the <c>N</c> and <c>V</c> flags have the same value and the <c>Z</c> flag is clear then the CPU continues executing the next instruction in sequence. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// None of the Condition Code flags are affected by this instruction.
/// 
/// When used following a subtract or compare of signed (twos-complement) values, the LBLE instruction will branch if the source value was less than or equal to the original destination value.
/// 
/// The branch address is calculated by adding the current value of the PC register (after the LBLE instruction bytes have been fetched) with the 16-bit twos-complement value contained in the third and fourth bytes of the instruction. 
/// Long branch instructions permit a relative jump to any location within the 64K address space. 
/// The smaller, faster BLE instruction can be used instead when the destination address is within -126 to +129 bytes of the address of the branch instruction.
/// 
/// Cycles (5 (6*))
/// Byte Count (4)
/// *The 6809 requires 6 cycles only if the branch is taken.
/// 
/// See Also: BLE, LBGT, LBLS
internal class _102F_LBle_R : OpCode, IOpCode
{
    internal _102F_LBle_R(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        int cycles = 5;

        if (CC_Z | (CC_N ^ CC_V))
        {
            PC += (ushort)(short)M16[PC];

            cycles += 1;
        }

        PC += 2;

        return cycles;
    }
}
