﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// BHI
    /// Branch if higher (unsigned)
    /// Branch If Higher
    /// RELATIVE
    /// IF (CC.Z = 0) AND (CC.C = 0) then PC’ ← PC + IMM
    /// SOURCE FORM   ADDRESSING MODE     OPCODE       CYCLES      BYTE COUNT
    /// BHI address   RELATIVE            22           3           2
    ///   [E F H I N Z V C]
    ///   [               ]
    /// </summary>
    /// <remarks>
    /// This instruction tests the Zero (Z) and Carry (C) flags in the CC register and, if both are zero, causes a relative branch. 
    /// If either the Z or C flags are set then the CPU continues executing the next instruction in sequence. 
    /// None of the Condition Code flags are affected by this instruction.
    /// 
    /// When used following a subtract or compare of unsigned binary values, the BHI instruction will branch if the source value was higher than the original destination value.
    /// 
    /// BHI is generally not useful following INC, DEC, LD, ST or TST instructions since none of those affect the Carry flag.
    /// 
    /// The branch address is calculated by adding the current value of the PC register (after the BHI instruction bytes have been fetched) with the 8-bit twos-complement value contained in the second byte of the instruction. 
    /// The range of the branch destination is limited to -126 to +129 bytes from the address of the BHI instruction. If a larger range is required then the LBHI instruction may be used instead.
    /// 
    /// See Also: BGT, BLS, LBHI
    /// </remarks>
    public class _22_Bhi_R : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            if (!(cpu.CC_C || cpu.CC_Z))
            {
                cpu.PC_REG += (ushort)(sbyte)cpu.MemRead8(cpu.PC_REG);
            }

            cpu.PC_REG++;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 3);
        public int Exec(IHD6309 cpu) => Exec(cpu, 3);
    }
}
