﻿using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>26/BNE/RELATIVE</code>
/// Branch If Not Equal to Zero
/// <code>IF CC.Z = 0 then PC’ ← PC + IMM</code>
/// </summary>
/// <remarks>
/// This instruction tests the Zero flag in the CC register and, if it is clear (0), causes a relative branch. 
/// If the Z flag is set, the CPU continues executing the next instruction in sequence. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// None of the Condition Code flags are affected by this instruction.
/// 
/// When used following almost any instruction that produces, tests or moves a value, the BNE instruction will branch if that value is not equal to zero. 
/// In the case of an instruction that performs a subtract or compare, the BNE instruction will branch if the source value was not equal to the original destination value.
/// 
/// BNE is generally not useful following a CLR instruction since the Z flag is always set.
/// 
/// The following instructions produce or move values, but do not affect the Z flag:
///         ABX   BAND  BEOR  BIAND BIEOR
///         BOR   BIOR  EXG   LDBT  LDMD
///         LEAS  LEAU  PSH   PUL   STBT
///         TFM   TFR
///         
/// The branch address is calculated by adding the current value of the PC register (after the BNE instruction bytes have been fetched) with the 8-bit twos-complement value contained in the second byte of the instruction. 
/// The range of the branch destination is limited to -126 to +129 bytes from the address of the BNE instruction. If a larger range is required then the LBNE instruction may be used instead.
/// 
/// Cycles (3)
/// Byte Count (2)
/// 
/// See Also: BEQ, LBNE
internal class _26_Bne_R : OpCode, IOpCode
{
    public int CycleCount => 3;

    public void Exec()
    {
        if (!CC_Z)
        {
            PC += (ushort)(sbyte)M8[PC];
        }

        PC++;
    }
}
