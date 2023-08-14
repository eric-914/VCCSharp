﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// ADDA
    /// Add memory to accumulator
    /// Add Memory Byte to 8-Bit Accumulator
    /// DIRECT
    /// r’ ← r + (M)
    /// SOURCE FORM   ADDRESSING MODE     OPCODE       CYCLES      BYTE COUNT
    /// ADDA          DIRECT              9B           4 / 3       2 
    ///   [E F H I N Z V C]
    ///   [    ↕   ↕ ↕ ↕ ↕]
    /// </summary>
    /// <remarks>
    /// These instructions add the contents of a byte in memory with one of the 8-bit accumulators (A,B,E,F). 
    /// The 8-bit result is placed back into the specified accumulator.
    ///     H The Half-Carry flag is set if a carry into bit 4 occurred; cleared otherwise.
    ///     N The Negative flag is set equal to the new value of bit 7 of the accumulator.
    ///     Z The Zero flag is set if the new accumulator value is zero; cleared otherwise.
    ///     V The Overflow flag is set if an overflow occurred; cleared otherwise.
    ///     C The Carry flag is set if a carry out of bit 7 occurred; cleared otherwise.
    /// The 8-bit ADD instructions are used for single-byte addition, and for addition of the least-significant byte in multi-byte additions. 
    /// Since the 6x09 also provides a 16-bit ADD instruction, it is not necessary to use the 8-bit ADD and ADC instructions for performing 16-bit addition.
    /// 
    /// See Also: ADD (16-bit), ADDR
    /// </remarks>
    public class _9B_Adda_D : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);
            byte _postByte = cpu.MemRead8(address);
            ushort _temp16 = (ushort)(cpu.A_REG + _postByte);

            cpu.CC_C = (_temp16 & 0x100) >> 8 != 0;
            cpu.CC_H = ((cpu.A_REG ^ _postByte ^ _temp16) & 0x10) >> 4 != 0;
            cpu.CC_V = OVERFLOW8(cpu.CC_C, _postByte, _temp16, cpu.A_REG);

            cpu.A_REG = (byte)_temp16;

            cpu.CC_N = NTEST8(cpu.A_REG);
            cpu.CC_Z = ZTEST(cpu.A_REG);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 4);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._43);
    }
}
