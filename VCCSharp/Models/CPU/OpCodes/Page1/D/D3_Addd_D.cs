﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1.D
{
    /// <summary>
    /// ADDD
    /// Add memory to D accumulator
    /// Add Memory Word to 16-Bit Accumulator
    /// DIRECT
    /// r’ ← r + (M:M+1)
    /// SOURCE FORM   ADDRESSING MODE     OPCODE       CYCLES      BYTE COUNT
    /// ADDD          DIRECT              D3           6 / 4       2
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ ↕ ↕]
    /// </summary>
    /// <remarks>
    /// These instructions add the contents of a double-byte value in memory with one of the 16-bit accumulators (D,W). 
    /// The 16-bit result is placed back into the specified accumulator.
    ///     H The Half-Carry flag is not affected by these instructions.
    ///     N The Negative flag is set equal to the new value of bit 15 of the accumulator.
    ///     Z The Zero flag is set if the new accumulator value is zero; cleared otherwise.
    ///     V The Overflow flag is set if an overflow occurred; cleared otherwise.
    ///     C The Carry flag is set if a carry out of bit 15 occurred; cleared otherwise.
    /// The 16-bit ADD instructions are used for double-byte addition, and for addition of the least-significant word of multi-byte additions. 
    /// See the description of the ADCD instruction for an example of how 32-bit addition can be performed on a 6309 processor.
    /// 
    /// See Also: ADD (8-bit), ADDR
    /// </remarks>
    public class D3_Addd_D : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);
            ushort value = cpu.MemRead16(address);
            uint sum = (uint)(cpu.D_REG + value);

            cpu.CC_C = (sum & 0x10000) >> 16 != 0;
            cpu.CC_V = OVERFLOW16(cpu.CC_C, sum, value, cpu.D_REG);

            cpu.D_REG = (ushort)sum;

            cpu.CC_Z = ZTEST(cpu.D_REG);
            cpu.CC_N = NTEST16(cpu.D_REG);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 6);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._64);
    }
}
