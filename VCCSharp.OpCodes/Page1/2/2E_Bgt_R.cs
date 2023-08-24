using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>2E/BGT/RELATIVE</code>
/// Branch If Greater Than Zero (signed)
/// <code>IF (CC.N = CC.V) AND (CC.Z = 0) then PC’ ← PC + IMM</code>
/// </summary>
/// <remarks>
/// This instruction tests the Zero (Z) flag in the CC register and, if it is clear AND the values of the Negative (N) and Overflow (V) flags are equal (both set OR both clear), causes a relative branch. 
/// If the N and V flags do not have the same value or if the Z flag is set then the CPU continues executing the next instruction in sequence. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
///   
/// None of the Condition Code flags are affected by this instruction.
/// 
/// When used following a subtract or compare of signed (twos-complement) values, the BGT instruction will branch if the source value was greater than the original destination value.
/// 
/// The branch address is calculated by adding the current value of the PC register (after the BGT instruction bytes have been fetched) with the 8-bit twos-complement value contained in the second byte of the instruction. 
/// The range of the branch destination is limited to -126 to +129 bytes from the address of the BGT instruction. 
/// If a larger range is required then the LBGT instruction may be used instead.
/// 
/// Cycles (3)
/// Byte Count (2)
/// 
/// See Also: BHI, BLE, LBGT
internal class _2E_Bgt_R : OpCode, IOpCode
{
    internal _2E_Bgt_R(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        if (!(CC_Z | (CC_N ^ CC_V)))
        {
            PC += (ushort)(sbyte)M8[PC];
        }

        PC++;

        return 3;
    }
}
