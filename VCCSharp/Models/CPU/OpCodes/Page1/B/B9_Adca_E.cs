﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1.B
{
    /// <summary>
    ///ADCA
    ///Add memory to accumulator with carry
    ///EXTENDED
    ///Add Memory Byte plus Carry with Accumulator A or B
    ///r’ ← r + (M) + C
    /// SOURCE FORM   ADDRESSING MODE     OPCODE       CYCLES      BYTE COUNT
    /// ADCA          EXTENDED            B9           5 / 4       3
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
    public class B9_Adca_E : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.MemRead16(cpu.PC_REG);
            byte value = cpu.MemRead8(address);
            ushort sum = (ushort)(cpu.A_REG + value + (cpu.CC_C ? 1 : 0));

            cpu.CC_C = (sum & 0x100) >> 8 != 0;
            cpu.CC_V = OVERFLOW8(cpu.CC_C, value, sum, cpu.A_REG);
            cpu.CC_H = ((cpu.A_REG ^ sum ^ value) & 0x10) >> 4 != 0;

            cpu.A_REG = (byte)sum;

            cpu.CC_N = NTEST8(cpu.A_REG);
            cpu.CC_Z = ZTEST(cpu.A_REG);

            cpu.PC_REG += 2;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 5);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._54);
    }
}
