using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// LBRA
    /// Branch always
    /// Long Branch Always
    /// RELATIVE
    /// PC’ ← PC + IMM
    /// SOURCE FORM         ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// LBRA address        RELATIVE            16          5 / 4       3
    ///   [E F H I N Z V C]
    ///   [               ]
    /// </summary>
    /// <remarks>
    /// This instruction causes an unconditional relative branch. 
    /// None of the Condition Code flags are affected.
    /// 
    /// The LBRA instruction is similar in function to the JMP instruction in that it always causes execution to be transferred to the effective address specified by the operand. 
    /// The primary difference is that LBRA uses the Relative Addressing mode which allows the code to be position-independent.
    /// 
    /// The branch address is calculated by adding the current value of the PC register (after the LBRA instruction bytes have been fetched) with the 16-bit twos-complement value contained in the second and third bytes of the instruction. 
    /// Long branch instructions permit a relative jump to any location within the 64K address space. 
    /// The smaller, faster BRA instruction can be used when the destination address is within -126 to +129 bytes of the address of the branch instruction.
    /// 
    /// See Also: BRA, LBRN, JMP
    /// </remarks>
    public class _16_Lbra_R : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            var word = cpu.MemRead16(cpu.PC_REG);

            cpu.PC_REG += 2;
            cpu.PC_REG += word;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 5);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._54);
    }
}
