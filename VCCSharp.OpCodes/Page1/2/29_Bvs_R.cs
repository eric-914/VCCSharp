using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>29/BVS/RELATIVE</code>
/// Branch if overflow set (invalid 2's complement result)
/// <code>IF CC.V ≠ 0 then PC’ ← PC + IMM</code>
/// </summary>
/// <remarks>
/// This instruction tests the Overflow (V) flag in the CC register and, if it is set (1), causes a relative branch. 
/// If the V flag is clear, the CPU continues executing the next instruction in sequence. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// None of the Condition Code flags are affected by this instruction.
/// 
/// When used following an operation on signed (twos-complement) binary values, the BVS instruction will branch if an overflow occurred.
/// 
/// The branch address is calculated by adding the current value of the PC register (after the BVS instruction bytes have been fetched) with the 8-bit twos-complement value contained in the second byte of the instruction. 
/// The range of the branch destination is limited to -126 to +129 bytes from the address of the BVS instruction. 
/// If a larger range is required then the LBVS instruction may be used instead.
/// 
/// Cycles (3)
/// Byte Count (2)
/// 
/// See Also: BVC, LBVS
internal class _29_Bvs_R : OpCode, IOpCode
{
    internal _29_Bvs_R(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        if (CC_V)
        {
            PC += (ushort)(sbyte)M8[PC];
        }

        PC++;

        return 3;
    }
}
