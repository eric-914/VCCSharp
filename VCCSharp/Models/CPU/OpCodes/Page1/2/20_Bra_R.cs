using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// BRA
    /// Branch always
    /// RELATIVE
    /// PC’ ← PC + IMM
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES     BYTE COUNT
    /// BRA address     RELATIVE            20          3          2
    ///   [E F H I N Z V C]
    ///   [               ]
    /// </summary>
    /// <remarks>
    /// This instruction causes an unconditional relative branch. 
    /// None of the Condition Code flags are affected.
    /// 
    /// The BRA instruction is similar in function to the JMP instruction in that it always causes execution to be transferred to the effective address specified by the operand. 
    /// The primary difference is that BRA uses the Relative Addressing mode which allows the code to be position-independent.
    /// 
    /// The branch address is calculated by adding the current value of the PC register (after the BRA instruction bytes have been fetched) with the 8-bit twos-complement value contained in the second byte of the instruction. 
    /// The range of the branch destination is limited to -126 to +129 bytes from the address of the BPL instruction. 
    /// If a larger range is required then the LBRA instruction may be used instead.
    /// 
    /// See Also: BRN, JMP, LBRA
    /// </remarks>
    public class _20_Bra_R : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            sbyte offset = (sbyte)cpu.MemRead8(cpu.PC_REG++);

            cpu.PC_REG += (ushort)offset;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 3);
        public int Exec(IHD6309 cpu) => Exec(cpu, 3);
    }
}
