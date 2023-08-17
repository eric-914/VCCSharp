using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// LBSR
    /// Branch to subroutine 
    /// Long Branch to Subroutine
    /// RELATIVE
    ///      S’ ← S - 2
    /// (S:S+1) ← PC
    ///     PC’ ← PC + IMM
    /// SOURCE FORM         ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// LBSR address        RELATIVE            17          9 / 7       3
    ///   [E F H I N Z V C]
    ///   [               ]
    /// </summary>
    /// <remarks>
    /// This instruction pushes the value of the PC register (after the LBSR instruction bytes have been fetched) onto the hardware stack and then performs an unconditional relative branch. 
    /// None of the Condition Code flags are affected.
    /// 
    /// By pushing the PC value onto the stack, the called subroutine can "return" to this address after it has completed.
    /// 
    /// The LBSR instruction is similar in function to the JSR instruction. 
    /// The primary difference is that LBSR uses the Relative Addressing mode which allows the code to be position-independent.
    /// 
    /// The branch address is calculated by adding the current value of the PC register (after the LBSR instruction bytes have been fetched) with the 16-bit twos-complement value contained in the second and third bytes of the instruction. 
    /// Long branch instructions permit a relative jump to any location within the 64K address space. 
    /// The smaller, faster BSR instruction can be used instead when the destination address is within -126 to +129 bytes of the address of the branch instruction.
    /// 
    /// See Also: BSR, JSR, PULS, RTS
    /// </remarks>
    internal class _17_Lbsr_R : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort word = cpu.MemRead16(cpu.PC_REG);

            cpu.PC_REG += 2;
            cpu.S_REG--;

            cpu.MemWrite8(cpu.PC_L, cpu.S_REG--);
            cpu.MemWrite8(cpu.PC_H, cpu.S_REG);

            cpu.PC_REG += word;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 9);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._97);
    }
}
