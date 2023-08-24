using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>20/BRA/RELATIVE</code>
/// Branch always
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
/// The BRA instruction is similar in function to the JMP instruction in that it always causes execution to be transferred to the effective address specified by the operand. 
/// The primary difference is that BRA uses the Relative Addressing mode which allows the code to be position-independent.
/// 
/// The branch address is calculated by adding the current value of the PC register (after the BRA instruction bytes have been fetched) with the 8-bit twos-complement value contained in the second byte of the instruction. 
/// The range of the branch destination is limited to -126 to +129 bytes from the address of the BPL instruction. 
/// If a larger range is required then the LBRA instruction may be used instead.
/// 
/// Cycles (3)
/// Byte Count (2)
/// 
/// See Also: BRN, JMP, LBRA
internal class _20_Bra_R : OpCode, IOpCode
{
    internal _20_Bra_R(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        PC += (ushort)(sbyte)M8[PC++];

        return 3;
    }
}
