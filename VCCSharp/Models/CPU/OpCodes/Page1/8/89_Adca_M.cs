using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    ///ADCA
    ///Add memory to accumulator with carry
    ///IMMEDIATE
    ///Add Memory Byte plus Carry with Accumulator A or B
    ///r’ ← r + (M) + C
    /// SOURCE FORM   ADDRESSING MODE     OPCODE       CYCLES      BYTE COUNT
    /// ADCA          IMMEDIATE           89           2           2 
    ///               DIRECT              99           4 / 3       2
    ///               INDEXED             A9           4+          2+ 
    ///               EXTENDED            B9           5 / 4       3
    /// ADCB          IMMEDIATE           C9           2           2
    ///               DIRECT              D9           4 / 3       2
    ///               INDEXED             E9           4+          2+ 
    ///               EXTENDED            F9           5 / 4       3
    ///   [E F H I N Z V C]
    ///   [    ↕   ↕ ↕ ↕ ↕]
    /// </summary>
    /// <remarks>
    /// These instructions add the contents of a byte in memory plus the contents of the Carry flag with either Accumulator A or B. 
    /// The 8-bit result is placed back into the specified accumulator.
    ///     H The Half-Carry flag is set if a carry into bit 4 occurred; cleared otherwise.
    ///     N The Negative flag is set equal to the new value of bit 7 of the accumulator.
    ///     Z The Zero flag is set if the new accumulator value is zero; cleared otherwise.
    ///     V The Overflow flag is set if an overflow occurred; cleared otherwise.
    ///     C The Carry flag is set if a carry out of bit 7 occurred; cleared otherwise.
    /// The ADC instruction is most often used to perform addition of the subsequent bytes of a multi-byte addition. 
    /// This allows the carry from a previous ADD or ADC instruction to be included when doing addition for the next higher-order byte.
    /// Since the 6x09 provides a 16-bit ADD instruction, it is not necessary to use the 8-bit ADD and ADC instructions for performing 16-bit addition.
    /// 
    /// See Also: ADCD, ADCR
    /// </remarks>
    public class _89_Adca_M : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            byte value = cpu.MemRead8(cpu.PC_REG++);
            ushort mask = (ushort)(cpu.A_REG + value + (cpu.CC_C ? 1 : 0));

            cpu.CC_C = (mask & 0x100) >> 8 != 0;
            cpu.CC_V = OVERFLOW8(cpu.CC_C, value, mask, cpu.A_REG);
            cpu.CC_H = ((cpu.A_REG ^ mask ^ value) & 0x10) >> 4 != 0;

            cpu.A_REG = (byte)mask;

            cpu.CC_N = NTEST8(cpu.A_REG);
            cpu.CC_Z = ZTEST(cpu.A_REG);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 2);
        public int Exec(IHD6309 cpu) => Exec(cpu, 2);
    }
}
