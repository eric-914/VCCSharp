using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// BSR
    /// Branch to subroutine 
    /// RELATIVE
    ///      S’ ← S - 2
    /// (S:S+1) ← PC
    ///     PC’ ← PC + IMM
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES     BYTE COUNT
    /// BSR address     RELATIVE            8D          7 / 6      2
    ///   [E F H I N Z V C]
    ///   [               ]
    /// </summary>
    /// <remarks>
    /// This instruction pushes the value of the PC register (after the BSR instruction bytes have been fetched) onto the hardware stack and then performs an unconditional relative branch. 
    /// None of the Condition Code flags are affected.
    /// 
    /// By pushing the PC value onto the stack, the called subroutine can "return" to this address after it has completed.
    /// 
    /// The BSR instruction is similar in function to the JSR instruction. 
    /// The significant difference is that BSR uses the Relative Addressing mode which implies that both the BSR instruction and the called subroutine may be contained in relocatable code, so long as both are contained in the same module.
    /// 
    /// The branch address is calculated by adding the current value of the PC register (after the BSR instruction bytes have been fetched) with the 8-bit twos-complement value contained in the second byte of the instruction. 
    /// The range of the branch destination is limited to -126 to +129 bytes from the address of the BSR instruction. 
    /// If a larger range is required then the LBSR instruction may be used instead.
    /// 
    /// See Also: JSR, LBSR, RTS
    /// </remarks>
    public class _8D_Bsr_R : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            var _postByte = cpu.MemRead8(cpu.PC_REG++);

            cpu.S_REG--;

            cpu.MemWrite8(cpu.PC_L, cpu.S_REG--);
            cpu.MemWrite8(cpu.PC_H, cpu.S_REG);

            cpu.PC_REG += (ushort)(sbyte)(_postByte);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 7);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._76);
    }
}
