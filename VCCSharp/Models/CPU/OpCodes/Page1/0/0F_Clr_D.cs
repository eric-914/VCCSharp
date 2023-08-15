using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// CLR
    /// Clear accumulator or memory location
    /// Store Zero into a Memory Byte
    /// DIRECT
    /// (M) ← 0
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// CLR             DIRECT              0F          6 / 5       2 
    ///   [E F H I N Z V C]
    ///   [        0 1 0 0]
    /// </summary>
    /// <remarks>
    /// This instruction clears (sets to zero) the byte in memory at the Effective Address specified by the operand. 
    /// The Condition Code flags are also modified as follows:
    ///         N The Negative flag is cleared.
    ///         Z The Zero flag is set.
    ///         V The Overflow flag is cleared.
    ///         C The Carry flag is cleared.
    ///         
    /// The CPU performs a Read-Modify-Write sequence when this instruction is executed and is therefore slower than an instruction which only writes to memory. 
    /// When more than one byte needs to be cleared, you can optimize for speed by first clearing an accumulator and then using ST instructions to clear the memory bytes. 
    /// The following examples illustrate this optimization:
    /// 
    /// Executes in 21 cycles (NM=0):
    ///         CLR $200 ; 7 cycles
    ///         CLR $210 ; 7 cycles
    ///         CLR $220 ; 7 cycles
    ///         
    /// Adds one additional code byte, but saves 4 cycles:
    ///         CLRA ; 2 cycles
    ///         STA $200 ; 5 cycles
    ///         STA $210 ; 5 cycles
    ///         STA $220 ; 5 cycles
    ///         
    /// See Also: CLR (accumulator), ST
    /// </remarks>
    public class _0F_Clr_D : OpCode, IOpCode
    {
        public static int Exec(ICpuProcessor cpu, int cycles)
        {
            cpu.MemWrite8(0, cpu.DPADDRESS(cpu.PC_REG++));

            cpu.CC_Z = true;
            cpu.CC_N = false;
            cpu.CC_V = false;
            cpu.CC_C = false;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 6);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._65);
    }
}
