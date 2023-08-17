using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// JMP
    /// Jump
    /// Unconditional Jump
    /// DIRECT
    /// PC’ ← EA
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// JMP             DIRECT              0E          3 / 2       2 
    ///   [E F H I N Z V C]
    ///   [               ]
    /// </summary>
    /// <remarks>
    /// This instruction causes an unconditional jump. None of the Condition Code flags are affected by this instruction.
    /// 
    /// The JMP instruction is similar in function to the BRA and LBRA instructions in that it always causes execution to be transferred to the effective address specified by the operand. 
    /// The primary difference is that BRA and LBRA use only the Relative Addressing mode, whereas JMP uses only the Direct, Indexed or Extended modes.
    /// 
    /// Unlike most other instructions which use the Direct, Indexed and Extended addressing modes, the operand value used by the JMP instruction is the Effective Address itself, rather than the memory contents stored at that address (unless Indirect Indexing is used).
    /// 
    /// Here are some examples:
    ///         JMP $4000   ; Jumps to address $4000
    ///         JMP [$4000] ; Jumps to address stored at $4000
    ///         JMP ,X      ; Jumps to the address in X
    ///         JMP B,X     ; Jumps to computed address X + B
    ///         JMP [B,X]   ; Jumps to address stored at X + B
    ///         JMP <$80    ; Jumps to address (DP * $100) + $80
    ///         
    /// Indexed operands are useful in that they provide the ability to compute the destination address at run-time. 
    /// The use of an Indirect Indexing mode is frequently used to call routines through a jump-table in memory.
    /// 
    /// Using Direct or Extended operands with the JMP instruction should be avoided in position-independent code unless the destination address is within non-relocatable code (such as a ROM routine).
    /// 
    /// See Also: BRA, JSR, LBRA
    /// </remarks>
    public class _0E_Jmp_D : OpCode, IOpCode
    {
        public static int Exec(ICpuProcessor cpu, int cycles)
        {
            cpu.PC_REG = (ushort)(cpu.DP_REG | cpu.MemRead8(cpu.PC_REG));

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 3);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._32);
    }
}
