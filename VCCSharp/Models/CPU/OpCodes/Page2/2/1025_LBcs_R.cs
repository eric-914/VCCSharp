using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    /// <summary>
    /// LBCS
    /// Branch if carry set 
    /// Long Branch If Carry Set
    /// LBLO
    /// Branch if lower (unsigned)
    /// Long Branch If Lower
    /// RELATIVE
    /// IF CC.C ≠ 0 then PC’ ← PC + IMM
    /// SOURCE FORM         ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// LBCS address        RELATIVE            1025        5 (6)*      4
    /// *The 6809 requires 6 cycles only if the branch is taken.
    ///   [E F H I N Z V C]
    ///   [               ]
    /// </summary>
    /// <remarks>
    /// This instruction tests the Carry flag in the CC register and, if it is set (1), causes a relative branch. 
    /// If the Carry flag is 0, the CPU continues executing the next instruction in sequence. 
    /// None of the Condition Code flags are affected by this instruction.
    /// 
    /// When used following a subtract or compare of unsigned binary values, the LBCS instruction will branch if the source value was lower than the original destination value.
    /// For this reason, 6809/6309 assemblers will accept LBLO as an alternate mnemonic for LBCS.
    /// 
    /// LBCS is generally not useful following INC, DEC, LD, ST or TST instructions since none of those affect the Carry flag. 
    /// Also, the LBCS instruction will never branch following a CLR instruction and always branch following a COM instruction due to the way those instructions affect the Carry flag.
    /// 
    /// The branch address is calculated by adding the current value of the PC register (after the LBCS instruction bytes have been fetched) with the 16-bit twos-complement value contained in the third and fourth bytes of the instruction. 
    /// Long branch instructions permit a relative jump to any location within the 64K address space. 
    /// The smaller, faster BCS instruction can be used instead when the destination address is within -126 to +129 bytes of the address of the branch instruction.
    /// 
    /// See Also: BCS, LBCC, LBLT
    /// See Also: BLO, LBHS, LBLT
    /// </remarks>
    public class _1025_LBcs_R : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            if (cpu.CC_C)
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
