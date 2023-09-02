using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>16/LBRA/RELATIVE</code>
/// Long Branch Always
/// <code>PC’ ← PC + IMM</code>
/// </summary>
/// <remarks>
/// This instruction causes an unconditional relative branch. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// None of the Condition Code flags are affected.
/// 
/// The LBRA instruction is similar in function to the JMP instruction in that it always causes execution to be transferred to the effective address specified by the operand. 
/// The primary difference is that LBRA uses the Relative Addressing mode which allows the code to be position-independent.
/// 
/// The branch address is calculated by adding the current value of the PC register (after the LBRA instruction bytes have been fetched) with the 16-bit twos-complement value contained in the second and third bytes of the instruction. 
/// Long branch instructions permit a relative jump to any location within the 64K address space. 
/// The smaller, faster BRA instruction can be used when the destination address is within -126 to +129 bytes of the address of the branch instruction.
/// 
/// Cycles (5 / 4)
/// Byte Count (3)
/// 
/// See Also: BRA, LBRN, JMP
internal class _16_Lbra_R : OpCode, IOpCode
{
    internal _16_Lbra_R(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        ushort word = M16[PC];

        PC += 2;

        PC += word;

        return DynamicCycles._54;
    }
}
