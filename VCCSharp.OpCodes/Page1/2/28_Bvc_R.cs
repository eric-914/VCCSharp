using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>28/BVC/RELATIVE</code>
/// Branch if overflow clear (valid 2's complement result)
/// <code>IF CC.V = 0 then PC’ ← PC + IMM</code>
/// </summary>
/// <remarks>
/// This instruction tests the Overflow (V) flag in the CC register and, if it is clear (0), causes a relative branch. 
/// If the V flag is set, the CPU continues executing the next instruction in sequence. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// None of the Condition Code flags are affected by this instruction.
/// 
/// When used following an operation on signed (twos-complement) binary values, the BVC instruction will branch if there was no overflow.
/// 
/// The branch address is calculated by adding the current value of the PC register (after the BVC instruction bytes have been fetched) with the 8-bit twos-complement value contained in the second byte of the instruction. 
/// The range of the branch destination is limited to -126 to +129 bytes from the address of the BVC instruction. 
/// If a larger range is required then the LBVC instruction may be used instead.
/// 
/// Cycles (3)
/// Byte Count (2)
/// 
/// See Also: BVS, LBVC
internal class _28_Bvc_R : OpCode, IOpCode
{
    public int CycleCount => 3;

    public int Exec()
    {
        if (!CC_V)
        {
            PC += (ushort)(sbyte)M8[PC];
        }

        PC++;

        return CycleCount;
    }
}
