using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>2C/BGE/RELATIVE</code>
/// Branch If Greater than or Equal to Zero (signed)
/// <code>IF CC.N = CC.V then PC’ ← PC + IMM</code>
/// </summary>
/// <remarks>
/// This instruction tests the Negative (N) and Overflow (V) flags in the CC register and, if both are set OR both are clear, causes a relative branch. 
/// If the N and V flags do not have the same value then the CPU continues executing the next instruction in sequence. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// None of the Condition Code flags are affected by this instruction.
/// 
/// When used following a subtract or compare of signed (twos-complement) values, the BGE instruction will branch if the source value was greater than or equal to the original destination value.
/// 
/// The branch address is calculated by adding the current value of the PC register (after the BGE instruction bytes have been fetched) with the 8-bit twos-complement value contained in the second byte of the instruction. 
/// The range of the branch destination is limited to -126 to +129 bytes from the address of the BGE instruction. If a larger range is required then the LBGE instruction may be used instead.
/// 
/// Cycles (3)
/// Byte Count (2)
/// 
/// See Also: BHS, BLT, LBGE
internal class _2C_Bge_R : OpCode, IOpCode
{
    public int Exec()
    {
        if (!(CC_N ^ CC_V))
        {
            PC += (ushort)(sbyte)M8[PC];
        }

        PC++;

        return 3;
    }
}
