using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1.A
{
    /// <summary>
    /// JSR
    /// Jump to subroutine
    /// Unconditional Jump to Subroutine
    /// INDEXED
    ///      S’ ← S - 2
    /// (S:S+1) ← PC
    ///     PC’ ← EA
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// JSR             INDEXED             AD          7+ / 6+     2+ 
    ///   [E F H I N Z V C]
    ///   [               ]
    /// </summary>
    /// <remarks>
    /// This instruction pushes the value of the PC register (after the JSR instruction bytes have been fetched) onto the hardware stack and then performs an unconditional jump. 
    /// None of the Condition Code flags are affected. 
    /// By pushing the PC value onto the stack, the called subroutine can "return" to this address after it has completed.
    /// 
    /// The JSR instruction is similar in function to that of the BSR and LBSR instructions. 
    /// The  primary difference is that BSR and LBSR use only the Relative Addressing mode, whereas JSR uses only the Direct, Indexed or Extended modes.
    /// 
    /// Unlike most other instructions which use the Direct, Indexed and Extended addressing modes, the operand value used by the JSR instruction is the Effective Address itself, rather than the memory contents stored at that address (unless Indirect Indexing is used).
    /// 
    /// Here are some examples:
    ///         JSR $4000   ; Calls to address $4000
    ///         JSR [$4000] ; Calls to the address stored at $4000
    ///         JSR ,X      ; Calls to the address in X
    ///         JSR [B,X]   ; Calls to the address stored at X + B
    ///         
    /// Indexed operands are useful in that they provide the ability to compute the subroutine address at run-time. 
    /// The use of an Indirect Indexing mode is frequently used to call subroutines through a jump-table in memory.
    /// Using Direct or Extended operands with the JSR instruction should be avoided in position-independent code unless the destination address is within non-relocatable code (such as a ROM routine).
    /// 
    /// See Also: BSR, JMP, LBSR, PULS, RTS
    /// </remarks>
    public class AD_Jsr_X : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.INDADDRESS(cpu.PC_REG++);

            cpu.S_REG--;

            cpu.MemWrite8(cpu.PC_L, cpu.S_REG--);
            cpu.MemWrite8(cpu.PC_H, cpu.S_REG);

            cpu.PC_REG = address;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 7);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._76);
    }
}
