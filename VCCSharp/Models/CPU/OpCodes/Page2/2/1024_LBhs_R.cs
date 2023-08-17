using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    /// <summary>
    /// LBHS
    /// Branch if higher or same (unsigned)
    /// Long Branch If Higher or Same
    /// LBCC
    /// Branch if carry clear
    /// Long Branch If Carry Clear
    /// RELATIVE
    /// IF CC.C = 0 then PC’ ← PC + IMM
    /// SOURCE FORM         ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// LBCC address        RELATIVE            1024        5 (6)*      4
    /// *The 6809 requires 6 cycles only if the branch is taken.
    ///   [E F H I N Z V C]
    ///   [               ]
    /// </summary>
    /// <remarks>
    /// This instruction tests the Carry flag in the CC register and, if it is clear (0), causes a relative branch. 
    /// If the Carry flag is 1, the CPU continues executing the next instruction in sequence. 
    /// None of the Condition Code flags are affected by this instruction.
    /// 
    /// When used following a subtract or compare of unsigned binary values, the LBCC instruction will branch if the source value was higher or the same as the original destination value. 
    /// For this reason, 6809/6309 assemblers will accept LBHS as an alternate mnemonic for LBCC.
    /// 
    /// LBCC is generally not useful following INC, DEC, LD, ST or TST instructions since none of those affect the Carry flag. 
    /// Also, the LBCC instruction will always branch following a CLR instruction and never branch following a COM instruction due to the way those instructions affect the Carry flag.
    /// 
    /// The branch address is calculated by adding the current value of the PC register (after the LBCC instruction bytes have been fetched) with the 16-bit twos-complement value contained in the third and fourth bytes of the instruction. 
    /// Long branch instructions permit a relative jump to any location within the 64K address space. 
    /// The smaller, faster BCC instruction can be used instead when the destination address is within -126 to +129 bytes of the address of the branch instruction.
    /// 
    /// See Also: BHS, LBGE, LBLO
    /// See Also: BCC, LBCS, LBGE
    /// </remarks>
    public class _1024_LBhs_R : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            if (!cpu.CC_C)
            {
                cpu.PC_REG += (ushort)(short)cpu.MemRead16(cpu.PC_REG);

                cycles += 1;
            }

            cpu.PC_REG += 2;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 6);
        public int Exec(IHD6309 cpu) => Exec(cpu, 6);
    }
}
