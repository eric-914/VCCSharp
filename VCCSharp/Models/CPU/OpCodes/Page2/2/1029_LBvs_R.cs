using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    /// <summary>
    /// LBVS
    /// Branch if overflow set
    /// Branch if invalid 2's complement result 
    /// Long Branch If Overflow Set
    /// RELATIVE
    /// IF CC.V ≠ 0 then PC’ ← PC + IMM
    /// SOURCE FORM         ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// LBVS address        RELATIVE            1029        5 (6) *     4
    /// *The 6809 requires 6 cycles only if the branch is taken
    ///   [E F H I N Z V C]
    ///   [               ]
    /// </summary>
    /// <remarks>
    /// This instruction tests the Overflow (V) flag in the CC register and, if it is set (1), causes a relative branch. 
    /// If the V flag is clear, the CPU continues executing the next instruction in sequence. 
    /// None of the Condition Code flags are affected by this instruction.
    /// 
    /// When used following an operation on signed (twos-complement) binary values, the LBVS instruction will branch if an overflow occurred.
    /// 
    /// The branch address is calculated by adding the current value of the PC register (after the LBVS instruction bytes have been fetched) with the 16-bit twos-complement value contained in the third and fourth bytes of the instruction. 
    /// Long branch instructions permit a relative jump to any location within the 64K address space. 
    /// The smaller, faster BVS instruction can be used instead when the destination address is within -126 to +129 bytes of the address of the branch instruction.
    /// 
    /// See Also: BVS, LBVC
    /// </remarks>
    public class _1029_LBvs_R : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            if (cpu.CC_V)
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
