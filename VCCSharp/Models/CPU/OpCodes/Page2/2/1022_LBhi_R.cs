using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    /// <summary>
    /// LBHI
    /// Branch if higher (unsigned)
    /// Long Branch If Higher
    /// RELATIVE
    /// IF (CC.Z = 0) AND (CC.C = 0) then PC’ ← PC + IMM
    /// SOURCE FORM         ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// LBHI address        RELATIVE            1022        5 (6)*      4
    /// *The 6809 requires 6 cycles only if the branch is taken.
    ///   [E F H I N Z V C]
    ///   [               ]
    /// </summary>
    /// <remarks>
    /// This instruction tests the Zero (Z) and Carry (C) flags in the CC register and, if both are zero, causes a relative branch. 
    /// If either the Z or C flags are set then the CPU continues executing the next instruction in sequence. 
    /// None of the Condition Code flags are affected by this instruction.
    /// 
    /// When used following a subtract or compare of unsigned binary values, the LBHI instruction will branch if the source value was higher than the original destination value.
    /// 
    /// LBHI is generally not useful following INC, DEC, LD, ST or TST instructions since none of those affect the Carry flag.
    /// 
    /// The branch address is calculated by adding the current value of the PC register (after the LBHI instruction bytes have been fetched) with the 16-bit twos-complement value contained in the third and fourth bytes of the instruction. 
    /// Long branch instructions permit a relative jump to any location within the 64K address space. 
    /// The smaller, faster BHI instruction can be used instead when the destination address is within -126 to +129 bytes of the address of the branch instruction.
    /// 
    /// See Also: BHI, LBGT, LBLS
    /// </remarks>
    public class _1022_LBhi_R : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            if (!(cpu.CC_C | cpu.CC_Z))
            {
                cpu.PC_REG += (ushort)(short)cpu.MemRead16(cpu.PC_REG);

                cycles += 1;
            }

            cpu.PC_REG += 2;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 5);
        public int Exec(IHD6309 cpu) => Exec(cpu, 5);
    }
}
