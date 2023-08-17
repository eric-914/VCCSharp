using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    /// <summary>
    /// LBMI
    /// Branch if minus 
    /// Long Branch If Minus
    /// RELATIVE
    /// IF CC.N ≠ 0 then PC’ ← PC + IMM
    /// SOURCE FORM         ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// LBMI address        RELATIVE            102B        5 (6) *     4
    /// *The 6809 requires 6 cycles only if the branch is taken.
    ///   [E F H I N Z V C]
    ///   [               ]
    /// </summary>
    /// <remarks>
    /// This instruction tests the Negative (N) flag in the CC register and, if it is set (1), causes a relative branch. 
    /// If the N flag is 0, the CPU continues executing the next instruction in sequence. 
    /// None of the Condition Code flags are affected by this instruction.
    /// 
    /// When used following an operation on signed (twos-complement) binary values, the LBMI instruction will branch if the resulting value is negative. 
    /// It is generally preferable to use the LBLT instruction following such an operation because the sign bit may be invalid due to a twos-complement overflow.
    /// 
    /// The branch address is calculated by adding the current value of the PC register (after the LBMI instruction bytes have been fetched) with the 16-bit twos-complement value contained in the third and fourth bytes of the instruction. 
    /// Long branch instructions permit a relative jump to any location within the 64K address space. 
    /// The smaller, faster BMI instruction can be used instead when the destination address is within -126 to +129 bytes of the address of the branch instruction.
    /// 
    /// See Also: BMI, LBLT, LBPL
    /// </remarks>
    public class _102B_LBmi_R : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            if (cpu.CC_N)
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
