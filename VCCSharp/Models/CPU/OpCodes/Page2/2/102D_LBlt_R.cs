using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    /// <summary>
    /// LBLT
    /// Branch if less than (signed)
    /// Long Branch If Less Than Zero
    /// RELATIVE
    /// IF CC.N ≠ CC.V then PC’ ← PC + IMM
    /// SOURCE FORM         ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// LBLT address        RELATIVE            102D        5 (6)*      4
    /// *The 6809 requires 6 cycles only if the branch is taken.
    ///   [E F H I N Z V C]
    ///   [               ]
    /// </summary>
    /// <remarks>
    /// This instruction performs a relative branch if the values of the Negative (N) and Overflow (V) flags are not equal. 
    /// If the N and V flags have the same value then the CPU continues executing the next instruction in sequence. 
    /// None of the Condition Code flags are affected by this instruction.
    /// 
    /// When used following a subtract or compare of signed (twos-complement) values, the LBLT instruction will branch if the source value was less than the original destination value.
    /// 
    /// The branch address is calculated by adding the current value of the PC register (after the LBLT instruction bytes have been fetched) with the 16-bit twos-complement value contained in the third and fourth bytes of the instruction. 
    /// Long branch instructions permit a relative jump to any location within the 64K address space. 
    /// The smaller, faster BLT instruction can be used instead when the destination address is within -126 to +129 bytes of the address of the branch instruction.
    /// 
    /// See Also: BLT, LBGE, LBLO
    /// </remarks>
    public class _102D_LBlt_R : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            if (cpu.CC_V ^ cpu.CC_N)
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
