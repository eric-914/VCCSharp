using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>22/BHI/RELATIVE</code>
/// Branch If Higher (unsigned)
/// <code>IF (CC.Z = 0) AND (CC.C = 0) then PC’ ← PC + IMM</code>
/// </summary>
/// <remarks>
/// This instruction tests the Zero (Z) and Carry (C) flags in the CC register and, if both are zero, causes a relative branch. 
/// If either the Z or C flags are set then the CPU continues executing the next instruction in sequence. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
///   
/// None of the Condition Code flags are affected by this instruction.
/// 
/// When used following a subtract or compare of unsigned binary values, the BHI instruction will branch if the source value was higher than the original destination value.
/// 
/// BHI is generally not useful following INC, DEC, LD, ST or TST instructions since none of those affect the Carry flag.
/// 
/// The branch address is calculated by adding the current value of the PC register (after the BHI instruction bytes have been fetched) with the 8-bit twos-complement value contained in the second byte of the instruction. 
/// The range of the branch destination is limited to -126 to +129 bytes from the address of the BHI instruction. If a larger range is required then the LBHI instruction may be used instead.
/// 
/// Cycles (3)
/// Byte Count (2)
/// 
/// See Also: BGT, BLS, LBHI
internal class _22_Bhi_R : OpCode, IOpCode
{
    internal _22_Bhi_R(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        if (!(CC_C || CC_Z))
        {
            PC += (ushort)(sbyte)M8[PC];
        }

        PC++;

        return 3;
    }
}
