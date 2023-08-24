using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>2D/BLT/RELATIVE</code>
/// Branch If Less Than Zero (signed)
/// <code>IF CC.N ≠ CC.V then PC’ ← PC + IMM</code>
/// </summary>
/// This instruction performs a relative branch if the values of the Negative (N) and Overflow (V) flags are not equal. 
/// If the N and V flags have the same value then the CPU continues executing the next instruction in sequence. 
/// <remarks>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// None of the Condition Code flags are affected by this instruction.
/// 
/// When used following a subtract or compare of signed (twos-complement) values, the BLT instruction will branch if the source value was less than the original destination value.
/// 
/// The branch address is calculated by adding the current value of the PC register (after the BLT instruction bytes have been fetched) with the 8-bit twos-complement value contained in the second byte of the instruction. 
/// The range of the branch destination is limited to -126 to +129 bytes from the address of the BLT instruction. 
/// If a larger range is required then the LBLT instruction may be used instead.
/// 
/// Cycles (3)
/// Byte Count (2)
/// 
/// See Also: BGE, BLO, LBLT
internal class _2D_Blt_R : OpCode, IOpCode
{
    internal _2D_Blt_R(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        if (CC_V ^ CC_N)
        {
            PC += (ushort)(sbyte)M8[PC];
        }

        PC++;

        return 3;
    }
}
