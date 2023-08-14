using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// ANDA
    /// And memory with accumulator
    /// Logically AND Memory Byte with Accumulator A or B
    /// IMMEDIATE
    /// r’ ← r AND (M)
    /// SOURCE FORM   ADDRESSING MODE     OPCODE       CYCLES      BYTE COUNT
    /// ANDA          IMMEDIATE           84           2           2
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ 0  ]
    /// </summary>
    /// <remarks>
    /// These instructions logically AND the contents of a byte in memory with either Accumulator A or B. 
    /// The 8-bit result is then placed in the specified accumulator.
    ///     N The Negative flag is set equal to the new value of bit 7 of the accumulator.
    ///     Z The Zero flag is set if the new value of the accumulator is zero; cleared otherwise.
    ///     V The Overflow flag is cleared by this instruction.
    ///     C The Carry flag is not affected by this instruction.
    /// The AND instructions are commonly used for clearing bits and for testing bits. 
    /// 
    /// Consider the following examples:
    ///     ANDA #%11101111 ;Clears bit 4 in A
    ///     ANDA #%00000100 ;Sets Z flag if bit 2 is not set
    /// 
    /// When testing bits, it is often preferable to use the BIT instructions instead, since they perform the same logical AND operation without modifying the contents of the accumulator.    
    /// 
    /// See Also: AIM, ANDCC, ANDD, ANDR, BAND, BIAND, BIT
    /// </remarks>
    public class _84_Anda_M : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            cpu.A_REG &= cpu.MemRead8(cpu.PC_REG++);

            cpu.CC_N = NTEST8(cpu.A_REG);
            cpu.CC_Z = ZTEST(cpu.A_REG);
            cpu.CC_V = false;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 2);
        public int Exec(IHD6309 cpu) => Exec(cpu, 2);
    }
}
