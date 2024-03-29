﻿using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>25/BCS/RELATIVE</code>
/// Branch if carry set 
/// <code>25/BLO/RELATIVE</code>
/// Branch if lower (unsigned)
/// <code>IF CC.C ≠ 0 then PC’ ← PC + IMM</code>
/// </summary>
/// <remarks>
/// This instruction tests the Carry flag in the CC register and, if it is set (1), causes a relative branch. 
/// If the Carry flag is 0, the CPU continues executing the next instruction in sequence. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// None of the Condition Code flags are affected by this instruction.
/// 
/// When used following a subtract or compare of unsigned binary values, the BCS instruction will branch if the source value was lower than the original destination value.
/// For this reason, 6809/6309 assemblers will accept BLO as an alternate mnemonic for BCS.
/// 
/// BCS is generally not useful following INC, DEC, LD, ST or TST instructions since none of those affect the Carry flag. 
/// BCS will never branch following a CLR instruction and will always branch following a COM instruction due to the way those instructions affect the Carry flag.
/// 
/// The branch address is calculated by adding the current value of the PC register (after the BCS instruction bytes have been fetched) with the 8-bit twos-complement value contained in the second byte of the instruction. 
/// The range of the branch destination is limited to -126 to +129 bytes from the address of the BCS instruction. 
/// If a larger range is required then the LBCS instruction may be used instead.
/// 
/// Cycles (3)
/// Byte Count (2)
/// 
/// See Also: BCC, BLT, LBCS
/// See Also: BHS, BLT, LBLO
internal class _25_Bcs_R : OpCode, IOpCode
{
    public int CycleCount => 3;

    public void Exec()
    {
        if (CC_C)
        {
            PC += (ushort)(sbyte)M8[PC];
        }

        PC++;
    }
}
