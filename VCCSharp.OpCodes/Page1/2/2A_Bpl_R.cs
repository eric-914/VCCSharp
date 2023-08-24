using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>2A/BPL/RELATIVE</code>
/// Branch if plus
/// <code>IF CC.N = 0 then PC’ ← PC + IMM</code>
/// </summary>
/// <remarks>
/// This instruction tests the Negative (N) flag in the CC register and, if it is clear (0), causes a relative branch. 
/// If the N flag is set, the CPU continues executing the next instruction in sequence. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// None of the Condition Code flags are affected by this instruction.
/// 
/// When used following an operation on signed (twos-complement) binary values, the BPL instruction will branch if the resulting value is positive. 
/// It is generally preferable to use the BGE instruction following such an operation because the sign bit may be invalid due to a twos-complement overflow.
/// 
/// The branch address is calculated by adding the current value of the PC register (after the BPL instruction bytes have been fetched) with the 8-bit twos-complement value contained in the second byte of the instruction. 
/// The range of the branch destination is limited to -126 to +129 bytes from the address of the BPL instruction. 
/// If a larger range is required then the LBPL instruction may be used instead.
/// 
/// Cycles (3)
/// Byte Count (2)
/// 
/// See Also: BGE, BMI, LBPL
internal class _2A_Bpl_R : OpCode, IOpCode
{
    internal _2A_Bpl_R(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        if (!CC_N)
        {
            PC += (ushort)(sbyte)M8[PC];
        }

        PC++;

        return 3;
    }
}
