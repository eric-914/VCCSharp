﻿using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>23/BLS/RELATIVE</code>
/// Branch if lower or same (unsigned)
/// <code>IF (CC.Z ≠ 0) OR (CC.C ≠ 0) then PC’ ← PC + IMM</code>
/// </summary>
/// <remarks>
/// This instruction tests the Zero (Z) and Carry (C) flags in the CC register and, if either are set, causes a relative branch. 
/// If both the Z and C flags are clear then the CPU continues executing the next instruction in sequence. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// None of the Condition Code flags are affected by this instruction.
/// 
/// When used following a subtract or compare of unsigned binary values, the BLS instruction will branch if the source value was lower than or the same as the original destination value.
/// 
/// BLS is generally not useful following INC, DEC, LD, ST or TST instructions since none of those affect the Carry flag.
/// 
/// The branch address is calculated by adding the current value of the PC register (after the BLS instruction bytes have been fetched) with the 8-bit twos-complement value contained in the second byte of the instruction. 
/// The range of the branch destination is limited to -126 to +129 bytes from the address of the BLS instruction. 
/// If a larger range is required then the LBLS instruction may be used instead.
/// 
/// Cycles (3)
/// Byte Count (2)
/// 
/// See Also: BHI, BLE, LBLS
internal class _23_Bls_R : OpCode, IOpCode
{
    public int CycleCount => 3;

    public void Exec()
    {
        if (CC_C | CC_Z)
        {
            PC += (ushort)(sbyte)M8[PC];
        }

        PC++;
    }
}
