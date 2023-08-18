using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// RTS
    /// Return from subroutine
    /// INHERENT
    /// PC’ ← (S:S+1)
    ///  S’ ← S + 2
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// RTS             INHERENT            39          5 / 4       1
    ///   [E F H I N Z V C]
    ///   [               ]
    /// </summary>
    /// <remarks>
    /// This instruction pulls the double-byte value pointed to by the hardware stack pointer (S) and places it into the PC register. 
    /// No condition code flags are affected. 
    /// The effective result is the same as would be achieved using a PULS PC instruction.
    /// RTS is typically used to exit from a subroutine that was called via a BSR or JSR instruction. 
    /// Note, however, that a subroutine which preserves registers on entry by pushing them onto the stack, may opt to use a single PULS instruction to both restore the registers and return to the caller, as in:
    ///         ENTRY PSHS A,B,X ; Preserve registers
    ///         ...
    ///         ...
    ///         PULS A,B,X,PC    ; Restore registers and return
    ///         
    /// See Also: BSR, JSR, PULS, RTI
    /// </remarks>
    public class _39_Rts_I : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            cpu.PC_H = cpu.MemRead8(cpu.S_REG++);
            cpu.PC_L = cpu.MemRead8(cpu.S_REG++);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 5);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._51);
    }
}
