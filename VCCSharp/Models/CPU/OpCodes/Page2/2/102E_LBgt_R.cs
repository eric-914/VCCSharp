using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    /// <summary>
    /// LBGT
    /// Branch if greater (signed) 
    /// Long Branch If Greater Than Zero
    /// RELATIVE
    /// IF (CC.N = CC.V) AND (CC.Z = 0) then PC’ ← PC + IMM
    /// SOURCE FORM         ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// LBGT address        RELATIVE            102E        5 (6)*      4
    /// *The 6809 requires 6 cycles only if the branch is taken.
    ///   [E F H I N Z V C]
    ///   [               ]
    /// </summary>
    /// <remarks>
    /// This instruction tests the Zero (Z) flag in the CC register and, if it is clear AND the values of the Negative (N) and Overflow (V) flags are equal (both set OR both clear), causes a relative branch. 
    /// If the N and V flags do not have the same value or if the Z flag is set then the CPU continues executing the next instruction in sequence. 
    /// None of the Condition Code flags are affected by this instruction.
    /// 
    /// When used following a subtract or compare of signed (twos-complement) values, the LBGT instruction will branch if the source value was greater than the original destination value.
    /// 
    /// The branch address is calculated by adding the current value of the PC register (after the LBGT instruction bytes have been fetched) with the 16-bit twos-complement value contained in the third and fourth bytes of the instruction. 
    /// Long branch instructions permit a relative jump to any location within the 64K address space. 
    /// The smaller, faster BGT instruction can be used instead when the destination address is within -126 to +129 bytes of the address of the branch instruction.
    /// 
    /// See Also: BGT, LBHI, LBLE
    /// </remarks>
    public class _102E_LBgt_R : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            if (!(cpu.CC_Z | (cpu.CC_N ^ cpu.CC_V)))
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
